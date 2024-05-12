using CrossCutting.Core.Contract.Aspects;
using Logic.Domain.WavManagement.Contract.DataClasses;
using Logic.Domain.WavManagement.Contract.Exceptions;

namespace Logic.Domain.WavManagement.Contract
{
    [MapException(typeof(WavParserException), "Error in WAV parser.")]
    public interface IWavParser<out TData>
    {
        TData Parse(WavData data);
    }
}
