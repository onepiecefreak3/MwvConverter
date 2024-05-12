using System.Runtime.Serialization;

namespace Logic.Domain.WavManagement.Contract.Exceptions
{
    [Serializable]
    public class WavParserException : Exception
    {
        public WavParserException()
        {
        }

        public WavParserException(string message) : base(message)
        {
        }

        public WavParserException(string message, Exception inner) : base(message, inner)
        {
        }

        protected WavParserException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
