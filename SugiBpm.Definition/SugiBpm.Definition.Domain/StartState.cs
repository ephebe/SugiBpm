using System.Xml;

namespace SugiBpm.Definition.Domain
{
    public class StartState : ActivityState
    {
        public StartState() : base()
        {
        }

        public StartState(ProcessDefinitionCreationContext creationContext) : base(creationContext)
        {
        }

        public override void ReadProcessData(XmlElement xmlElement)
        {
            creationContext.DefinitionObject = this;
            base.ReadProcessData(xmlElement);
            creationContext.DefinitionObject = null;
        }
    }
}
