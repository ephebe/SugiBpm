using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface.Organization
{
    public interface IMembership
    {
        IGroup Group { get; set; }
        IUser User { get; set; }
    }
}
