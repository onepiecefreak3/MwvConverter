using System.Runtime.Serialization;

namespace Logic.Domain.WavManagement.Contract.Exceptions
{
    [Serializable]
    public class WavReaderException : Exception
    {
        public WavReaderException()
        {
        }

        public WavReaderException(string message) : base(message)
        {
        }

        public WavReaderException(string message, Exception inner) : base(message, inner)
        {
        }

        protected WavReaderException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
