using System;
using System.Xml;

namespace SugiBpm.Definition.Domain
{
    public class ActivityState : State
    {
        public string ActorRoleName { get; set; }
        public Guid? AssignmentDelegationId { get; protected set; }
        public DelegationDef AssignmentDelegation { get; set; }

        public ActivityState() : base()
        {
        }

        public ActivityState(ProcessDefinitionCreationContext creationContext) : base(creationContext)
        {
        }

        public override void ReadProcessData(XmlElement xmlElement)
        {
            base.ReadProcessData(xmlElement);

            XmlElement assignmentElement = xmlElement.GetChildElement("assignment");
            if (assignmentElement != null)
            {
                creationContext.DelegatingObject = this;
                this.AssignmentDelegation = new DelegationDef(creationContext);
                this.AssignmentDelegation.ReadProcessData(assignmentElement);
                creationContext.DelegatingObject = null;
            }
            this.ActorRoleName = xmlElement.GetProperty("role");
        }
    }
}
