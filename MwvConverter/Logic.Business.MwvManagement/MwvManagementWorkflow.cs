using Logic.Business.MwvManagement.Contract;
using Logic.Business.MwvManagement.InternalContract;
using Logic.Domain.WavManagement.Contract;
using Logic.Domain.WavManagement.Contract.DataClasses;

namespace Logic.Business.MwvManagement
{
    internal class MwvManagementWorkflow : IMwvManagementWorkflow
    {
        private readonly MwvManagementConfiguration _config;
        private readonly IMwvManagementConfigurationValidator _configValidator;
        private readonly IWavReader _reader;
        private readonly IWavWriter _writer;
        private readonly IWavDecoderFactory _decoderFactory;
        private readonly IWavEncoderFactory _encoderFactory;

        public MwvManagementWorkflow(MwvManagementConfiguration config, IMwvManagementConfigurationValidator configValidator,
            IWavReader reader, IWavWriter writer,
            IWavDecoderFactory decoderFactory, IWavEncoderFactory encoderFactory)
        {
            _config = config;
            _configValidator = configValidator;
            _reader = reader;
            _writer = writer;
            _decoderFactory = decoderFactory;
            _encoderFactory = encoderFactory;
        }

        public int Execute()
        {
            if (_config.ShowHelp || Environment.GetCommandLineArgs().Length <= 0)
            {
                PrintHelp();
                return 0;
            }

            _configValidator.Validate(_config);

            switch (_config.Operation)
            {
                case "d":
                    DecodeWavs();
                    break;

                case "e":
                    EncodeMwvs();
                    break;
            }

            return 0;
        }

        private void DecodeWavs()
        {
            // Collect files to decode
            bool isDirectory = Directory.Exists(_config.FilePath);
            string[] files = isDirectory ?
                Directory.GetFiles(_config.FilePath, "*.mwv", SearchOption.AllDirectories) :
                new[] { _config.FilePath };

            // Decode files
            foreach (string file in files)
            {
                Console.Write($"Decode {file}: ");
                try
                {
                    DecodeWav(file);
                    Console.WriteLine("Ok");
                }
                catch (Exception e)
                {
                    string relativePath = isDirectory ?
                        Path.GetRelativePath(_config.FilePath, file) :
                        Path.GetFileName(file);
                    Console.WriteLine($"Could not decode {relativePath}: {GetInnermostException(e).Message}");
                }
            }
        }

        private void DecodeWav(string filePath)
        {
            // Read WAV data
            using Stream fileStream = File.OpenRead(filePath);

            WavData wavData = _reader.Read(fileStream);

            // Decode WAV data
            IWavDecoder decoder = _decoderFactory.GetDecoder(wavData.Format.Format);
            DecodedWavData decodedData = decoder.Decode(wavData);

            // Encode PCM WAV data
            IWavEncoder encoder = _encoderFactory.GetEncoder(1);
            WavData pcmWavData = encoder.Encode(decodedData);

            // Write PCM WAV data
            using Stream newFileStream = File.Create(filePath + ".wav");

            _writer.Write(pcmWavData, newFileStream);
        }

        private void EncodeMwvs()
        {
            // Collect files to encode
            bool isDirectory = Directory.Exists(_config.FilePath);
            string[] files = isDirectory ?
                Directory.GetFiles(_config.FilePath, "*.wav", SearchOption.AllDirectories) :
                new[] { _config.FilePath };

            // Encode files
            foreach (string file in files)
            {
                Console.Write($"Encode {file}: ");
                try
                {
                    EncodeMwv(file);
                    Console.WriteLine("Ok");
                }
                catch (Exception e)
                {
                    string relativePath = isDirectory ?
                        Path.GetRelativePath(_config.FilePath, file) :
                        Path.GetFileName(file);
                    Console.WriteLine($"Could not encode {relativePath}: {GetInnermostException(e).Message}");
                }
            }
        }

        private void EncodeMwv(string filePath)
        {
            // Read PCM WAV data
            using Stream fileStream = File.OpenRead(filePath);

            WavData pcmWavData = _reader.Read(fileStream);

            // Decode PCM WAV data
            IWavDecoder decoder = _decoderFactory.GetDecoder(pcmWavData.Format.Format);
            DecodedWavData decodedData = decoder.Decode(pcmWavData);

            // Encode WAV data
            IWavEncoder encoder = _encoderFactory.GetEncoder(0x555);
            WavData wavData = encoder.Encode(decodedData);

            // Write WAV data
            using Stream newFileStream = File.Create(filePath + ".mwv");

            _writer.Write(wavData, newFileStream);
        }

        private void PrintHelp()
        {
            Console.WriteLine("Following commands exist:");
            Console.WriteLine("  -h, --help\t\tShows this help message.");
            Console.WriteLine("  -o, --operation\tThe operation to take on the file");
            Console.WriteLine("    Valid operations are: d for decode, e for encode");
            Console.WriteLine("  -f, --file\t\tThe file to process");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine($"\tDecode mwv to pcm wav: {Environment.ProcessPath} -o d -f Path/To/File.mwv");
            Console.WriteLine($"\tEncode pcm wav to mwv: {Environment.ProcessPath} -o e -f Path/To/File.wav");
        }

        private Exception GetInnermostException(Exception e)
        {
            while (e.InnerException != null)
                e = e.InnerException;

            return e;
        }
    }
}
