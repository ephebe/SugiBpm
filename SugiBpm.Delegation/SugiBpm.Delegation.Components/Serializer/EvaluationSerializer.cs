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
    [ExportMetadata("ClassName", "EvaluationSerializer")]
    public class EvaluationSerializer : ISerializer
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(EvaluationSerializer));
        private IDictionary<string, object> configuration = null;
        public string Serialize(object object_Renamed)
        {
            string serialized = null;

            if (!(object_Renamed is Evaluation))
            {
                throw new ArgumentException("EvaluationSerializer can't serialize " + object_Renamed);
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

            Evaluation evaluation = null;
          
            try
            {
                evaluation = Evaluation.ParseEvaluation(text);
            }
            catch (FormatException e)
            {
                throw new ArgumentException("can't deserialize " + text + " to an Evaluation. " + e.Message);
            }
            return evaluation;
        }

        public void SetConfiguration(IDictionary<string, object> configuration)
        {
            this.configuration = configuration;
        }
    }
}
