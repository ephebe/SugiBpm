using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Test
{
    [Export(typeof(IDecisionHandler))]
    [ExportMetadata("ClassName", "DecisionTest")]
    public class DecisionTest : IDecisionHandler
    {
        public string Decide(IDecisionContext decisionContext)
        {
            return "pass";
        }
    }
}
