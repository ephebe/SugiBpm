using Microsoft.Practices.ServiceLocation;
using SugiBpm.Definition.Domain;
using SugiBpm.Delegation.Interface;
using SugiBpm.Execution.Domain;
using SunStone.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Domain
{
    public class AuthorizationHelper
    {
        [ImportMany]
        public IEnumerable<System.Lazy<IAuthorizationHandler, IClassNameMetadata>> AuthorizationHandlers { get; set; }

        public void CheckRemoveProcessInstance(Guid authenticatedActorId, Guid processInstanceId)
        {
            IAuthorizationHandler authorizationHandler = GetHandlerFromProcessInstanceId(processInstanceId);
            if (authorizationHandler != null)
            {
                authorizationHandler.CheckRemoveProcessInstance(authenticatedActorId, processInstanceId);
            }
        }

        public void CheckRemoveProcessDefinition(Guid authenticatedActorId, Guid processDefinitionId)
        {
            IAuthorizationHandler authorizationHandler = GetHandlerFromProcessDefinitionId(processDefinitionId);
            if (authorizationHandler != null)
            {
                authorizationHandler.CheckRemoveProcessDefinition(authenticatedActorId, processDefinitionId);
            }
        }

        public void CheckStartProcessInstance(Guid authenticatedActorId, Guid processDefinitionId, IDictionary<string,object> attributeValues, string transitionName)
        {
            IAuthorizationHandler authorizationHandler = GetHandlerFromProcessDefinitionId(processDefinitionId);
            if (authorizationHandler != null)
            {
                authorizationHandler.CheckStartProcessInstance(authenticatedActorId, processDefinitionId, attributeValues, transitionName);
            }
        }

        public void CheckGetStartForm(Guid authenticatedActorId, Guid processDefinitionId)
        {
            IAuthorizationHandler authorizationHandler = GetHandlerFromProcessDefinitionId(processDefinitionId);
            if (authorizationHandler != null)
            {
                authorizationHandler.CheckGetStartForm(authenticatedActorId, processDefinitionId);
            }
        }

        public void CheckGetActivityForm(Guid authenticatedActorId, Guid flowId)
        {
            IAuthorizationHandler authorizationHandler = GetHandlerFromFlowId(flowId);
            if (authorizationHandler != null)
            {
                authorizationHandler.CheckGetActivityForm(authenticatedActorId, flowId);
            }
        }

        public void CheckPerformActivity(Guid authenticatedActorId, Guid flowId, IDictionary<string,object> attributeValues, string transitionName)
        {
            IAuthorizationHandler authorizationHandler = GetHandlerFromFlowId(flowId);
            if (authorizationHandler != null)
            {
                authorizationHandler.CheckPerformActivity(authenticatedActorId, flowId, attributeValues, transitionName);
            }
        }

        public void CheckDelegateActivity(Guid authenticatedActorId, Guid flowId, string delegateActorId)
        {
            IAuthorizationHandler authorizationHandler = GetHandlerFromFlowId(flowId);
            if (authorizationHandler != null)
            {
                authorizationHandler.CheckDelegateActivity(authenticatedActorId, flowId, delegateActorId);
            }
        }

        public void CheckCancelProcessInstance(Guid authenticatedActorId, Guid processInstanceId)
        {
            IAuthorizationHandler authorizationHandler = GetHandlerFromProcessInstanceId(processInstanceId);
            if (authorizationHandler != null)
            {
                authorizationHandler.CheckCancelProcessInstance(authenticatedActorId, processInstanceId);
            }
        }

        public void CheckCancelFlow(Guid authenticatedActorId, Guid flowId)
        {
            IAuthorizationHandler authorizationHandler = GetHandlerFromFlowId(flowId);
            if (authorizationHandler != null)
            {
                authorizationHandler.CheckCancelFlow(authenticatedActorId, flowId);
            }
        }

        public void CheckGetFlow(Guid authenticatedActorId, Guid flowId)
        {
            IAuthorizationHandler authorizationHandler = GetHandlerFromFlowId(flowId);
            if (authorizationHandler != null)
            {
                authorizationHandler.CheckGetFlow(authenticatedActorId, flowId);
            }
        }

        private IAuthorizationHandler GetHandlerFromProcessDefinitionId(Guid processDefinitionId)
        {
            ProcessDefinition processDefinition = null;
            
            try
            {
                var processDefinitionRepository = ServiceLocator.Current.GetInstance<IRepository<ProcessDefinition>>();
                processDefinition = processDefinitionRepository.Get(processDefinitionId);
            }
            catch (Exception e)
            {
                throw new ArgumentException("couldn't check authorization : process definition with id '" + processDefinitionId + "' does not exist : " + e.Message);
            }
            return GetAuthorizationHandler(processDefinition);
        }

        private IAuthorizationHandler GetHandlerFromProcessInstanceId(Guid processInstanceId)
        {
            ProcessDefinition processDefinition = null;
            try
            {
                var processInstanceRepository = ServiceLocator.Current.GetInstance<IRepository<ProcessInstance>>();
                var query =
                    from m in processInstanceRepository
                    where m.Id == processInstanceId
                    select m.ProcessDefinition;
                processDefinition = query.SingleOrDefault();
            }
            catch (Exception e)
            {
                throw new ArgumentException("couldn't check authorization : process instance with id '" + processInstanceId + "' does not exist : " + e.Message);
            }
            return GetAuthorizationHandler(processDefinition);
        }

        private IAuthorizationHandler GetHandlerFromFlowId(Guid flowId)
        {
            ProcessDefinition processDefinition = null;
            try
            {
                var flowRepository = ServiceLocator.Current.GetInstance<IRepository<Flow>>();
                var processInstanceRepository = ServiceLocator.Current.GetInstance<IRepository<ProcessInstance>>();
                var query =
                    from n in flowRepository
                    join m in processInstanceRepository on n.ProcessInstanceId equals m.Id
                    where n.Id == flowId
                    select m.ProcessDefinition;
                processDefinition = query.SingleOrDefault();
            }
            catch (Exception e)
            {
                throw new ArgumentException("couldn't check authorization : flow with id '" + flowId + "' does not exist : " + e.Message);
            }
            return GetAuthorizationHandler(processDefinition);
        }

        private IAuthorizationHandler GetAuthorizationHandler(ProcessDefinition processDefinition)
        {
            IAuthorizationHandler authorizationHandler = null;
            var delegationRepository = ServiceLocator.Current.GetInstance<IRepository<DelegationDef>>();
            DelegationDef delegation = delegationRepository.Get(processDefinition.AuthorizationDelegationId);
            if (delegation != null)
            {
                foreach (var handler in AuthorizationHandlers)
                {
                    if (handler.Metadata.ClassName == delegation.ClassName)
                        authorizationHandler = handler.Value;
                }
            }
            return authorizationHandler;
        }
    }
}
