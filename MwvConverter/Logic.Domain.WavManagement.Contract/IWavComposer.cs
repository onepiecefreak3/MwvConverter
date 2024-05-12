using CrossCutting.Core.Contract.Aspects;
using Logic.Domain.WavManagement.Contract.DataClasses;
using Logic.Domain.WavManagement.Contract.Exceptions;

namespace Logic.Domain.WavManagement.Contract
{
    [MapException(typeof(WavComposerException), "Error in WAV composer.")]
    public interface IWavComposer<in TData>
    {
        WavData Compose(TData data);
    }
}
