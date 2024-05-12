namespace Logic.Domain.WavManagement.Contract.DataClasses
{
    public class DecodedWavData
    {
        public DecodedFormatData Format { get; set; }
        public short[] Samples { get; set; }
        public WavChunk[] Chunks { get; set; }
    }
}
