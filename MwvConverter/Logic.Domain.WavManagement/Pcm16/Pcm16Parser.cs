using Logic.Domain.Kuriimu2.KomponentAdapter.Contract;
using Logic.Domain.WavManagement.Contract;
using Logic.Domain.WavManagement.Contract.DataClasses;
using Logic.Domain.WavManagement.InternalContract.Pcm16.DataClasses;

namespace Logic.Domain.WavManagement.Pcm16
{
    internal class Pcm16Parser : IWavParser<Pcm16Data>
    {
        private readonly IBinaryFactory _binaryFactory;

        public Pcm16Parser(IBinaryFactory binaryFactory)
        {
            _binaryFactory = binaryFactory;
        }

        public Pcm16Data Parse(WavData data)
        {
            var result = new Pcm16Data
            {
                Format = data.Format
            };

            var remainingChunks = new List<WavChunk>();
            foreach (WavChunk chunk in data.Chunks)
            {
                switch (chunk.Identifier)
                {
                    case "data":
                        result.Samples = ReadSamples(chunk.Data);
                        break;

                    default:
                        remainingChunks.Add(chunk);
                        break;
                }
            }

            result.RemainingChunks = remainingChunks.ToArray();

            return result;
        }

        private short[] ReadSamples(Stream dataStream)
        {
            var result = new short[dataStream.Length / 2];

            using IBinaryReaderX br = _binaryFactory.CreateReader(dataStream, true);

            var sampleIndex = 0;
            while (dataStream.Position < dataStream.Length)
                result[sampleIndex++] = br.ReadInt16();

            return result;
        }
    }
}
