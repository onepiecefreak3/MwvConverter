using Logic.Domain.Level5Management.InternalContract.Audio.DataClasses;
using Logic.Domain.WavManagement.Contract;
using Logic.Domain.WavManagement.Contract.DataClasses;

namespace Logic.Domain.Level5Management.Audio
{
    internal class MwvSampleDecoder : ISampleDecoder<MwvData>, IWavDecoder
    {
        private static readonly int[] Scales = {
            0x00001000, 0x0000144E, 0x000019C5, 0x000020B4, 0x00002981, 0x000034AC, 0x000042D9, 0x000054D6,
            0x00006BAB, 0x000088A4, 0x0000AD69, 0x0000DC13, 0x0001174C, 0x00016275, 0x0001C1D8, 0x00023AE5,
            0x0002D486, 0x0003977E, 0x00048EEE, 0x0005C8F3, 0x00075779, 0x0009513E, 0x000BD31C, 0x000F01B5,
            0x00130B82, 0x00182B83, 0x001EAC92, 0x0026EDB2, 0x00316777, 0x003EB2E6, 0x004F9232, 0x0064FBD1
        };

        private readonly IWavParser<MwvData> _parser;

        public int[] SupportedFormats { get; } = { 0x555 };

        public MwvSampleDecoder(IWavParser<MwvData> parser)
        {
            _parser = parser;
        }

        public DecodedWavData Decode(WavData data)
        {
            MwvData mwvData = _parser.Parse(data);
            return DecodeSamples(mwvData);
        }

        public DecodedWavData DecodeSamples(MwvData data)
        {
            ReadOnlySpan<sbyte> nibbles = new sbyte[] { 0, 1, 2, 3, 4, 5, 6, 7, -8, -7, -6, -5, -4, -3, -2, -1 };

            var samples = new short[data.Frames.Length * 32];
            var sampleIndex = 0;

            short hist1 = 0;
            short hist2 = 0;
            short hist3 = 0;

            foreach (MwvFrameData frame in data.Frames)
            {
                int coef1 = data.PredictorCoefficients[frame.CoefficientIndex][0];
                int coef2 = data.PredictorCoefficients[frame.CoefficientIndex][1];
                int coef3 = data.PredictorCoefficients[frame.CoefficientIndex][2];

                foreach (byte rawSample in frame.Samples)
                {
                    sbyte sample = nibbles[rawSample];
                    int prediction = -(hist1 * coef1 + hist2 * coef2 + hist3 * coef3);

                    int fullSample = sample >= 0 ?
                        (prediction + sample * Scales[frame.PosScaleIndex]) >> 12 :
                        (prediction + sample * Scales[frame.NegScaleIndex]) >> 12;

                    samples[sampleIndex] = (short)Math.Clamp(fullSample, short.MinValue, short.MaxValue);

                    hist3 = hist2;
                    hist2 = hist1;
                    hist1 = samples[sampleIndex++];
                }
            }

            return new DecodedWavData
            {
                Format = new DecodedFormatData
                {
                    ChannelCount = data.Format.ChannelCount,
                    SampleRate = data.Format.SampleRate
                },
                Samples = samples,
                Chunks = data.RemainingChunks
            };
        }
    }
}
