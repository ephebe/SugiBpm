using System;
using System.Xml;

namespace SugiBpm.Definition.Domain
{
    public class Fork : Node
    {
        public Guid? ForkDelegationId { get; set; }
        public DelegationDef ForkDelegation { get; set; }
        public Fork() : base()
        {
        }

        public Fork(ProcessDefinitionCreationContext creationContext) : base(creationContext)
        {
        }

        public override void ReadProcessData(XmlElement xmlElement)
        {
            base.ReadProcessData(xmlElement);

            if (!string.IsNullOrEmpty(xmlElement.GetAttribute("handler")))
            {
                creationContext.DelegatingObject = this;
                this.ForkDelegation = new DelegationDef(creationContext);
                this.ForkDelegation.ReadProcessData(xmlElement);
                creationContext.DelegatingObject = null;
            }
        }

        public override ProcessBlock RegisteredScope()
        {
            return creationContext.ProcessBlock.ParentBlock;
        }
    }
}
