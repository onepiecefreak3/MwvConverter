using Logic.Domain.Kuriimu2.KomponentAdapter.Contract;
using Logic.Domain.WavManagement.Contract;
using Logic.Domain.WavManagement.Contract.DataClasses;

namespace Logic.Domain.WavManagement
{
    internal class WavReader : IWavReader
    {
        private readonly IStreamFactory _streamFactory;
        private readonly IBinaryFactory _binaryFactory;

        public WavReader(IStreamFactory streamFactory, IBinaryFactory binaryFactory)
        {
            _streamFactory = streamFactory;
            _binaryFactory = binaryFactory;
        }

        public WavData Read(Stream input)
        {
            using IBinaryReaderX br = _binaryFactory.CreateReader(input, true);

            if (br.ReadString(4) != "RIFF")
                throw new InvalidOperationException("Invalid WAV file.");

            int wavSize = br.ReadInt32();

            if (br.ReadString(4) != "WAVE")
                throw new InvalidOperationException("Invalid WAV data.");

            FormatData? formatData = null;

            var result = new List<WavChunk>();
            while (input.Position < wavSize + 8)
            {
                string identifier = br.ReadString(4);
                int size = br.ReadInt32();

                switch (identifier)
                {
                    case "fmt ":
                        Stream formatStream = _streamFactory.CreateSubStream(input, input.Position, size);
                        formatData = ReadFormat(formatStream);
                        break;

                    default:
                        result.Add(new WavChunk
                        {
                            Identifier = identifier,
                            Data = _streamFactory.CreateSubStream(input, input.Position, size)
                        });
                        break;
                }

                input.Position += size;
            }

            if (formatData == null)
                throw new InvalidOperationException("WAV did not include a format chunk.");

            return new WavData
            {
                Format = formatData,
                Chunks = result.ToArray()
            };
        }

        private FormatData ReadFormat(Stream fmtData)
        {
            using IBinaryReaderX br = _binaryFactory.CreateReader(fmtData, false);

            return new FormatData
            {
                Format = br.ReadInt16(),
                ChannelCount = br.ReadInt16(),
                SampleRate = br.ReadInt32(),
                AvgBytesPerSec = br.ReadInt32(),
                BlockAlign = br.ReadInt16(),
                BitsPerSample = br.ReadInt16()
            };
        }
    }
}
