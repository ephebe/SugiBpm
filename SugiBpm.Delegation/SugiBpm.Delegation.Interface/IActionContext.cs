using SugiBpm.Delegation.Interface.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public interface IActionContext : IHandlerContext
    {
        /// <summary> gets the {@link Flow} in which the {@link Action} is executed.</summary>
        new IFlow GetFlow();

        /// <summary> sets the {@link AttributeInstance} for the attribute with name attributeName to the
        /// value attributeValue.   
        /// </summary>
        void SetAttribute(string attributeName, object attributeValue);
    }
}
