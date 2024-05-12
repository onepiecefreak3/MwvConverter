using Logic.Domain.Kuriimu2.KomponentAdapter.Contract;
using Logic.Domain.Level5Management.InternalContract.Audio.DataClasses;
using Logic.Domain.WavManagement.Contract;
using Logic.Domain.WavManagement.Contract.DataClasses;

namespace Logic.Domain.Level5Management.Audio
{
    internal class MwvComposer : IWavComposer<MwvData>
    {
        private readonly IBinaryFactory _binaryFactory;

        public MwvComposer(IBinaryFactory binaryFactory)
        {
            _binaryFactory = binaryFactory;
        }

        public WavData Compose(MwvData data)
        {
            var result = new WavChunk[2 + (data.RemainingChunks?.Length ?? 0)];

            for (var i = 0; i < result.Length - 2; i++)
                result[i] = data.RemainingChunks![i];

            result[^2] = ComposePredictorCoeffs(data.PredictorCoefficients);
            result[^1] = ComposeFrames(data.Frames);

            return new WavData
            {
                Format = data.Format,
                Chunks = result
            };
        }

        private WavChunk ComposePredictorCoeffs(int[][] coefficients)
        {
            var output = new MemoryStream();

            using IBinaryWriterX bw = _binaryFactory.CreateWriter(output, true);

            bw.Write(3);
            bw.Write(32);

            for (var i = 0; i < 32; i++)
            {
                for (var j = 0; j < 3; j++)
                    bw.Write(i < coefficients.Length ? coefficients[i][j] : 0);
            }

            return new WavChunk
            {
                Identifier = "pflt",
                Data = output
            };
        }

        private WavChunk ComposeFrames(MwvFrameData[] frames)
        {
            var output = new MemoryStream();

            using IBinaryWriterX bw = _binaryFactory.CreateWriter(output, true);

            foreach (MwvFrameData frame in frames)
            {
                var frameHeader = (short)(frame.NegScaleIndex & 0x1F);
                frameHeader |= (short)((frame.PosScaleIndex & 0x1F) << 5);
                frameHeader |= (short)((frame.CoefficientIndex & 0x1F) << 10);

                bw.Write(frameHeader);

                for (var i = 0; i < frame.Samples.Length; i += 2)
                {
                    var frameByte = (byte)((frame.Samples[i] << 4) | frame.Samples[i + 1]);

                    bw.Write(frameByte);
                }
            }

            return new WavChunk
            {
                Identifier = "data",
                Data = output
            };
        }
    }
}
