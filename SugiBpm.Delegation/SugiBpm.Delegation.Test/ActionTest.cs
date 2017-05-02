using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Test
{
    [Export(typeof(IActionHandler))]
    [ExportMetadata("ClassName", "ActionTest")]
    public class ActionTest : IActionHandler
    {
        public void Run(IActionContext actionContext)
        {
            actionContext.Configuration.Add("test", "1234");
        }
    }
}
