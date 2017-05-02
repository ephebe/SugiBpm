using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Test
{
    [Export(typeof(IForkHandler))]
    [ExportMetadata("ClassName", "ForkTest")]
    public class ForkTest : IForkHandler
    {
        public void Fork(IForkContext forkContext)
        {
            forkContext.ForkFlow("");
        }
    }
}
