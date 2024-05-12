using CrossCutting.Core.Contract.Aspects;
using Logic.Domain.WavManagement.Contract.DataClasses;
using Logic.Domain.WavManagement.Contract.Exceptions;

namespace Logic.Domain.WavManagement.Contract
{
    [MapException(typeof(SampleDecoderException), "Error in sample decoder.")]
    public interface ISampleDecoder<in TData>
    {
        DecodedWavData DecodeSamples(TData data);
    }
}
