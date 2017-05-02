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
    [ExportMetadata("ClassName", "TextSerializer")]
    public class TextSerializer : ISerializer
    {
        private IDictionary<string, object> configuration = null;
        public string Serialize(object object_Renamed)
        {
            string serialized = null;

            if (object_Renamed != null)
            {
                serialized = object_Renamed.ToString();
            }

            return serialized;
        }

        public object Deserialize(string text)
        {
            return text;
        }

        public void SetConfiguration(IDictionary<string, object> configuration)
        {
            this.configuration = configuration;
        }
    }
}
