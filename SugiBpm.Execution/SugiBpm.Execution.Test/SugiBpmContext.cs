using SugiBpm.Execution.Test.Mappings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Test
{
    public class SugiBpmContext : DbContext
    {
        public SugiBpmContext()
            : base("Name=SugiBpmContext")
        {
           
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new ActionMap());
            modelBuilder.Configurations.Add(new ActivityStateMap());
            modelBuilder.Configurations.Add(new AttributeMap());
            modelBuilder.Configurations.Add(new ConcurrentBlockMap());
            modelBuilder.Configurations.Add(new DecisionMap());
            modelBuilder.Configurations.Add(new DelegationMap());
            modelBuilder.Configurations.Add(new FieldMap());
            modelBuilder.Configurations.Add(new ForkMap());
            modelBuilder.Configurations.Add(new JoinMap());
            modelBuilder.Configurations.Add(new NodeMap());
            modelBuilder.Configurations.Add(new ProcessBlockMap());
            modelBuilder.Configurations.Add(new ProcessDefinitionMap());
            modelBuilder.Configurations.Add(new StateMap());
            modelBuilder.Configurations.Add(new TransitionMap());
            modelBuilder.Configurations.Add(new ProcessInstanceMap());
            modelBuilder.Configurations.Add(new FlowMap());
            modelBuilder.Configurations.Add(new AttributeInstanceMap());
        }

       
    }
}
