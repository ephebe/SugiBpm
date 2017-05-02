using System;
using System.Xml;

namespace SugiBpm.Definition.Domain
{
    public class Join : Node
    {
        public Guid? JoinDelegationId { get; set; }
        public DelegationDef JoinDelegation { get; set; }
        public Join() : base()
        {
        }
        public Join(ProcessDefinitionCreationContext creationContext) : base(creationContext)
        {
        }

        public override void ReadProcessData(XmlElement xmlElement)
        {
            base.ReadProcessData(xmlElement);

            if (!string.IsNullOrEmpty(xmlElement.GetAttribute("handler")))
            {
                creationContext.DelegatingObject = this;
                this.JoinDelegation = new DelegationDef(creationContext);
                this.JoinDelegation.ReadProcessData(xmlElement);
                creationContext.DelegatingObject = null;
            }
        }
    }
}
