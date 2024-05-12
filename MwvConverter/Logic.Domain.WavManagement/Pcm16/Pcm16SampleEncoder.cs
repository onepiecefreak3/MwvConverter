using Logic.Domain.WavManagement.Contract;
using Logic.Domain.WavManagement.InternalContract.Pcm16.DataClasses;
using Logic.Domain.WavManagement.Contract.DataClasses;

namespace Logic.Domain.WavManagement.Pcm16
{
    internal class Pcm16SampleEncoder : ISampleEncoder<Pcm16Data>, IWavEncoder
    {
        private readonly IWavComposer<Pcm16Data> _composer;

        public int[] SupportedFormats { get; } = { 1 };

        public Pcm16SampleEncoder(IWavComposer<Pcm16Data> composer)
        {
            _composer = composer;
        }

        public WavData Encode(DecodedWavData sampleData)
        {
            Pcm16Data data = EncodeSamples(sampleData);
            return _composer.Compose(data);
        }

        public Pcm16Data EncodeSamples(DecodedWavData sampleData)
        {
            return new Pcm16Data
            {
                Format = new FormatData
                {
                    Format = 1,
                    ChannelCount = sampleData.Format.ChannelCount,
                    SampleRate = sampleData.Format.SampleRate,
                    AvgBytesPerSec = sampleData.Format.SampleRate * 2,
                    BlockAlign = 2,
                    BitsPerSample = 16
                },
                Samples = sampleData.Samples,
                RemainingChunks = sampleData.Chunks
            };
        }
    }
}
