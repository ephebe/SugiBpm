using SunStone.Util;
using System;
using System.Xml;

namespace SugiBpm.Definition.Domain
{
    public class ActionDef
    {
        public Guid Id { get; set; }
        public Guid DefinitionObjectId { get; set; }
        public EventType EventType { get; set; }
        public Guid ActionDelegationId { get; set; }
        public DelegationDef ActionDelegation { get; set; }


        private ProcessDefinitionCreationContext creationContext;
        public ActionDef()
        {
        }

        public ActionDef(ProcessDefinitionCreationContext creationContext)
        {
            this.Id = SequentialGuid.NewGuid();
            this.creationContext = creationContext;
        }

        public void ReadProcessData(XmlElement xmlElement)
        {
            DefinitionObject definitionObject = creationContext.DefinitionObject;

            this.DefinitionObjectId = definitionObject.Id;

            EventType = (EventType)Enum.Parse(typeof(EventType), xmlElement.GetAttribute("event").ToUpper());

            creationContext.DelegatingObject = this;
            this.ActionDelegation = new DelegationDef(creationContext);
            this.ActionDelegation.ReadProcessData(xmlElement);

            creationContext.DelegatingObject = null;
        }
    }
}
