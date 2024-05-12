using CrossCutting.Core.Contract.Aspects;
using Logic.Domain.WavManagement.Contract.DataClasses;
using Logic.Domain.WavManagement.Contract.Exceptions;

namespace Logic.Domain.WavManagement.Contract
{
    [MapException(typeof(SampleEncoderException), "Error in sample encoder.")]
    public interface ISampleEncoder<out TData>
    {
        TData EncodeSamples(DecodedWavData data);
    }
}
