using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Application.ViewModel
{
    public class ActivityFormView
    {
        public Guid? ProcessDefinitionId { get; set; }
        public Guid? FlowId { get; set; }
        public Dictionary<string, object> AttributeValues { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }

}
