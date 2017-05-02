using SugiBpm.Definition.Domain;
using SugiBpm.Delegation.Interface.Execution;
using SugiBpm.Delegation.Interface.Organization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Domain
{
    public class Flow : IFlow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProcessInstanceId { get; set; }
        public Node Node { get; set; }
        public ICollection<AttributeInstance> AttributeInstances { get; set; }
        public Flow Parent { get; set; }
        public ICollection<Flow> Children { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; internal set; }
        public bool ParentReactivation { get; set; }
        public Guid? ActorId { get; set; }

        public Flow()
        {
            this.Children = new HashSet<Flow>();
        }

        public Flow(IActor actor,ProcessInstance processInstance) 
            :this()
        {
            this.Id = SunStone.Util.SequentialGuid.NewGuid();
            this.Name = "root";
            this.ProcessInstanceId = processInstance.Id;
            this.Node = processInstance.ProcessDefinition.Nodes.Single(s => s is StartState);
            this.Start = DateTime.Now;
            this.ParentReactivation = true;
            this.ActorId = actor.Id;
            createAttributeInstances(processInstance.ProcessDefinition.Attributes);
        }

        public Flow(string name, Flow parentFlow, ProcessBlock processBlock)
            :this()
        {
            this.Id = SunStone.Util.SequentialGuid.NewGuid();
            Name = name;
            this.Parent = parentFlow;
            this.Start = DateTime.Now;
            this.ProcessInstanceId = parentFlow.ProcessInstanceId;
            this.ParentReactivation = true;
            createAttributeInstances(processBlock.Attributes);
        }

        private void createAttributeInstances(ICollection<AttributeDef> attributes)
        {
            this.AttributeInstances = new HashSet<AttributeInstance>();
            IEnumerator iter = attributes.GetEnumerator();
            foreach (var attribute in attributes)
            {
                String attributeName = attribute.Name;
                AttributeInstance attributeInstance = new AttributeInstance(attribute, this);
                attributeInstance.ValueText = attribute.InitialValue;
                this.AttributeInstances.Add(attributeInstance);
            }
        }

        public bool IsRootFlow()
        {
            return (Parent == null);
        }

        public override string ToString()
        {
            return "flow[" + Id + "|" + Name + "]";
        }
    }
}
