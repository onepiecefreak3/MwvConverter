using System.Runtime.Serialization;

namespace Logic.Domain.WavManagement.Contract.Exceptions
{
    [Serializable]
    public class WavDecoderException:Exception
    {
        public WavDecoderException()
        {
        }

        public WavDecoderException(string message) : base(message)
        {
        }

        public WavDecoderException(string message, Exception inner) : base(message, inner)
        {
        }

        protected WavDecoderException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
