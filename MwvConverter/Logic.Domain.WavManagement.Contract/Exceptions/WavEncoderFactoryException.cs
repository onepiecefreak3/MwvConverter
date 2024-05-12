using System.Runtime.Serialization;

namespace Logic.Domain.WavManagement.Contract.Exceptions
{
    [Serializable]
    public class WavEncoderFactoryException : Exception
    {
        public WavEncoderFactoryException()
        {
        }

        public WavEncoderFactoryException(string message) : base(message)
        {
        }

        public WavEncoderFactoryException(string message, Exception inner) : base(message, inner)
        {
        }

        protected WavEncoderFactoryException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
