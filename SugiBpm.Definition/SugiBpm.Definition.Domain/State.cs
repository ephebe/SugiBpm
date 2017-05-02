using Microsoft.Practices.ServiceLocation;
using SugiBpm.Delegation.Interface.Definition;
using SunStone.Data;
using System.Collections.Generic;
using System.Xml;

namespace SugiBpm.Definition.Domain 
{
    public class State : Node,IState
    {
        public ICollection<Field> Fields { get; set; }
        public State() : base()
        {
        }

        public State(ProcessDefinitionCreationContext creationContext) : base(creationContext)
        {
        }

        public override void ReadProcessData(XmlElement xmlElement)
        {
            base.ReadProcessData(xmlElement);

            creationContext.State = this;
            this.Fields = new HashSet<Field>();
            foreach (XmlElement child in xmlElement.GetChildElements("field"))
            {
                Field field = new Field(creationContext);
                field.ReadProcessData(child);
                Fields.Add(field);
            }
            creationContext.State = null;
        }
    }
}
