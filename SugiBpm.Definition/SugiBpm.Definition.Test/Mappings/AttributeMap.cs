using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Definition.Test.Mappings
{
    public class AttributeMap : EntityTypeConfiguration<AttributeDef>
    {
        public AttributeMap()
            : this("dbo")
        {
        }

        public AttributeMap(string schema)
        {
            ToTable(schema + ".ATTRIBUTE");
            HasKey(x => x.Id);
            Property(p => p.Name).HasColumnName("name").HasColumnType("nvarchar").HasMaxLength(255);
            Property(p => p.Description).HasColumnName("desciption").HasColumnType("nvarchar").HasMaxLength(255);
            //Property(p => p.ProcessDefinitionId).HasColumnName("processDefinition");
            HasOptional(s => s.ProcessDefinition).WithMany().Map(m => m.MapKey("processDefinition"));
            //Property(p => p.ProcessDefinitionId).HasColumnName("processDefinition").HasColumnType("uniqueidentifier");
            HasOptional(s => s.Scope).WithMany().Map(m => m.MapKey("scope"));
            Property(p => p.InitialValue).HasColumnName("initialValue").HasColumnType("nvarchar").HasMaxLength(255).IsOptional();
            HasOptional(s => s.SerializerDelegation).WithOptionalDependent().Map(m => m.MapKey("serializerDelegation"));
        }
    }
}
