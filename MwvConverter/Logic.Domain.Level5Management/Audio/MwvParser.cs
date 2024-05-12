using Logic.Domain.Kuriimu2.KomponentAdapter.Contract;
using Logic.Domain.Level5Management.InternalContract.Audio.DataClasses;
using Logic.Domain.WavManagement.Contract;
using Logic.Domain.WavManagement.Contract.DataClasses;

namespace Logic.Domain.Level5Management.Audio
{
    internal class MwvParser : IWavParser<MwvData>
    {
        private readonly IBinaryFactory _binaryFactory;

        public MwvParser(IBinaryFactory binaryFactory)
        {
            _binaryFactory = binaryFactory;
        }

        public MwvData Parse(WavData data)
        {
            var result = new MwvData
            {
                Format = data.Format
            };

            var remainingChunks = new List<WavChunk>();
            foreach (WavChunk chunk in data.Chunks)
            {
                switch (chunk.Identifier)
                {
                    case "pflt":
                        result.PredictorCoefficients = ReadPredictorCoeffs(chunk.Data);
                        break;

                    case "data":
                        result.Frames = ReadFrames(chunk.Data);
                        break;

                    default:
                        remainingChunks.Add(chunk);
                        break;
                }
            }

            result.RemainingChunks = remainingChunks.ToArray();

            return result;
        }

        private int[][] ReadPredictorCoeffs(Stream pfltData)
        {
            using IBinaryReaderX br = _binaryFactory.CreateReader(pfltData, true);

            int filterOrder = br.ReadInt32();
            int filterCount = br.ReadInt32();

            if (filterOrder != 3 || pfltData.Length < 8 + filterCount * 4 * filterOrder)
                throw new InvalidOperationException("Invalid prediction coefficient length.");

            var result = new int[filterCount][];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = new int[filterOrder];

                for (var j = 0; j < filterOrder; j++)
                    result[i][j] = br.ReadInt32();
            }

            return result;
        }

        private MwvFrameData[] ReadFrames(Stream data)
        {
            using IBinaryReaderX br = _binaryFactory.CreateReader(data, true);

            var result = new List<MwvFrameData>();

            while (data.Position < data.Length)
            {
                short frameHeader = br.ReadInt16();

                int negScale = frameHeader & 0x1F;
                int posScale = (frameHeader >> 5) & 0x1F;
                int coefIndex = (frameHeader >> 10) & 0x1F;

                var samples = new byte[32];

                for (var i = 0; i < 16; i++)
                {
                    byte frameByte = br.ReadByte();

                    // Get 2 signed samples from byte
                    byte sample1 = (byte)(frameByte >> 4);
                    byte sample2 = (byte)(frameByte & 0xF);

                    samples[i * 2] = sample1;
                    samples[i * 2 + 1] = sample2;
                }

                result.Add(new MwvFrameData
                {
                    CoefficientIndex = coefIndex,
                    PosScaleIndex = posScale,
                    NegScaleIndex = negScale,
                    Samples = samples
                });
            }

            return result.ToArray();
        }
    }
}
