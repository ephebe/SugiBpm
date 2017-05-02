using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Definition.Test.Mappings
{
    public class FieldMap : EntityTypeConfiguration<Field>
    {
        public FieldMap()
            : this("dbo")
        {
        }

        public FieldMap(string schema)
        {
            ToTable(schema + ".FIELD");
            Property(p => p.Name).HasColumnName("name").HasColumnType("nvarchar").HasMaxLength(255);
            Property(p => p.Description).HasColumnName("desciption").HasColumnType("nvarchar").HasMaxLength(255);
            //Property(p => p.AttributeId).HasColumnName("attribute");
            //Property(p => p.StateId).HasColumnName("state");
            HasOptional(o => o.Attribute).WithMany().Map(m => m.MapKey("attribute"));
            //HasOptional(o => o.State).WithMany().Map(m => m.MapKey("state"));
            Property(p => p.Access.AccessType).HasColumnName("fieldAccess");
        }
    }
}
