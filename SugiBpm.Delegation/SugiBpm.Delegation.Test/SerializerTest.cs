using SugiBpm.Delegation.Domain;
using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Test
{
    [Export(typeof(ISerializer))]
    [ExportMetadata("ClassName", "SerializerTest")]
    public class SerializerTest : ISerializer
    {
        public object Deserialize(string text)
        {
            if (text.ToUpper() == "APPROVE")
                return Evaluation.APPROVE;
            else if (text.ToUpper() == "DISAPPROVE")
                return Evaluation.DISAPPROVE;
            else
                return null;
        }

        public string Serialize(object valueObject)
        {
            throw new NotImplementedException();
        }

        public void SetConfiguration(IDictionary<string, object> configuration)
        {
           
        }
    }
}
