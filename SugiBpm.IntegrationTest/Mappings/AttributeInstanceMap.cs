using SugiBpm.Execution.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.IntegrationTest.Mappings
{
    public class AttributeInstanceMap : EntityTypeConfiguration<AttributeInstance>
    {
        public AttributeInstanceMap()
            : this("dbo")
        {
        }

        public AttributeInstanceMap(string schema)
        {
            ToTable(schema + ".ATTRIBUTEINSTANCE");
            HasKey(x => x.Id);
            Property(p => p.Name).HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(255).IsOptional();
            Property(p => p.ValueText).HasColumnName("valueText").HasColumnType("nvarchar").HasMaxLength(255).IsOptional();
            //HasOptional(o => o.Scope).WithMany(m => m.AttributeInstances).Map(m => m.MapKey("scope"));
            Property(p => p.AttributeId).HasColumnName("attribute");
            Property(p => p.AttributeName).HasColumnName("attributeName").HasColumnType("nvarchar").HasMaxLength(255);
        }
    }
}
