using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public enum ExceptionHandlingType
    {
        ROLLBACK = 1,
        LOG = 2,
        IGNORE = 3,
    }
}
