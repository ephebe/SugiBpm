using SugiBpm.Definition.Domain;
using SugiBpm.Delegation.Interface.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delagation.Test
{
    public class FakeNode : Node
    {
        public FakeNode() : base()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
