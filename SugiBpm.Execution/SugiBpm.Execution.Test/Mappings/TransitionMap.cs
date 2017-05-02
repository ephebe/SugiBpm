using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Test.Mappings
{
    public class TransitionMap : EntityTypeConfiguration<Transition>
    {
        public TransitionMap()
            : this("dbo")
        {
        }

        public TransitionMap(string schema)
        {
            ToTable(schema + ".TRANSITION");
            HasKey(x => x.Id);
            Property(p => p.Name).HasColumnName("name").HasColumnType("nvarchar").HasMaxLength(255);
            Property(p => p.Description).HasColumnName("desciption").HasColumnType("nvarchar").HasMaxLength(255);
            //Property(p => p.FromId).HasColumnName("from_");
            //Property(p => p.ToId).HasColumnName("to_");
            HasOptional(s => s.ProcessDefinition).WithMany().Map(m => m.MapKey("processDefinition"));
            //Property(p => p.ProcessDefinitionId).HasColumnName("processDefinition");
            HasOptional(s => s.From).WithMany(m => m.LeavingTransitions).Map(m => m.MapKey("from_"));
            //HasRequired(s => s.From).WithMany(m => m.LeavingTransitions).HasForeignKey(f => f.FromId).WillCascadeOnDelete(false);
            HasOptional(s => s.To).WithMany(m => m.ArrivingTransitions).Map(m => m.MapKey("to_"));
            //HasRequired(s => s.To).WithMany(m => m.ArrivingTransitions).HasForeignKey(f => f.ToId).WillCascadeOnDelete(false);
        }
    }
}
