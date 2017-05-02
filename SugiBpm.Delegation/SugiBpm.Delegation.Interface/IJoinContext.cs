using SugiBpm.Delegation.Interface.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public interface IJoinContext : IHandlerContext
    {
        /// <summary> gets all active concurrent flows other then the one 
        /// that is actually arriving in the join. 
        /// </summary>
        IList<IFlow> GetOtherActiveConcurrentFlows();
    }
}
