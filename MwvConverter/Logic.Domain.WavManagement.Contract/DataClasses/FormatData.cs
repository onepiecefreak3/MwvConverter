namespace Logic.Domain.WavManagement.Contract.DataClasses
{
    public class FormatData
    {
        public short Format { get; set; }
        public short ChannelCount { get; set; }
        public int SampleRate { get; set; }
        public int AvgBytesPerSec { get; set; }
        public short BlockAlign { get; set; }
        public short BitsPerSample { get; set; }
    }
}
