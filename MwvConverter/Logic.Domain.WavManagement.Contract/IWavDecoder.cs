using CrossCutting.Core.Contract.Aspects;
using Logic.Domain.WavManagement.Contract.DataClasses;
using Logic.Domain.WavManagement.Contract.Exceptions;

namespace Logic.Domain.WavManagement.Contract
{
    [MapException(typeof(WavDecoderException), "Error in WAV decoder.")]
    public interface IWavDecoder
    {
        int[] SupportedFormats { get; }

        DecodedWavData Decode(WavData data);
    }
}
