using System;
using System.Xml;

namespace SugiBpm.Definition.Domain
{
    public class Transition : DefinitionObject
    {
        public Node From { get; set; }
        public Node To { get; set; }

        public Transition() : base()
        {
        }

        public Transition(ProcessDefinitionCreationContext creationContext) : base(creationContext)
        {
        }

        public override void ReadProcessData(XmlElement xmlElement)
        {
            DefinitionObject parent = creationContext.DefinitionObject;
            creationContext.DefinitionObject = this;
            base.ReadProcessData(xmlElement);
            creationContext.DefinitionObject = parent;
            From = creationContext.Node;

            creationContext.AddUnresolvedReference(this, xmlElement.GetProperty("to"), creationContext.TransitionDestinationScope, "to", typeof(Node));
        }
    }
}
