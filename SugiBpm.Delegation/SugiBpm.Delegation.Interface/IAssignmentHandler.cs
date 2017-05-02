using SugiBpm.Delegation.Interface.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public interface IAssignmentHandler
    {
        IActor SelectActor(IAssignmentContext assignerContext);
    }
}
