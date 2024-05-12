using Logic.Domain.WavManagement.Contract.DataClasses;
using CrossCutting.Core.Contract.Aspects;
using Logic.Domain.WavManagement.Contract.Exceptions;

namespace Logic.Domain.WavManagement.Contract
{
    [MapException(typeof(WavEncoderException), "Error in WAV encoder.")]
    public interface IWavEncoder
    {
        int[] SupportedFormats { get; }

        WavData Encode(DecodedWavData data);
    }
}
