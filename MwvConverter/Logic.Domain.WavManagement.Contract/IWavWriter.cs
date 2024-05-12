using CrossCutting.Core.Contract.Aspects;
using Logic.Domain.WavManagement.Contract.DataClasses;
using Logic.Domain.WavManagement.Contract.Exceptions;

namespace Logic.Domain.WavManagement.Contract
{
    [MapException(typeof(WavWriterException), "Error in WAV writer.")]
    public interface IWavWriter
    {
        void Write(WavData data, Stream output);
    }
}
