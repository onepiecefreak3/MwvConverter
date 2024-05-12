using Logic.Domain.WavManagement.Contract;

namespace Logic.Domain.WavManagement
{
    internal class WavDecoderFactory : IWavDecoderFactory
    {
        private readonly IDictionary<int, IWavDecoder> _decoderLookup;

        public WavDecoderFactory(IEnumerable<IWavDecoder> decoders)
        {
            _decoderLookup = new Dictionary<int, IWavDecoder>();

            foreach (IWavDecoder decoder in decoders)
            {
                foreach (int format in decoder.SupportedFormats)
                {
                    if (_decoderLookup.ContainsKey(format))
                        continue;

                    _decoderLookup[format] = decoder;
                }
            }
        }

        public IWavDecoder GetDecoder(int format)
        {
            if (!_decoderLookup.TryGetValue(format, out IWavDecoder? decoder))
                throw new InvalidOperationException($"No WAV decoder for format {format} found.");

            return decoder;
        }
    }
}
