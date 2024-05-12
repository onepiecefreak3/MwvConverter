using Logic.Domain.WavManagement.Contract.DataClasses;

namespace Logic.Domain.WavManagement.InternalContract.Pcm16.DataClasses
{
    public class Pcm16Data
    {
        public FormatData Format { get; set; }
        public short[] Samples { get; set; }
        public WavChunk[]? RemainingChunks { get; set; }
    }
}
