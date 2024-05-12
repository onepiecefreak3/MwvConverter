using Logic.Domain.WavManagement.Contract;

namespace Logic.Domain.WavManagement
{
    internal class WavEncoderFactory : IWavEncoderFactory
    {
        private readonly IDictionary<int, IWavEncoder> _encoderLookup;

        public WavEncoderFactory(IEnumerable<IWavEncoder> encoders)
        {
            _encoderLookup = new Dictionary<int, IWavEncoder>();

            foreach (IWavEncoder encoder in encoders)
            {
                foreach (int format in encoder.SupportedFormats)
                {
                    if (_encoderLookup.ContainsKey(format))
                        continue;

                    _encoderLookup[format] = encoder;
                }
            }
        }

        public IWavEncoder GetEncoder(int format)
        {
            if (!_encoderLookup.TryGetValue(format, out IWavEncoder? encoder))
                throw new InvalidOperationException($"No WAV encoder for format {format} found.");

            return encoder;
        }
    }
}
