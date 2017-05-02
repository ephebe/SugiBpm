using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SugiBpm.Delegation.Interface.Definition;
using SugiBpm.Delegation.Interface.Execution;
using SugiBpm.Definition.Domain;

namespace SugiBpm.Execution.Domain
{
    //public class ActivityForm : IActivityForm
    //{
    //    public ActivityState Activity { get; set; }
    //    public IDictionary<string, object> AttributeValues { get; set; }
    //    public IEnumerable<Field> Fields { get; set; }
    //    public Flow Flow { get; set; }
    //    public ProcessDefinition ProcessDefinition { get; set; }
    //    public List<string> TransitionNames { get; set; }

    //    IFlow IActivityForm.Flow
    //    {
    //        get { return Flow;}
    //    }

    //    IProcessDefinition IActivityForm.ProcessDefinition
    //    {
    //        get { return ProcessDefinition;}
    //    }

    //    IActivityState IActivityForm.Activity
    //    {
    //        get { return Activity; }
    //    }

    //    IEnumerable<Field> IActivityForm.Fields
    //    {
    //        get { return Fields;}
    //    }

    //    IDictionary<string, object> IActivityForm.AttributeValues
    //    {
    //        get { return AttributeValues;}
    //    }

    //    IList<string> IActivityForm.TransitionNames
    //    {
    //        get { return TransitionNames; }
    //    }

    //    IEnumerable<IField> IActivityForm.Fields
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    public ActivityForm(ProcessDefinition processDefinition, IEnumerable<Field> fields, Dictionary<string, object> attributeValues)
    //    {
    //        this.ProcessDefinition = processDefinition;
    //        this.Fields = fields;
    //        AttributeValues = attributeValues;
    //    }

       
    //}
}
