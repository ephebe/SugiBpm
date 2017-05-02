using System.Xml;

namespace SugiBpm.Definition.Domain
{
    public class EndState : ActivityState
    {
        public EndState() : base()
        {
        }

        public EndState(ProcessDefinitionCreationContext creationContext) : base(creationContext)
        {
        }

        public override void ReadProcessData(XmlElement xmlElement)
        {
            base.ReadProcessData(xmlElement);
        }
    }
}
