using CrossCutting.Core.Contract.Bootstrapping;
using CrossCutting.Core.Contract.Configuration;
using CrossCutting.Core.Contract.DependencyInjection;
using CrossCutting.Core.Contract.DependencyInjection.DataClasses;
using CrossCutting.Core.Contract.EventBrokerage;
using Logic.Domain.Level5Management.Audio;
using Logic.Domain.Level5Management.InternalContract.Audio.DataClasses;
using Logic.Domain.WavManagement.Contract;

namespace Logic.Domain.Level5Management
{
    public class Level5ManagementActivator : IComponentActivator
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
            kernel.Register<IWavParser<MwvData>, MwvParser>(ActivationScope.Unique);
            kernel.Register<ISampleDecoder<MwvData>, MwvSampleDecoder>(ActivationScope.Unique);
            kernel.Register<ISampleEncoder<MwvData>, MwvSampleEncoder>(ActivationScope.Unique);
            kernel.Register<IWavComposer<MwvData>, MwvComposer>(ActivationScope.Unique);

            kernel.Register<IWavDecoder, MwvSampleDecoder>(ActivationScope.Unique);
            kernel.Register<IWavEncoder, MwvSampleEncoder>(ActivationScope.Unique);

            kernel.RegisterConfiguration<Level5ManagementConfiguration>();
        }

        public void AddMessageSubscriptions(IEventBroker broker)
        {
        }

        public void Configure(IConfigurator configurator)
        {
        }
    }
}
