using CrossCutting.Core.Contract.Aspects;
using Logic.Domain.WavManagement.Contract.Exceptions;

namespace Logic.Domain.WavManagement.Contract
{
    [MapException(typeof(WavEncoderFactoryException), "Error in WAV encoder factory.")]
    public interface IWavEncoderFactory
    {
        IWavEncoder GetEncoder(int format);
    }
}
