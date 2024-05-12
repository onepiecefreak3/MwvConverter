using System.Runtime.Serialization;

namespace Logic.Domain.WavManagement.Contract.Exceptions
{
    [Serializable]
    public class WavComposerException : Exception
    {
        public WavComposerException()
        {
        }

        public WavComposerException(string message) : base(message)
        {
        }

        public WavComposerException(string message, Exception inner) : base(message, inner)
        {
        }

        protected WavComposerException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
