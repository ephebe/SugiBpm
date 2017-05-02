using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public interface ISerializer : IConfigurable
    {
        string Serialize(object valueObject);

        object Deserialize(string text);
    }
}
