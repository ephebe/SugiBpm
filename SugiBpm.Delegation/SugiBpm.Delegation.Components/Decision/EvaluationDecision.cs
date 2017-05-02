using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components
{
    [Export(typeof(IDecisionHandler))]
    [ExportMetadata("ClassName", "EvaluationDecision")]
    public class EvaluationDecision : IDecisionHandler
    {
        public string Decide(IDecisionContext decisionContext)
        {
            string transitionName = "";

            string attributeName = (string)decisionContext.Configuration["attribute"];
            object attributeValue = decisionContext.GetAttribute(attributeName);

            if (attributeValue == Evaluation.APPROVE)
            {
                transitionName = "approve";
            }
            else if (attributeValue == Evaluation.DISAPPROVE)
            {
                transitionName = "disapprove";
            }

            return transitionName;
        }
    }
}
