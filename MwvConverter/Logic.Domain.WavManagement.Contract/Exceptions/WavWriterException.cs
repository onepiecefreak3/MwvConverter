using System.Runtime.Serialization;

namespace Logic.Domain.WavManagement.Contract.Exceptions
{
    [Serializable]
    public class WavWriterException : Exception
    {
        public WavWriterException()
        {
        }

        public WavWriterException(string message) : base(message)
        {
        }

        public WavWriterException(string message, Exception inner) : base(message, inner)
        {
        }

        protected WavWriterException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
