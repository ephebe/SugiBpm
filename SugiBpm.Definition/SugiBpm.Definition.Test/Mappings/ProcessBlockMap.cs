using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Definition.Test.Mappings
{
    public class ProcessBlockMap : EntityTypeConfiguration<ProcessBlock>
    {
        public ProcessBlockMap()
            : this("dbo")
        {
        }

        public ProcessBlockMap(string schema)
        {
            ToTable(schema + ".PROCESSBLOCK");
            HasKey(x => x.Id);
            Property(p => p.Name).HasColumnName("name").HasColumnType("nvarchar").HasMaxLength(255);
            Property(p => p.Description).HasColumnName("desciption").HasColumnType("nvarchar").HasMaxLength(255);
            HasOptional(s => s.ProcessDefinition).WithMany().Map(m => m.MapKey("processDefinition"));
            //Property(p => p.ProcessDefinitionId).HasColumnName("processDefinition");
            Map<ProcessDefinition>(m => m.Requires("subclass").HasValue("ProcessDefinition"));
            Map<ConcurrentBlock>(m => m.Requires("subclass").HasValue("ConcurrentBlock"));
            HasMany(m => m.Nodes).WithOptional().HasForeignKey(f => f.ProcessBlockId);
            //HasMany(m => m.Nodes).WithRequired(w => w.ProcessBlock).HasForeignKey(f => f.ProcessBlockId).WillCascadeOnDelete(false);
            HasMany(m => m.Attributes).WithOptional(w => w.Scope).Map(m => m.MapKey("scope"));
            HasMany(m => m.ChildBlocks).WithOptional(w => w.ParentBlock).Map(m => m.MapKey("parentBlock"));
        }
    }
}
