using SugiBpm.Delegation.Interface.Definition;
using SugiBpm.Delegation.Interface.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SugiBpm.Delegation.Interface.Organization;

namespace SugiBpm.Delegation.Interface
{
    public interface IHandlerContext
    {
        /// <summary> gets the configuration, specified with the 'parameter'-tags in the process archive.</summary>
		IDictionary<string, object> Configuration { get; set; }

        /// <summary> gets the {@link ProcessDefinition}.</summary>
        IProcessDefinition GetProcessDefinition();

        /// <summary> gets the {@link ProcessInstance}.</summary>
        IProcessInstance GetProcessInstance();

        /// <summary> gets the {@link Flow} in which this delegation is executed. </summary>
        IFlow GetFlow();

        /// <summary> gets the {@link Node} in which this delegation is executed.</summary>
        INode GetNode();

        /// <summary> gets the value of an {@link AttributeInstance} associated with this {@link ProcessInstance}.</summary>
        object GetAttribute(string name);
        IActor GetPreviousActor();

        /// <summary> allows the Delegate-implementations to log events to the database.</summary>
        //void AddLog(String msg);

        // <summary> convenience method for scheduling jobs</summary>
        //void Schedule(Job job);

        // <summary> convenience method for scheduling jobs</summary>
        //void Schedule(Job job, String reference);
    }
}
