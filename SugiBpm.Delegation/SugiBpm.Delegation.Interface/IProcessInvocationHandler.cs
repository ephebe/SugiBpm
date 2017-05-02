using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public interface IProcessInvocationHandler
    {
        string GetStartTransitionName(IProcessInvocationContext processInvokerContext);

        IDictionary<string,object> GetStartAttributeValues(IProcessInvocationContext processInvokerContext);
        IDictionary<string, object> CollectResults(IProcessInvocationContext processInvocationContext);

        string GetCompletionTransitionName(IProcessInvocationContext processInvokerContext);
    }
}
