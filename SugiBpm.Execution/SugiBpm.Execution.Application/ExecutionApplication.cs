using AutoMapper;
using Microsoft.Practices.ServiceLocation;
using Serilog;
using SugiBpm.Definition.Domain;
using SugiBpm.Delegation.Interface;
using SugiBpm.Delegation.Interface.Execution;
using SugiBpm.Delegation.Interface.Organization;
using SugiBpm.Execution.Application.ViewModel;
using SugiBpm.Execution.Domain;
using SunStone.Data;
using SunStone.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Application
{
    public class ExecutionApplication : IExecutionApplication
    {
        public ProcessInstanceView StartProcessInstance(Guid actorId,Guid processDefinitionId, IDictionary<string, object> attributeValues = null, string transitionName = null)
        {
            ProcessInstance processInstance = null;
            using (var scope = new UnitOfWorkScope(autoCommit:false))
            {
                try
                {
                    var processDefinitionRepository = ServiceLocator.Current.GetInstance<IRepository<ProcessDefinition>>();
                    ProcessDefinition processDefinition = processDefinitionRepository.With(w => w.Nodes).With(w=>w.Attributes).Get(processDefinitionId);

                    var organizationApplication = ServiceLocator.Current.GetInstance<IOrganizationApplication>();
                    var actor = organizationApplication.FindActor(actorId);
                    ExecutionContext executionContext = new ExecutionContext(actor, processDefinition);
                    processInstance = executionContext.StartProcess(attributeValues);

                    Log.Information("actor '" + actor + "' starts an instance of process '" + processDefinition.Name + "'...");

                    var processInstanceRepository = ServiceLocator.Current.GetInstance<IRepository<ProcessInstance>>();
                    processInstanceRepository.Add(processInstance);

                    scope.Commit();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ProcessInstance, ProcessInstanceView>());
                    var map = config.CreateMapper();
                    return map.Map<ProcessInstanceView>(processInstance);
                }
                catch
                {
                    throw;
                }
            }
        }

        public void PerformActivity(Guid actorId,Guid flowId, IDictionary<string, object> attributeValues = null, string transitionName = null)
        {
            using (var scope = new UnitOfWorkScope(autoCommit: false))
            {
                try
                {
                    var organizationApplication = ServiceLocator.Current.GetInstance<IOrganizationApplication>();
                    var actor = organizationApplication.FindActor(actorId);

                    var flowRepository = ServiceLocator.Current.GetInstance<IRepository<Flow>>();
                    var flow =
                        flowRepository.With(w => w.Node)
                        .With(w => w.Parent)
                        .With(w => w.AttributeInstances)
                        .Single(s => s.Id == flowId);

                    var processInstanceRepository = ServiceLocator.Current.GetInstance<IRepository<ProcessInstance>>();
                    var processInstance = processInstanceRepository.Get(flow.ProcessInstanceId);

                    var processDefinitionRepository = ServiceLocator.Current.GetInstance<IRepository<ProcessDefinition>>();
                    ProcessDefinition processDefinition = processDefinitionRepository.With(w => w.Nodes).Get(processInstance.ProcessDefinitionId);

                    ExecutionContext executionContext = new ExecutionContext(actor, processDefinition, processInstance, flow);
                    executionContext.PerformActivity(attributeValues);

                    scope.Commit();
                }
                catch
                {
                    throw;
                }
            }
        }

        public IList<FlowView> GetTaskList(Guid actorId)
        {
            List<Flow> tasks = null;
            ProcessInstance[] processInstances = null;
            using (var scope = new UnitOfWorkScope())
            {
                var organizationApplication = ServiceLocator.Current.GetInstance<IOrganizationApplication>();
                var actor = organizationApplication.FindActor(actorId);

                var flowRepository = ServiceLocator.Current.GetInstance<IRepository<Flow>>();
                var processInstanceRepository = ServiceLocator.Current.GetInstance<IRepository<ProcessInstance>>();
                
                if (actor is IUser)
                {
                    tasks = flowRepository
                        .With(w => w.Node)
                        .Query(new Specification<Flow>(s => s.ActorId == actor.Id)).ToList();

                    var processInstanceIds = tasks.Select(s => s.ProcessInstanceId).ToArray();
                    processInstances =
                    processInstanceRepository.With(w=>w.ProcessDefinition)
                        .Query(new Specification<ProcessInstance>(s => processInstanceIds.Contains(s.Id)))
                        .ToArray();
                }
                else if (actor is IGroup)
                {

                }
            }

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Flow, FlowView>()
                    .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.Node.Name))
                    .AfterMap((src, dest) => dest.ProcessDefinitionName = processInstances.Single(s => s.Id == src.ProcessInstanceId).ProcessDefinition.Name);
            });
            return Mapper.Map<List<FlowView>>(tasks);
        }

        public FlowView GetFlow(Guid flowId)
        {
            using (var scope = new UnitOfWorkScope())
            {
                try
                {
                    var flowRepository = ServiceLocator.Current.GetInstance<IRepository<Flow>>();
                    var processInstanceRepository = ServiceLocator.Current.GetInstance<IRepository<ProcessInstance>>();

                    var flow = flowRepository.Get(flowId);
                    var processInstance = processInstanceRepository.With(w => w.ProcessDefinition).Get(flow.ProcessInstanceId);


                    Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<Flow, FlowView>()
                            .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.Node.Name))
                            .AfterMap((src, dest) => dest.ProcessDefinitionName = processInstance.ProcessDefinition.Name);
                    });

                    return Mapper.Map<FlowView>(flow);
                }
                catch
                {
                    throw;
                }
            }
        }

        public ActivityFormView GetStartForm(Guid processDefinitionId)
        {
            ActivityFormView activityForm = null;
            using (var scope = new UnitOfWorkScope())
            {
                try
                {
                    var processDefinitionRepository = ServiceLocator.Current.GetInstance<IRepository<ProcessDefinition>>();
                    ProcessDefinition processDefinition = processDefinitionRepository.With(w => w.Nodes).With(w => w.Attributes).Get(processDefinitionId);

                    var fieldRepository = ServiceLocator.Current.GetInstance<IRepository<Field>>();
                    var fields = fieldRepository.With(w => w.Attribute).Query(new Specification<Field>(q => q.State.Id == processDefinition.StartStateId));
                    var attributeValues = new Dictionary<string, object>();

                    foreach (var field in fields)
                    {
                        var attribute = field.Attribute;
                        string attributeName = attribute.Name;
                        string initialValue = attribute.InitialValue;
                        if (!string.IsNullOrEmpty(initialValue) && (field.Access.IsReadable() || field.Access.IsWritable()))
                        {
                            AttributeInstance attributeInstance = new AttributeInstance(attribute, null);
                            attributeInstance.ValueText = initialValue;
                            attributeValues.Add(attributeName, attributeInstance.GetValue());
                        }
                    }

                    activityForm = new ActivityFormView();
                    activityForm.ProcessDefinitionId = processDefinition.Id;
                    activityForm.AttributeValues = attributeValues;

                    return activityForm;
                }
                catch
                {
                    throw;
                }
            }
        }

        public ActivityFormView GetActivityForm(Guid flowId)
        {
            ActivityFormView activityForm = null;
            using (var scope = new UnitOfWorkScope())
            {
                try
                {
                    var flowRepository = ServiceLocator.Current.GetInstance<IRepository<Flow>>();
                    Flow flow = flowRepository.With(w => w.Node).Get(flowId);
                    State state = flow.Node as State;

                    var fieldRepository = ServiceLocator.Current.GetInstance<IRepository<Field>>();
                    var fields = fieldRepository.With(w => w.Attribute).Query(new Specification<Field>(q => q.State.Id == state.Id));
                    var attributeValues = new Dictionary<string, object>();

                    var attributeInstanceRepository = ServiceLocator.Current.GetInstance<IRepository<AttributeInstance>>();
                    foreach (var field in fields)
                    {
                        var attribute = field.Attribute;
                        string attributeName = attribute.Name;
                        if ((field.Access.IsReadable() || field.Access.IsWritable()))
                        {
                            AttributeInstance attributeInstance = attributeInstanceRepository.SingleOrDefault(s => s.Scope.Id == flowId && s.AttributeName == attributeName);
                            if(attributeInstance != null)                            
                                attributeValues.Add(attributeName, attributeInstance.GetValue());
                        }
                    }

                    activityForm = new ActivityFormView();
                    activityForm.FlowId = flowId;
                    activityForm.AttributeValues = attributeValues;
                    return activityForm;
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
