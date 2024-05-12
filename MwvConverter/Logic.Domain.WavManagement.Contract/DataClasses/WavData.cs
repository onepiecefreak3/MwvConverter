namespace Logic.Domain.WavManagement.Contract.DataClasses
{
    public class WavData
    {
        public FormatData Format { get; set; }
        public WavChunk[] Chunks { get; set; }
    }
}
