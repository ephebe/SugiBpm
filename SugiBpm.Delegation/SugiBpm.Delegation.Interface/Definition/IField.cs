using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface.Definition
{
    public interface IField
    {
        string Name { get; }
        string Description { get; }
        IAttribute Attribute { get; }
        IState State { get; }
        FieldAccess Access { get; }

        IHtmlFormatter GetHtmlFormatter();
    }
}
