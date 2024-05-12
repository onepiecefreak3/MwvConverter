using CrossCutting.Core.Contract.EventBrokerage;
using CrossCutting.Core.Contract.Messages;
using CrossCutting.Core.Contract.DependencyInjection;
using Logic.Business.MwvManagement.Contract;
using MwvConverter;

KernelLoader loader = new();
ICoCoKernel kernel = loader.Initialize();

var eventBroker = kernel.Get<IEventBroker>();
eventBroker.Raise(new InitializeApplicationMessage());

var mainLogic = kernel.Get<IMwvManagementWorkflow>();
return mainLogic.Execute();
