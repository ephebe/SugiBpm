using SugiBpm.Delegation.Interface.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public interface IAssignmentContext : IHandlerContext
    {
        /// <summary> since Assigner-implementations tend to use the organisation-component 
        /// this is a convenient way to get it.
        /// </summary>
        //IOrganisationSessionLocal GetOrganisationComponent();

        /// <summary> gets the {@link ActivityState} for which an {@link Actor} has to be selected. </summary>
        new INode GetNode();

        /// <summary> gets the {@link Actor} that performed the previous activity-state.</summary>
        //IActor GetPreviousActor();
    }
}
