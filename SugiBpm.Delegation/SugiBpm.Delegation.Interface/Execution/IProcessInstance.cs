using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface.Execution
{
    public interface IProcessInstance
    {
        Guid InitiatorActorId { get; }
    }
}
