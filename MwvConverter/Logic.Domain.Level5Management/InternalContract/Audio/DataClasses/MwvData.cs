using Logic.Domain.WavManagement.Contract.DataClasses;

namespace Logic.Domain.Level5Management.InternalContract.Audio.DataClasses
{
    public class MwvData
    {
        public FormatData Format { get; set; }
        public int[][] PredictorCoefficients { get; set; }
        public MwvFrameData[] Frames { get; set; }
        public WavChunk[] RemainingChunks { get; set; }
    }
}
