using SugiBpm.Definition.Domain;
using SugiBpm.Delegation.Interface.Execution;
using SugiBpm.Delegation.Interface.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Domain
{
    public class ProcessInstance : IProcessInstance
    {
        public Guid Id { get; set; }
        public Guid ProcessDefinitionId { get; protected set; }
        public ProcessDefinition ProcessDefinition { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public Flow RootFlow { get; set; }
        public Flow SuperProcessFlow { get; internal set; }
        public Guid InitiatorActorId { get; set; }

        public ProcessInstance()
        {
        }

        public ProcessInstance(IActor initiatorActor,ProcessDefinition processDefinition)
        {
            this.Id = SunStone.Util.SequentialGuid.NewGuid();
            this.ProcessDefinition = processDefinition;
            this.RootFlow = new Flow(initiatorActor,this);
            this.InitiatorActorId = initiatorActor.Id;
        }
    }
}
