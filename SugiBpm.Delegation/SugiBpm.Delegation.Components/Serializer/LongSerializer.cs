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
    [ExportMetadata("ClassName", "LongSerializer")]
    public class LongSerializer : ISerializer
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(LongSerializer));
        private IDictionary<string, object> configuration = null;
        public string Serialize(object object_Renamed)
        {
            string serialized = null;

            if (!(object_Renamed is long))
            {
                throw new ArgumentException("LongSerializer can't serialize " + object_Renamed);
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
            return long.Parse(text);
        }

        public void SetConfiguration(IDictionary<string, object> configuration)
        {
            this.configuration = configuration;
        }
    }
}
