using System.Xml;

namespace SugiBpm.Definition.Domain
{
    public class ProcessState : State
    {
        public ProcessState() : base()
        {
        }

        public ProcessState(ProcessDefinitionCreationContext creationContext) : base(creationContext)
        {
        }

        public override void ReadProcessData(XmlElement xmlElement)
        {

        }
    }
}
