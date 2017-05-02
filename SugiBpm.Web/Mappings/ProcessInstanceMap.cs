using SugiBpm.Execution.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Web.Mappings
{
    public class ProcessInstanceMap : EntityTypeConfiguration<ProcessInstance>
    {
        public ProcessInstanceMap()
            : this("dbo")
        {

        }

        public ProcessInstanceMap(string schema)
        {
            ToTable(schema + ".PROCESSINSTANCE");
            HasKey(x => x.Id);
            Property(p => p.Start).HasColumnName("start").HasColumnType("datetime").IsOptional();
            Property(p => p.End).HasColumnName("end").HasColumnType("datetime").IsOptional();
            Property(p => p.InitiatorActorId).HasColumnName("initiatorActorId").IsOptional();
            Property(p => p.ProcessDefinitionId).HasColumnName("processDefinition").IsOptional();
            HasOptional(o => o.ProcessDefinition).WithMany().HasForeignKey(f => f.ProcessDefinitionId);
            HasOptional(o => o.RootFlow).WithOptionalDependent().Map(m => m.MapKey("rootFlow"));
            HasOptional(o => o.SuperProcessFlow).WithOptionalDependent().Map(m => m.MapKey("superProcessFlow"));
        }
    }
}
