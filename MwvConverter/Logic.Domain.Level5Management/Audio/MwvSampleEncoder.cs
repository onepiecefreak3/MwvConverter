using Logic.Domain.Level5Management.InternalContract.Audio.DataClasses;
using Logic.Domain.WavManagement.Contract;
using Logic.Domain.WavManagement.Contract.DataClasses;

/*  mwv_commission,  by Znullptr for 'onepiecefreak'
 *  C version and encoding bugfix by dominic@phoboslab
 *
 *  Copyright (c) 2024, Dominic Szablewski - https://phoboslab.org
 *  SPDX-License-Identifier: MIT
 *
 *  Converted to C# by onepiecefreak
 */

namespace Logic.Domain.Level5Management.Audio
{
    internal class MwvSampleEncoder : ISampleEncoder<MwvData>, IWavEncoder
    {
        private static readonly int[] Scales = {
            0x00001000, 0x0000144E, 0x000019C5, 0x000020B4, 0x00002981, 0x000034AC, 0x000042D9, 0x000054D6,
            0x00006BAB, 0x000088A4, 0x0000AD69, 0x0000DC13, 0x0001174C, 0x00016275, 0x0001C1D8, 0x00023AE5,
            0x0002D486, 0x0003977E, 0x00048EEE, 0x0005C8F3, 0x00075779, 0x0009513E, 0x000BD31C, 0x000F01B5,
            0x00130B82, 0x00182B83, 0x001EAC92, 0x0026EDB2, 0x00316777, 0x003EB2E6, 0x004F9232, 0x0064FBD1
        };

        // These are the same 4 fixed predictors as used by FLAC, 
        // represented in .12 fixed point.
        // Reference for frame order calculation: https://github.com/SerenityOS/serenity/blob/master/Userland/Libraries/LibAudio/FlacLoader.cpp#L877-L896
        private static readonly int[][] DEFAULT_COEFFS = new int[][]
        {
            new[] {      0,     0,     0 }, // s_0(t) = 0
            new[] {  -4096,     0,     0 }, // s_1(t) = s(t-1)
            new[] {  -8192,  4096,     0 }, // s_2(t) = 2s(t-1) - s(t-2)
            new[] { -12288, 12288, -4096 }  // s_3(t) = 3s(t-1) - 3s(t-2) + s(t-3);
        };

        private readonly IWavComposer<MwvData> _composer;

        public int[] SupportedFormats { get; } = { 0x555 };

        public MwvSampleEncoder(IWavComposer<MwvData> composer)
        {
            _composer = composer;
        }

        public WavData Encode(DecodedWavData data)
        {
            MwvData mwvData = EncodeSamples(data);
            return _composer.Compose(mwvData);
        }

        public MwvData EncodeSamples(DecodedWavData data)
        {
            // LMS (Least Mean Square) algorithm to efficiently calculate the error of predictions:
            // https://www.codeproject.com/Articles/1000084/Least-Mean-Square-Algorithm-using-Cplusplus

            var frames = new MwvFrameData[data.Samples.Length / 32];

            short hist1 = 0, hist2 = 0, hist3 = 0;

            var buf_residuals = new byte[32];

            for (var f = 0; f < frames.Length; f++)
            {
                ReadOnlySpan<byte> nibbles = new byte[] { 8, 9, 10, 11, 12, 13, 14, 15, 0, 1, 2, 3, 4, 5, 6, 7 };

                var best_residuals = new byte[32];
                short best_hist1 = 0, best_hist2 = 0, best_hist3 = 0;

                var best_error = double.MaxValue;
                var best_cf = 0;
                var best_sf = 0;

                // Brute force the default coeffs;
                for (int cf = 0; cf < DEFAULT_COEFFS.Length; cf++)
                {
                    // Brute forcing the negative and positive scale factors separately is
                    // kinda pointless, as they are usually very close together.
                    // Therefore assume the same value for negative and positive scale factors.
                    // For even better performance:
                    // 1. start with the highest sf and go down to zero; stop as soon
                    //    as the cur_error is worse than best_error (local optimal)
                    for (int sf = 0; sf < 32; sf++)
                    {
                        short cur_hist1 = hist1, cur_hist2 = hist2, cur_hist3 = hist3;

                        double cur_error = 0;

                        for (int si = 0; si < 32; si++)
                        {
                            int predicted =
                                cur_hist1 * DEFAULT_COEFFS[cf][0] +
                                cur_hist2 * DEFAULT_COEFFS[cf][1] +
                                cur_hist3 * DEFAULT_COEFFS[cf][2];

                            int sample = data.Samples[f * 32 + si];
                            int sf_enc = Scales[sf];

                            // Dividing by float and rounding the result is vital for better quality!
                            // int encoded = clamp(((sample << 12) + predicted) / sf_enc, -8, 7);
                            var encoded = (int)Math.Clamp(Math.Round(((sample << 12) + predicted) / (float)sf_enc), -8, 7);
                            buf_residuals[si] = nibbles[encoded + 8];

                            // Careful, we might use a different sf for decoding than for
                            // encoding, if the sample was negative, but rounded to zero!
                            int sf_dec = Scales[sf];
                            var decoded = (short)Math.Clamp((encoded * sf_dec - predicted) >> 12, short.MinValue, short.MaxValue);

                            cur_hist3 = cur_hist2;
                            cur_hist2 = cur_hist1;
                            cur_hist1 = decoded;

                            double err = sample - decoded;
                            cur_error += err * err;
                        }

                        if (cur_error < best_error)
                        {
                            best_hist1 = cur_hist1;
                            best_hist2 = cur_hist2;
                            best_hist3 = cur_hist3;

                            best_error = cur_error;
                            best_cf = cf;
                            best_sf = sf;

                            Array.Copy(buf_residuals, best_residuals, 32);
                        }
                    }
                }

                frames[f] = new MwvFrameData
                {
                    CoefficientIndex = best_cf,
                    NegScaleIndex = best_sf,
                    PosScaleIndex = best_sf,
                    Samples = best_residuals
                };

                hist1 = best_hist1;
                hist2 = best_hist2;
                hist3 = best_hist3;
            }

            return new MwvData
            {
                Format = new FormatData
                {
                    Format = 0x555,
                    ChannelCount = data.Format.ChannelCount,
                    SampleRate = data.Format.SampleRate,
                    AvgBytesPerSec = (int)((float)data.Format.SampleRate / 0x20 * 0x12),
                    BlockAlign = 0x12,
                    BitsPerSample = 0x10
                },
                PredictorCoefficients = DEFAULT_COEFFS,
                Frames = frames,
                RemainingChunks = data.Chunks
            };
        }
    }
}
