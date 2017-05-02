using SugiBpm.Delegation.Interface;
using SugiBpm.Delegation.Interface.Execution;
using SugiBpm.Execution.Application.ViewModel;
using SugiBpm.Execution.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Application
{
    public interface IExecutionApplication
    {
        ProcessInstanceView StartProcessInstance(Guid actorId, Guid processDefinitionId, IDictionary<string, object> attributeValues = null, string transitionName = null);

        void PerformActivity(Guid actorId, Guid flowId, IDictionary<string,object> attributeValues = null, string transitionName = null);

        IList<FlowView> GetTaskList(Guid actorId);

        ActivityFormView GetStartForm(Guid processDefinitionId);

        ActivityFormView GetActivityForm(Guid flowId);

        FlowView GetFlow(Guid flowId);
    }
}
