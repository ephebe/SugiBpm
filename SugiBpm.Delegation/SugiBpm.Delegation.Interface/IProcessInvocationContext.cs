using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public interface IProcessInvocationContext : IHandlerContext
    {
        /// <summary> gets the ActionContext for the invoked process instance;</summary>
        IActionContext GetInvokedProcessContext();
    }
}
