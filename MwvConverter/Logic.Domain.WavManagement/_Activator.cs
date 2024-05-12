using CrossCutting.Core.Contract.Bootstrapping;
using CrossCutting.Core.Contract.Configuration;
using CrossCutting.Core.Contract.DependencyInjection;
using CrossCutting.Core.Contract.DependencyInjection.DataClasses;
using CrossCutting.Core.Contract.EventBrokerage;
using Logic.Domain.WavManagement.Contract;
using Logic.Domain.WavManagement.InternalContract.Pcm16.DataClasses;
using Logic.Domain.WavManagement.Pcm16;

namespace Logic.Domain.WavManagement
{
    public class WavManagementActivator : IComponentActivator
    {
        public void Activating()
        {
        }

        public void Activated()
        {
        }

        public void Deactivating()
        {
        }

        public void Deactivated()
        {
        }

        public void Register(ICoCoKernel kernel)
        {
            kernel.Register<IWavReader, WavReader>(ActivationScope.Unique);
            kernel.Register<IWavWriter, WavWriter>(ActivationScope.Unique);

            kernel.Register<IWavDecoderFactory, WavDecoderFactory>(ActivationScope.Unique);
            kernel.Register<IWavEncoderFactory, WavEncoderFactory>(ActivationScope.Unique);

            kernel.Register<IWavParser<Pcm16Data>, Pcm16Parser>(ActivationScope.Unique);
            kernel.Register<ISampleDecoder<Pcm16Data>, Pcm16SampleDecoder>(ActivationScope.Unique);
            kernel.Register<ISampleEncoder<Pcm16Data>, Pcm16SampleEncoder>(ActivationScope.Unique);
            kernel.Register<IWavComposer<Pcm16Data>, Pcm16Composer>(ActivationScope.Unique);

            kernel.Register<IWavDecoder, Pcm16SampleDecoder>(ActivationScope.Unique);
            kernel.Register<IWavEncoder, Pcm16SampleEncoder>(ActivationScope.Unique);

            kernel.RegisterConfiguration<Configuration>();
        }

        public void AddMessageSubscriptions(IEventBroker broker)
        {
        }

        public void Configure(IConfigurator configurator)
        {
        }
    }
}
