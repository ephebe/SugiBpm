using System.Collections.Generic;
using System.Xml;

namespace SugiBpm.Definition.Domain
{
    public class ProcessBlock : DefinitionObject
    {
        public ICollection<Node> Nodes { get; set; }
        public ICollection<AttributeDef> Attributes { get; set; }
        public ProcessBlock ParentBlock { get; set; }

        public ICollection<ProcessBlock> ChildBlocks { get; set; }

        public ProcessBlock() : base()
        {
            this.Nodes = new HashSet<Node>();
            this.Attributes = new HashSet<AttributeDef>();
            this.ChildBlocks = new HashSet<ProcessBlock>();
        }

        public ProcessBlock(ProcessDefinitionCreationContext creationContext) : base(creationContext)
        {
            this.Nodes = new HashSet<Node>();
            this.Attributes = new HashSet<AttributeDef>();
            this.ChildBlocks = new HashSet<ProcessBlock>();
        }

        public override void ReadProcessData(XmlElement xmlElement)
        {
            base.ReadProcessData(xmlElement);

            foreach (XmlElement attributeElement in xmlElement.GetChildElements("attribute"))
            {
                AttributeDef attribute = new AttributeDef(creationContext);
                attribute.ReadProcessData(attributeElement);
                Attributes.Add(attribute);
            }

            foreach (XmlElement activityStateElement in xmlElement.GetChildElements("activity-state"))
            {
                ActivityState activityState = new ActivityState(creationContext);
                activityState.ReadProcessData(activityStateElement);
                Nodes.Add(activityState);
            }

            foreach (XmlElement processStateElement in xmlElement.GetChildElements("process-state"))
            {
                ProcessState processState = new ProcessState(creationContext);
                processState.ReadProcessData(processStateElement);
                Nodes.Add(processState);
            }

            foreach (XmlElement decisionElement in xmlElement.GetChildElements("decision"))
            {
                Decision decision = new Decision(creationContext);
                decision.ReadProcessData(decisionElement);
                Nodes.Add(decision);
            }

            foreach (XmlElement concurrentBlockElement in xmlElement.GetChildElements("concurrent-block"))
            {
                ConcurrentBlock concurrentBlock = new ConcurrentBlock(creationContext);
                concurrentBlock.ReadProcessData(concurrentBlockElement);
                ChildBlocks.Add(concurrentBlock);
            }
        }
    }
}
