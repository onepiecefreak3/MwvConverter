namespace Logic.Domain.Level5Management.InternalContract.Audio.DataClasses
{
    public struct MwvFrameData
    {
        public int CoefficientIndex { get; set; }
        public int PosScaleIndex { get; set; }
        public int NegScaleIndex { get; set; }

        public byte[] Samples { get; set; }
    }
}
