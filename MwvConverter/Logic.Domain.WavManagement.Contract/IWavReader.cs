using CrossCutting.Core.Contract.Aspects;
using Logic.Domain.WavManagement.Contract.DataClasses;
using Logic.Domain.WavManagement.Contract.Exceptions;

namespace Logic.Domain.WavManagement.Contract
{
    [MapException(typeof(WavReaderException), "Error in WAV reader.")]
    public interface IWavReader
    {
        WavData Read(Stream input);
    }
}
