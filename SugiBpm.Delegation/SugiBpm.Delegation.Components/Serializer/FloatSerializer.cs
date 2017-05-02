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
    [ExportMetadata("ClassName", "FloatSerializer")]
    public class FloatSerializer : ISerializer
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(FloatSerializer));
        private IDictionary<string, object> configuration = null;
        public string Serialize(object object_Renamed)
        {
            string serialized = null;

            if (!(object_Renamed is float))
            {
                throw new ArgumentException("FloatSerializer can't serialize " + object_Renamed);
            }

            if (object_Renamed != null)
            {
                serialized = object_Renamed.ToString();
            }

            return serialized;
        }

        public object Deserialize(string text)
        {
            if ((object)text == null)
                return null;
            return float.Parse(text);
        }

        public void SetConfiguration(IDictionary<string, object> configuration)
        {
            this.configuration = configuration;
        }
    }
}
