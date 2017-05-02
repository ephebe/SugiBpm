using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public interface IJoinHandler
    {
        bool Join(IJoinContext joinContext);
    }
}
