using Logic.Domain.Kuriimu2.KomponentAdapter.Contract;
using Logic.Domain.WavManagement.Contract;
using Logic.Domain.WavManagement.Contract.DataClasses;
using Logic.Domain.WavManagement.InternalContract.Pcm16.DataClasses;

namespace Logic.Domain.WavManagement.Pcm16
{
    internal class Pcm16Composer : IWavComposer<Pcm16Data>
    {
        private readonly IBinaryFactory _binaryFactory;

        public Pcm16Composer(IBinaryFactory binaryFactory)
        {
            _binaryFactory = binaryFactory;
        }

        public WavData Compose(Pcm16Data data)
        {
            var result = new WavChunk[1 + (data.RemainingChunks?.Length ?? 0)];

            for (var i = 0; i < result.Length - 1; i++)
                result[i] = data.RemainingChunks![i];

            result[^1] = ComposeDataChunk(data.Samples);

            return new WavData
            {
                Format = data.Format,
                Chunks = result
            };
        }

        private WavChunk ComposeDataChunk(short[] samples)
        {
            var dataStream = new MemoryStream();

            using IBinaryWriterX bw = _binaryFactory.CreateWriter(dataStream, true);

            foreach (short sample in samples)
                bw.Write(sample);

            return new WavChunk
            {
                Identifier = "data",
                Data = dataStream
            };
        }
    }
}
