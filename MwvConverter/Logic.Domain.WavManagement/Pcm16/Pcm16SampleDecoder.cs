using Logic.Domain.WavManagement.Contract;
using Logic.Domain.WavManagement.Contract.DataClasses;
using Logic.Domain.WavManagement.InternalContract.Pcm16.DataClasses;

namespace Logic.Domain.WavManagement.Pcm16
{
    internal class Pcm16SampleDecoder : ISampleDecoder<Pcm16Data>, IWavDecoder
    {
        private readonly IWavParser<Pcm16Data> _parser;

        public int[] SupportedFormats { get; } = { 1 };

        public Pcm16SampleDecoder(IWavParser<Pcm16Data> parser)
        {
            _parser = parser;
        }

        public DecodedWavData Decode(WavData data)
        {
            Pcm16Data pcmData = _parser.Parse(data);
            return DecodeSamples(pcmData);
        }

        public DecodedWavData DecodeSamples(Pcm16Data data)
        {
            return new DecodedWavData
            {
                Format = new DecodedFormatData
                {
                    ChannelCount = data.Format.ChannelCount,
                    SampleRate = data.Format.SampleRate
                },
                Samples = data.Samples,
                Chunks = data.RemainingChunks ?? Array.Empty<WavChunk>()
            };
        }
    }
}
