using CrossCutting.Core.Contract.Bootstrapping;
using CrossCutting.Core.Contract.Configuration;
using CrossCutting.Core.Contract.DependencyInjection;
using CrossCutting.Core.Contract.DependencyInjection.DataClasses;
using CrossCutting.Core.Contract.EventBrokerage;
using Logic.Business.MwvManagement.Contract;
using Logic.Business.MwvManagement.InternalContract;

namespace Logic.Business.MwvManagement
{
    public class WmvManagementActivator : IComponentActivator
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
            kernel.Register<IMwvManagementWorkflow, MwvManagementWorkflow>(ActivationScope.Unique);

            kernel.Register<IMwvManagementConfigurationValidator, MwvManagementConfigurationValidator>(ActivationScope.Unique);

            kernel.RegisterConfiguration<MwvManagementConfiguration>();
        }

        public void AddMessageSubscriptions(IEventBroker broker)
        {
        }

        public void Configure(IConfigurator configurator)
        {
        }
    }
}
