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
    [ExportMetadata("ClassName", "DateSerializer")]
    public class DateSerializer : ISerializer
    {
        private IDictionary<string, object> configuration = null;
        public string Serialize(object object_Renamed)
        {
            string serailized = null;

            if (!(object_Renamed is DateTime))
            {
                throw new ArgumentException("DateSerializer can't serialize " + object_Renamed);
            }

            if (object_Renamed != null)
            {
                DateTime date = (DateTime)object_Renamed;
                serailized = Convert.ToString(date.Ticks);
            }

            return serailized;
        }

        public object Deserialize(string text)
        {
            DateTime date = DateTime.MinValue;

            if (((object)text != null) && (!"".Equals(text)))
            {
                try
                {
                    long time = Int64.Parse(text);
                    date = new DateTime(time);
                }
                catch (FormatException e)
                {
                    throw new ArgumentException("can't deserialize " + text + " to an Date.", e);
                }
            }

            return date;
        }

        public void SetConfiguration(IDictionary<string, object> configuration)
        {
            this.configuration = configuration;
        }
    }
}
