using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public interface IForkContext : IHandlerContext
    {
        void ForkFlow(string transitionName);
        void ForkFlow(string transitionName, IDictionary<string,object> attributeValues);
        void ForkFlow(object name);
    }
}
