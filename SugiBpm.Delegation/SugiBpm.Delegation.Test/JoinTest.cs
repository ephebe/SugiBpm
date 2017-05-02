using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Test
{
    [Export(typeof(IJoinHandler))]
    [ExportMetadata("ClassName", "JoinTest")]
    public class JoinTest : IJoinHandler
    {
        public bool Join(IJoinContext joinContext)
        {
            return false;
        }
    }
}
