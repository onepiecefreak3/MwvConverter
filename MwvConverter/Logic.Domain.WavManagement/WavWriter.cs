using System.Text;
using Logic.Domain.Kuriimu2.KomponentAdapter.Contract;
using Logic.Domain.WavManagement.Contract;
using Logic.Domain.WavManagement.Contract.DataClasses;

namespace Logic.Domain.WavManagement
{
    internal class WavWriter : IWavWriter
    {
        private readonly IBinaryFactory _binaryFactory;

        public WavWriter(IBinaryFactory binaryFactory)
        {
            _binaryFactory = binaryFactory;
        }

        public void Write(WavData data, Stream output)
        {
            using IBinaryWriterX bw = _binaryFactory.CreateWriter(output, true);

            output.Position = 0xC;

            WriteFormat(bw, data.Format);

            foreach (WavChunk chunk in data.Chunks)
                WriteChunk(bw, chunk);

            output.Position = 0;

            bw.WriteString("RIFF", Encoding.ASCII, false, false);
            bw.Write((int)output.Length - 8);
            bw.WriteString("WAVE", Encoding.ASCII, false, false);
        }

        private void WriteFormat(IBinaryWriterX bw, FormatData formatData)
        {
            bw.WriteString("fmt ", Encoding.ASCII, false, false);
            bw.Write(0x10);

            bw.Write(formatData.Format);
            bw.Write(formatData.ChannelCount);
            bw.Write(formatData.SampleRate);
            bw.Write(formatData.AvgBytesPerSec);
            bw.Write(formatData.BlockAlign);
            bw.Write(formatData.BitsPerSample);
        }

        private void WriteChunk(IBinaryWriterX bw, WavChunk chunk)
        {
            bw.WriteString(chunk.Identifier, Encoding.ASCII, false, false);
            bw.Write((int)chunk.Data.Length);

            chunk.Data.Position = 0;
            chunk.Data.CopyTo(bw.BaseStream);
        }
    }
}
