using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public interface IAuthorizationHandler
    {
        void CheckRemoveProcessInstance(Guid authenticatedActorId, Guid processInstanceId);
        void CheckRemoveProcessDefinition(Guid authenticatedActorId, Guid processDefinitionId);
        void CheckStartProcessInstance(Guid authenticatedActorId, Guid processDefinitionId, IDictionary<string,object> attributeValues, string transitionName);
        void CheckGetStartForm(Guid authenticatedActorId, Guid processDefinitionId);
        void CheckGetActivityForm(Guid authenticatedActorId, Guid flowId);
        void CheckPerformActivity(Guid authenticatedActorId, Guid flowId, IDictionary<string, object> attributeValues, string transitionName);
        void CheckDelegateActivity(Guid authenticatedActorId, Guid flowId, string delegateActorId);
        void CheckCancelProcessInstance(Guid authenticatedActorId, Guid processInstanceId);
        void CheckCancelFlow(Guid authenticatedActorId, Guid flowId);
        void CheckGetFlow(Guid authenticatedActorId, Guid flowId);
    }
}
