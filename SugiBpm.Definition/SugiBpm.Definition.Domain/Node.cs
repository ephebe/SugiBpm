using SugiBpm.Delegation.Interface.Definition;
using System;
using System.Collections.Generic;
using System.Xml;

namespace SugiBpm.Definition.Domain
{
    public class Node : DefinitionObject,INode
    {
        /// <summary>
        /// 在Execution時，不使用這個Property
        /// </summary>
        public ICollection<Transition> ArrivingTransitions { get; set; }
        /// <summary>
        /// 在Execution時，不使用這個Property
        /// </summary>
        public ICollection<Transition> LeavingTransitions { get; set; }
        public Guid ProcessBlockId { get; set; }

        public Node() : base()
        {
        }

        public Node(ProcessDefinitionCreationContext creationContext) : base(creationContext)
        {
        }

        public override void ReadProcessData(XmlElement xmlElement)
        {
            base.ReadProcessData(xmlElement);

            ArrivingTransitions = new HashSet<Transition>();
            LeavingTransitions = new HashSet<Transition>();
            ProcessBlockId = creationContext.ProcessBlock.Id;

            creationContext.Node = this;
            creationContext.TransitionDestinationScope = creationContext.ProcessBlock;
            foreach (XmlElement transitionElement in xmlElement.GetChildElements("transition"))
            {
                Transition transition = new Transition(creationContext);
                transition.ReadProcessData(transitionElement);
                LeavingTransitions.Add(transition);
            }
            creationContext.TransitionDestinationScope = null;
            creationContext.Node = null;
            creationContext.AddReferencableObject(Name, RegisteredScope(), typeof(Node),this);
        }
    }
}
