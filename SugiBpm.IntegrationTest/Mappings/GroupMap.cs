using SugiBpm.Organization.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.IntegrationTest.Mappings
{
    public class GroupMap : EntityTypeConfiguration<Group>
    {
        public GroupMap()
        {
            Property(p => p.GroupName).HasColumnName("name").HasColumnType("nvarchar").HasMaxLength(255).IsOptional();
            Property(p => p.Type).HasColumnName("type_").HasColumnType("nvarchar").HasMaxLength(255).IsOptional();
            HasMany(s => s.Memberships).WithOptional(s => s.Group).Map(m => m.MapKey("group_"));
            HasOptional(s => s.Parent).WithMany(s => s.Children).Map(m => m.MapKey("parent"));
        }
    }
}
