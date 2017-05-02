using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public interface IConfigurable
    {
        void SetConfiguration(IDictionary<string,object> configuration);
    }
}
