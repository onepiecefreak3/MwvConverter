using System.Runtime.Serialization;

namespace Logic.Domain.WavManagement.Contract.Exceptions
{
    [Serializable]
    public class WavDecoderFactoryException : Exception
    {
        public WavDecoderFactoryException()
        {
        }

        public WavDecoderFactoryException(string message) : base(message)
        {
        }

        public WavDecoderFactoryException(string message, Exception inner) : base(message, inner)
        {
        }

        protected WavDecoderFactoryException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
