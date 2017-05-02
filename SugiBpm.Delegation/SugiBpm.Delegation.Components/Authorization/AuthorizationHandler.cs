using Microsoft.Practices.ServiceLocation;
using SugiBpm.Delegation.Interface;
using SugiBpm.Delegation.Interface.Execution;
using SugiBpm.Delegation.Interface.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components.Authorization
{
    //public class AuthorizationHandler : IAuthorizationHandler
    //{
    //    public void CheckCancelFlow(Guid authenticatedActorId, Guid flowId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void CheckCancelProcessInstance(Guid authenticatedActorId, Guid processInstanceId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void CheckDelegateActivity(Guid authenticatedActorId, Guid flowId, string delegateActorId)
    //    {
    //        IExecutionApplication executionApplication = ServiceLocator.Current.GetInstance<IExecutionApplication>();
    //        try
    //        {
    //            IFlow flow = executionApplication.GetFlow(flowId);
    //            IActor director = null;

    //            ICollection<IAttributeInstance> attributeInstances = flow.AttributeInstances;
    //            foreach (var attribute in attributeInstances)
    //            {
    //                if ("director".Equals(attribute.Attribute.Name))
    //                {
    //                    director = (IActor)attributeInstance.GetValue();
    //                }
    //            }

    //            if ("irector".Id.Equals(authenticatedActorId) == false)
    //            {
    //                throw new Exception("Only director is allowed to delegate activity");
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            throw new System.SystemException("failed doing authorization : " + e.Message);
    //        }
    //    }

    //    public void CheckGetActivityForm(Guid authenticatedActorId, Guid flowId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void CheckGetFlow(Guid authenticatedActorId, Guid flowId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void CheckGetStartForm(Guid authenticatedActorId, Guid processDefinitionId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void CheckPerformActivity(Guid authenticatedActorId, Guid flowId, IDictionary<string, object> attributeValues, string transitionName)
    //    {
    //        //only actor assigned for that activity can perform an activity
    //        IExecutionApplication executionApplication = ServiceLocator.Current.GetInstance<IExecutionApplication>();
    //        try
    //        {
    //            IFlow flow = executionApplication.GetFlow(flowId);
    //            if (flow.GetActor().Id.Equals(authenticatedActorId) == false)
    //            {
    //                throw new AuthorizationException("only actor assigned for that activity can perform an activity");
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            throw new System.SystemException("failed doing authorization : " + e.Message);
    //        }
    //    }

    //    public void CheckRemoveProcessDefinition(Guid authenticatedActorId, Guid processDefinitionId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void CheckRemoveProcessInstance(Guid authenticatedActorId, Guid processInstanceId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void CheckStartProcessInstance(Guid authenticatedActorId, Guid processDefinitionId, IDictionary<string, object> attributeValues, string transitionName)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
