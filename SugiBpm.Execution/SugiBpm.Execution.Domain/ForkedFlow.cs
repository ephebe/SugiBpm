using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SugiBpm.Definition.Domain;

namespace SugiBpm.Execution.Domain
{
    public class ForkedFlow
    {
        public Flow Flow { get; set; }
        public Transition Transition { get; set; }

        public ForkedFlow(Transition transition, Flow subFlow)
        {
            this.Transition = transition;
            this.Flow = subFlow;
        }
    }
}
