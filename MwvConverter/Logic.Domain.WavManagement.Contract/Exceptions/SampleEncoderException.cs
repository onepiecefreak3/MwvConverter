using System.Runtime.Serialization;

namespace Logic.Domain.WavManagement.Contract.Exceptions
{
    [Serializable]
    public class SampleEncoderException : Exception
    {
        public SampleEncoderException()
        {
        }

        public SampleEncoderException(string message) : base(message)
        {
        }

        public SampleEncoderException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SampleEncoderException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
