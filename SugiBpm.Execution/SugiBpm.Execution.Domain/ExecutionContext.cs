using Microsoft.Practices.ServiceLocation;
using SugiBpm.Definition.Domain;
using SugiBpm.Delegation.Interface;
using SunStone.Data;
using SunStone.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SugiBpm.Delegation.Interface.Definition;
using SugiBpm.Delegation.Interface.Execution;
using SugiBpm.Delegation.Interface.Organization;
using Serilog;

namespace SugiBpm.Execution.Domain
{
    public class ExecutionContext : IActionContext,IDecisionContext,IForkContext,IJoinContext,IAssignmentContext
    {
        private ProcessDefinition processDefintion = null;
        public ProcessInstance ProcessInstance { get; set; }

        public Flow Flow { get; set; }
        public Node Node { get; set; }
        public IActor Actor { get; set; }
        public IActor PreviousActor { get; set; }
        public IList<Flow> AssignedFlows { get; set; }
        public IList<ForkedFlow> ForkedFlows { get; set; }
        public IDictionary<string, object> Configuration { get; set; }

        public ExecutionContext(IActor actor,ProcessDefinition processDefintion, ProcessInstance processInstance = null, Flow flow = null)
        {
            this.Actor = actor;
            this.processDefintion = processDefintion;
            this.ProcessInstance = processInstance;
            this.Flow = flow;
            this.AssignedFlows = new List<Flow>();
            this.ForkedFlows = new List<ForkedFlow>();
            this.Configuration = new Dictionary<string, object>();
        }

        public ProcessInstance StartProcess(IDictionary<string, object> attributeValues = null)
        {
            this.ProcessInstance = new ProcessInstance(this.Actor,this.processDefintion);
            this.Flow = ProcessInstance.RootFlow;
            var startState = processDefintion.Nodes.Single(s => s is StartState) as StartState;

            this.RunActionsForEvent(EventType.BEFORE_PERFORM_OF_ACTIVITY, startState.Id);

            this.storeAttributeValues(attributeValues);
            this.StoreRole(this.Actor,startState);

            this.RunActionsForEvent(EventType.PROCESS_INSTANCE_START, startState.Id);
            this.setActorAsPrevious();

            IRepository<Transition> transitions = ServiceLocator.Current.GetInstance<IRepository<Transition>>();
            var leavingTransitions = transitions.With(w => w.To).Query(new Specification<Transition>(s => s.From.Id == startState.Id));
            var transition = leavingTransitions.First();
            var ss = transition.From;
            ExecutionEngine.ProcessTransition(transition, this);

            this.RunActionsForEvent(EventType.AFTER_PERFORM_OF_ACTIVITY, startState.Id);

            return ProcessInstance;
        }

        public void PerformActivity(IDictionary<string, object> attributeValues = null,string transitionName = null)
        {
            ActivityState activityState = (ActivityState)Flow.Node;
            IRepository<Transition> transitions = ServiceLocator.Current.GetInstance<IRepository<Transition>>();
            var leavingTransitions = transitions.With(w=>w.To).Query(new Specification<Transition>(s => s.From.Id == activityState.Id));

            Transition transition = null;
            if (transitionName == null)
                transition = leavingTransitions.First();
            else
                transition = leavingTransitions.Single(q => q.Name == transitionName);

            this.storeAttributeValues(attributeValues);

            ExecutionEngine.ProcessTransition(transition, this);
        }

        private void storeAttributeValues(IDictionary<string,object> attributeValues)
        {
            if (attributeValues != null)
            {
                foreach (var attributeName in attributeValues.Keys)
                {
                    setAttribute(attributeName, attributeValues[attributeName]);
                }
            }
        }

        private void setAttribute(string attributeName, object valueObject)
        {
            AttributeInstance attributeInstance = this.Flow.AttributeInstances.SingleOrDefault(s => s.AttributeName == attributeName);
            if (attributeInstance != null)
            {
                attributeInstance.SetValue(valueObject);
            }
        }

        public void StoreRole(IActor actor ,ActivityState activityState)
        {
            string role = activityState.ActorRoleName;
            if (!string.IsNullOrEmpty(role))
            {
                setAttribute(role, actor);
            }
        }

