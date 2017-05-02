using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Application.ViewModel
{
    public class FlowView
    {
        public Guid Id { get; set; }
        public string ProcessDefinitionName { get; set; }
        public string NodeName { get; set; }
    }
}
