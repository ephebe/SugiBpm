using System.Xml;
using System;
using SugiBpm.Delegation.Interface.Definition;

namespace SugiBpm.Definition.Domain
{
    public class ProcessDefinition : ProcessBlock,IProcessDefinition
    {
        public Guid StartStateId { get; set; }
        public Guid EndStateId { get; set; }
        public Guid? AuthorizationDelegationId { get; set; }
        //public DelegationDef AuthorizationDelegation { get; set; }
        public string ResponsibleUserName { get; set; }

        public ProcessDefinition() : base()
        {
        }

        public ProcessDefinition(ProcessDefinitionCreationContext creationContext) : base(creationContext)
        {
        }

        public override void ReadProcessData(XmlElement xmlElement)
        {
            if (xmlElement == null)
                throw new ArgumentException("ProcessDefinition's ReadProcessData's xmlElement is null");

            StartState startState = new StartState(creationContext);
            this.StartStateId = startState.Id;
            EndState endState = new EndState(creationContext);
            this.EndStateId = endState.Id;

            creationContext.ProcessBlock = this;
            base.ReadProcessData(xmlElement);
            XmlElement startElement = xmlElement.GetChildElement("start-state");
            XmlElement endElement = xmlElement.GetChildElement("end-state");
            startState.ReadProcessData(startElement);
            endState.ReadProcessData(endElement);
            creationContext.ProcessBlock = null;

            Nodes.Add(startState);
            Nodes.Add(endState);

            XmlElement authorizationElement = xmlElement.GetChildElement("authorization");
            if (authorizationElement != null)
            {
                creationContext.DelegatingObject = this;
                var authorizationDelegation = new DelegationDef(creationContext);
                authorizationDelegation.ReadProcessData(authorizationElement);
                creationContext.DelegatingObject = null;
            }

            this.ResponsibleUserName = xmlElement.GetProperty("responsible");
        }

    }
}
