using Microsoft.Practices.ServiceLocation;
using SugiBpm.Definition.Domain;
using SugiBpm.Delegation.Interface.Organization;
using SunStone.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Domain
{
    public static class ExecutionEngine
    {
        public static void ProcessTransition(Transition transition, ExecutionContext executionContext)
        {
            executionContext.RunActionsForEvent(EventType.TRANSITION, transition.Id);

            Flow flow = executionContext.Flow;
            Node destination = transition.To;
            flow.Node = destination;
            executionContext.Node = destination;

            string nodeType = destination.GetType().FullName;
          
            if (nodeType.Contains("ActivityState"))
            {
                ProcessActivityState((ActivityState)destination, executionContext);
            }
            else if (nodeType.Contains("ProcessState"))
            {
                ProcessProcessState((ProcessState)destination, executionContext);
            }
            else if (nodeType.Contains("Decision"))
            {
                ProcessDecision((Decision)destination, executionContext);
            }
            else if (nodeType.Contains("Fork"))
            {
                ProcessFork((Fork)destination, executionContext);
            }
            else if (nodeType.Contains("Join"))
            {
                ProcessJoin((Join)destination, executionContext);
            }
            else if (nodeType.Contains("EndState"))
            {
                ProcessEndState((EndState)destination, executionContext);
            }
            else
            {
                throw new SystemException("");
            }
        }

        private static void ProcessActivityState(ActivityState destination, ExecutionContext executionContext)
        {
            Flow flow = executionContext.Flow;

            executionContext.RunActionsForEvent(EventType.BEFORE_ACTIVITYSTATE_ASSIGNMENT, destination.Id);

            IActor actor = null;
            string role = destination.ActorRoleName;
            var delegationRepository = ServiceLocator.Current.GetInstance<IRepository<DelegationDef>>();
            DelegationDef assignmentDelegation = delegationRepository.Get(destination.AssignmentDelegationId);
            var delegationHelper = ServiceLocator.Current.GetInstance<IDelegationHelper>();

            if (assignmentDelegation != null)
            {
                // delegate the assignment of the activity-state
                actor = delegationHelper.DelegateAssignment(assignmentDelegation, executionContext);
                if (actor == null)
                {
                    throw new SystemException("invalid process definition : assigner of activity-state '" + destination.Name + "' returned null instead of a valid actorId");
                }
                //log.Debug("setting actor of flow " + flow + " to " + actorId);
            }
            else
            {
                // get the assigned actor from the specified attribute instance
                if (!string.IsNullOrEmpty(role))
                {
                    actor = (IActor)executionContext.GetAttribute(role);
                    if (actor == null)
                    {
                        throw new SystemException("invalid process definition : activity-state must be assigned to role '" + role + "' but that attribute instance is null");
                    }
                }
                else
                {
                    throw new SystemException("invalid process definition : activity-state '" + destination.Name + "' does not have an assigner or a role");
                }
            }

            flow.ActorId = actor.Id;

            if ((actor != null) && (assignmentDelegation != null))
            {
                executionContext.StoreRole(actor, destination);
            }

            executionContext.AssignedFlows.Add(flow);

            executionContext.RunActionsForEvent(EventType.AFTER_ACTIVITYSTATE_ASSIGNMENT, destination.Id);
        }

        private static void ProcessProcessState(ProcessState destination, ExecutionContext executionContext)
        {
            throw new NotImplementedException();
        }

        private static void ProcessDecision(Decision decision, ExecutionContext executionContext)
        {
            executionContext.RunActionsForEvent(EventType.BEFORE_DECISION, decision.Id);

            var delegationRepository = ServiceLocator.Current.GetInstance<IRepository<DelegationDef>>();
            var decisionDelegation = delegationRepository.Get(decision.DecisionDelegationId);
            IDelegationHelper delegationHelper = ServiceLocator.Current.GetInstance<IDelegationHelper>();
            Transition selectedTransition = 
                delegationHelper.DelegateDecision(decisionDelegation, executionContext);

            ProcessTransition(selectedTransition, executionContext);
        }

        private static void ProcessFork(Fork fork, ExecutionContext executionContext)
        {
            Flow flow = executionContext.Flow;
            var delegationRepository = ServiceLocator.Current.GetInstance<IRepository<DelegationDef>>();
            DelegationDef delegation = delegationRepository.Get(fork.ForkDelegationId);
            if (delegation != null)
            {
                var delegationHelper = ServiceLocator.Current.GetInstance<IDelegationHelper>();
                delegationHelper.DelegateFork(delegation, executionContext);
            }
            else
            {
                var transitionRepository = ServiceLocator.Current.GetInstance<IRepository<Transition>>();
                var leavingTransitions = transitionRepository.With(w=>w.To).Where(q=> q.From.Id == fork.Id);

                foreach (var transition in leavingTransitions)
                {
                    executionContext.ForkFlow(transition);
                }
            }

            Flow parentFlow = executionContext.Flow;

            executionContext.Flow = parentFlow;
            IList<ForkedFlow> forkedFlows = executionContext.ForkedFlows;
          
            foreach (var forkedFlow in forkedFlows)
            {
                executionContext.RunActionsForEvent(EventType.FORK, fork.Id);

                executionContext.Flow = forkedFlow.Flow;
                ProcessTransition(forkedFlow.Transition, executionContext);
            }
        }

        private static void ProcessJoin(Join join, ExecutionContext executionContext)
        {
            Flow joiningFlow = executionContext.Flow;
            joiningFlow.End = DateTime.Now;
            joiningFlow.ActorId = null;
            joiningFlow.Node = join; 

            if (joiningFlow.ParentReactivation)
            {   
                bool parentReactivation = false;
                IList<Flow> concurrentFlows = executionContext.GetOtherActiveConcurrentFlows();
                if (concurrentFlows.Count == 0)
                {
                    parentReactivation = true;
                }
                else
                {
                    var delegationRepository = ServiceLocator.Current.GetInstance<IRepository<DelegationDef>>();
                    DelegationDef joinDelegation = delegationRepository.Get(join.JoinDelegationId);
                    if (joinDelegation != null)
                    {
                        var delegationHelper = ServiceLocator.Current.GetInstance<IDelegationHelper>();
                        parentReactivation = delegationHelper.DelegateJoin(joinDelegation, executionContext);
                    }
                }

                if (parentReactivation)
                {
                    foreach (var concurrentFlow in concurrentFlows)
                    {
                        concurrentFlow.ParentReactivation = false;
                    }

                    Flow parentFlow = joiningFlow.Parent;
                    executionContext.Flow = parentFlow;

                    var transitionRepository = ServiceLocator.Current.GetInstance<IRepository<Transition>>();
                    var leavingTransition = transitionRepository.With(w => w.To).First(f => f.From.Id == join.Id);
                    ProcessTransition(leavingTransition, executionContext);
                }
            }
        }

        private static void ProcessEndState(EndState endState, ExecutionContext executionContext)
        {
            executionContext.RunActionsForEvent(EventType.PROCESS_INSTANCE_END, endState.ProcessDefinition.Id);

            Flow rootFlow = executionContext.Flow;
            rootFlow.ActorId = null;
            rootFlow.End = DateTime.Now;
            rootFlow.Node = endState; 
        }
    }
}