        public void setActorAsPrevious()
        {
            this.PreviousActor = this.Actor;
            this.Actor = null;
            this.Flow.ActorId = null;
        }

        public void ForkFlow(Transition transition, IDictionary<string, object> attributeValues = null)
        {
            var processBlockRepository = ServiceLocator.Current.GetInstance<IRepository<ProcessBlock>>();

            Flow subFlow = new Flow(transition.Name, this.Flow, processBlockRepository.With(w => w.Attributes).Get(this.Node.ProcessBlockId));
            this.Flow.Children.Add(subFlow);

            this.Flow = subFlow;
            storeAttributeValues(attributeValues);
            this.Flow = this.Flow.Parent;

            this.ForkedFlows.Add(new ForkedFlow(transition, subFlow));
        }

        public IList<Flow> GetOtherActiveConcurrentFlows()
        {
            var flowRepository = ServiceLocator.Current.GetInstance<IRepository<Flow>>();

            var result =
            from c in flowRepository
            where (c.Parent != null && c.Parent.Id == this.Flow.Parent.Id) &&
                  (c.Id != this.Flow.Id) &&
                  (c.End == null)
            select c;

            return result.ToList();

        }

        public void RunActionsForEvent(EventType eventType,Guid definitionObjectId)
        {
            var actionRepository = ServiceLocator.Current.GetInstance<IRepository<ActionDef>>();
            actionRepository.With(s => s.ActionDelegation);
            var actions = actionRepository.Where(s => s.EventType == eventType && s.DefinitionObjectId == definitionObjectId);

            var delegationHelper = ServiceLocator.Current.GetInstance<IDelegationHelper>();
            foreach (var action in actions)
            {
                delegationHelper.DelegateAction(action.ActionDelegation, this);
            }
        }

        public IFlow GetFlow()
        {
            throw new NotImplementedException();
        }

        public void SetAttribute(string attributeName, object attributeValue)
        {
            throw new NotImplementedException();
        }

        public IProcessDefinition GetProcessDefinition()
        {
            throw new NotImplementedException();
        }

        public IProcessInstance GetProcessInstance()
        {
            return this.ProcessInstance;
        }

        public IActor GetPreviousActor()
        {
            return this.PreviousActor;
        }

        public INode GetNode()
        {
            return Node;
        }

        public object GetAttribute(string name)
        {
            AttributeInstance attributeInstance = FindAttributeInstanceInScope(name);
            if (attributeInstance != null)
            {
                return attributeInstance.GetValue();
            }
            return null;
        }

        private AttributeInstance FindAttributeInstanceInScope(string attributeName)
        {
            AttributeInstance attributeInstance = null;
            Flow scope = this.Flow;
            var attributeRepository = ServiceLocator.Current.GetInstance<IRepository<AttributeInstance>>();

            while (attributeInstance == null)
            {
                var query =
                    from a in attributeRepository
                    where a.Scope.Id == scope.Id && a.AttributeName == attributeName
                    select a;

                attributeInstance = query.SingleOrDefault();
                if (attributeInstance == null)
                {
                    if (!scope.IsRootFlow())
                    {
                        scope = scope.Parent;
                    }
                    else
                    {
                        //throw new RuntimeException( "couldn't find attribute-instance '" + attributeName + "' in scope of flow '" + this.flow + "'" ); 
                        // log a warning message (indicate that attribute supplied is not defined in attribute-instance in db)
                        Log.Warning("couldn't find attribute-instance '" + attributeName + "' in scope of flow '" + this.Flow + "'");
                        break;
                    }
                }
              
            }
            return attributeInstance;
        }

        public void ForkFlow(string transitionName)
        {
            throw new ArgumentOutOfRangeException();
        }

        public void ForkFlow(string transitionName, IDictionary<string, object> attributeValues)
        {
            throw new ArgumentOutOfRangeException();
        }

        public void ForkFlow(object name)
        {
            throw new ArgumentOutOfRangeException();
        }

        IList<IFlow> IJoinContext.GetOtherActiveConcurrentFlows()
        {
            List<IFlow> flows = new List<IFlow>();
            foreach (var flow in this.GetOtherActiveConcurrentFlows())
            {
                flows.Add(flow);
            }

            return flows;
        }
    }
}
