using SugiBpm.Delegation.Interface.Definition;
using System;
using System.Xml;

namespace SugiBpm.Definition.Domain
{
    public class AttributeDef : DefinitionObject,IAttribute
    {
        public ProcessBlock Scope { get; set; }
        public string InitialValue { get; set; }
        public DelegationDef SerializerDelegation { get; set; }

        public AttributeDef() : base()
        {
        }

        public AttributeDef(ProcessDefinitionCreationContext creationContext) : base(creationContext)
        {
        }

        public override void ReadProcessData(XmlElement xmlElement)
        {
            base.ReadProcessData(xmlElement);
            this.Scope = creationContext.ProcessBlock;
            this.InitialValue = xmlElement.GetProperty("initial-value");

            creationContext.DelegatingObject = this;
            this.SerializerDelegation = new DelegationDef(creationContext);
            this.SerializerDelegation.ReadProcessData(xmlElement);
            creationContext.DelegatingObject = null;
            creationContext.AddReferencableObject(this.Name, this.Scope, typeof(AttributeDef), this);
        }
    }
}
