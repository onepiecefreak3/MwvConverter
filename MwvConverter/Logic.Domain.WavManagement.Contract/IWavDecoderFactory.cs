using CrossCutting.Core.Contract.Aspects;
using Logic.Domain.WavManagement.Contract.Exceptions;

namespace Logic.Domain.WavManagement.Contract
{
    [MapException(typeof(WavDecoderFactoryException), "Error in WAV decoder factory.")]
    public interface IWavDecoderFactory
    {
        IWavDecoder GetDecoder(int format);
    }
}
