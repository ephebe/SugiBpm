using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SugiBpm.Delegation.Interface.Definition;

namespace SugiBpm.Delegation.Interface.Execution
{
    public class IFork
    {
        public IEnumerable<ITransition> LeavingTransitions { get; set; }
    }
}
