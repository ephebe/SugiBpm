using SugiBpm.Delegation.Interface.Definition;
using SugiBpm.Delegation.Interface.Execution;
using SugiBpm.Execution.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SugiBpm.Web.Models
{
    public class UserViewModel
    {
        public IList<FlowView> TaskList { get; set; }

        public IList<IProcessDefinition> ProcessDefinitions  { get; set; }

        public string Preview { get; set; }

        public string ProcessDefinitionName { get; set; }
    }
}