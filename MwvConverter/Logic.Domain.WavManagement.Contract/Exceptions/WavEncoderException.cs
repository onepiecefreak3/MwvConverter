using System.Runtime.Serialization;

namespace Logic.Domain.WavManagement.Contract.Exceptions
{
    [Serializable]
    public class WavEncoderException : Exception
    {
        public WavEncoderException()
        {
        }

        public WavEncoderException(string message) : base(message)
        {
        }

        public WavEncoderException(string message, Exception inner) : base(message, inner)
        {
        }

        protected WavEncoderException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
