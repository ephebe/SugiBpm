using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public interface IActionHandler
    {
        /// <summary> implements the process-initiated action.</summary>
        /// <param name="actionContext">is the object that allows the Interaction-implementator to communicate with the NetBpm process engine.
        /// </param>
        void Run(IActionContext actionContext);
    }
}
