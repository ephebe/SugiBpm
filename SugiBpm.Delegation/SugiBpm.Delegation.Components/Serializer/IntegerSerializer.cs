using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components
{
    [Export(typeof(ISerializer))]
    [ExportMetadata("ClassName", "IntegerSerializer")]
    public class IntegerSerializer : ISerializer
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(IntegerSerializer));
        private IDictionary<string, object> configuration = null;
        public string Serialize(object object_Renamed)
        {
            string serialized = null;

            if (!(object_Renamed is Int32))
            {
                throw new ArgumentException("IntegerSerializer can't serialize " + object_Renamed);
            }

            if (object_Renamed != null)
            {
                serialized = object_Renamed.ToString();
            }

            return serialized;
        }

        public object Deserialize(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            return Int32.Parse(text);
        }

        public void SetConfiguration(IDictionary<string, object> configuration)
        {
            this.configuration = configuration;
        }
    }
}
