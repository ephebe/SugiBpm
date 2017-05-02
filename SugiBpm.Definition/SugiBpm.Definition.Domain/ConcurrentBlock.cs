using System;
using System.Xml;

namespace SugiBpm.Definition.Domain
{
    public class ConcurrentBlock : ProcessBlock
    {
        public Guid ForkId { get; set; }
        public Guid JoinId { get; set; }

        public ConcurrentBlock() : base()
        {
        }

        public ConcurrentBlock(ProcessDefinitionCreationContext creationContext) : base(creationContext)
        {
        }

        public override void ReadProcessData(XmlElement xmlElement)
        {
            ParentBlock = creationContext.ProcessBlock;
            Fork fork = new Fork(creationContext);
            this.ForkId = fork.Id;
            Join join = new Join(creationContext);
            this.JoinId = join.Id;

            creationContext.ProcessBlock = this;
            base.ReadProcessData(xmlElement);
            XmlElement forkElement = xmlElement.GetChildElement("fork");
            fork.ReadProcessData(forkElement);
            XmlElement joinElement = xmlElement.GetChildElement("join");
            join.ReadProcessData(joinElement);
            creationContext.ProcessBlock = ParentBlock;

            this.Nodes.Add(join);
            this.Nodes.Add(fork);
        }
    }
}
