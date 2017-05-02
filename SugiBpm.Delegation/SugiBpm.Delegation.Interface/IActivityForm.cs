using SugiBpm.Delegation.Interface.Definition;
using SugiBpm.Delegation.Interface.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public interface IActivityForm
    {
		IFlow Flow { get; }

        IProcessDefinition ProcessDefinition { get; }

        IActivityState Activity { get; }

        IEnumerable<IField> Fields { get; }

        IDictionary<string,object> AttributeValues { get; }

        /// <summary> gets the list of all named transitions leaving the {@link ActivityState}.</summary>
        IList<string> TransitionNames { get; }
    }
}
