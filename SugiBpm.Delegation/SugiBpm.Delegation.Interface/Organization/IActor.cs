using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface.Organization
{
    public interface IActor
    {
        Guid Id { get; }
        string UniqueName { get; set; }
    }
}
