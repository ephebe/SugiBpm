using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Definition.Test.Mappings
{
    public class DelegationMap : EntityTypeConfiguration<DelegationDef>
    {
        public DelegationMap()
            : this("dbo")
        {
        }

        public DelegationMap(string schema)
        {
            ToTable(schema + ".DELEGATION");
            HasKey(x => x.Id);
            Property(p => p.ClassName).HasColumnName("className").HasColumnType("nvarchar").HasMaxLength(255).IsOptional();
            Property(p => p.Configuration).HasColumnName("configuration").HasColumnType("nvarchar").HasMaxLength(255).IsOptional();
            Property(p => p.ExceptionHandlingType).HasColumnName("exceptionHandler").HasColumnType("int").IsOptional();
            //Property(p => p.ProcessDefinitionId).HasColumnName("processDefinition");
            HasOptional(s => s.ProcessDefinition).WithMany().Map(m => m.MapKey("processDefinition"));
        }
    }
}
