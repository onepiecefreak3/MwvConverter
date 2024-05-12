using System.Runtime.Serialization;

namespace Logic.Domain.WavManagement.Contract.Exceptions
{
    [Serializable]
    public class SampleDecoderException : Exception
    {
        public SampleDecoderException()
        {
        }

        public SampleDecoderException(string message) : base(message)
        {
        }

        public SampleDecoderException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SampleDecoderException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
