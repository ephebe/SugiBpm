using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public sealed class Evaluation
    {
        public static readonly Evaluation APPROVE = new Evaluation("approve");
        public static readonly Evaluation DISAPPROVE = new Evaluation("disapprove");
        private string _name = null;

        public static bool TryParseEvaluation(string text, out Evaluation evaluation)
        {
            try
            {
                evaluation = Evaluation.ParseEvaluation(text);
                return true;
            }
            catch (FormatException)
            {
                evaluation = null;
                return false;
            }
        }

        public static Evaluation ParseEvaluation(string text)
        {
            if (text == null)
                return null;

            if (text.ToUpper().Equals(APPROVE.ToString().ToUpper()))
            {
                return APPROVE;
            }
            else
            {
                if (text.ToUpper().Equals(DISAPPROVE.ToString().ToUpper()))
                {
                    return DISAPPROVE;
                }
                else
                {
                    throw new FormatException("Couldn't parse " + text + " to a valid EvaluationResult");
                }
            }
        }

        private Evaluation(string name)
        {
            this._name = name;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
