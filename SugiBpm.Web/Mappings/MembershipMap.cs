using SugiBpm.Organization.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Web.Mappings
{
    public class MembershipMap : EntityTypeConfiguration<Membership>
    {
        public MembershipMap()
            : this("dbo")
        {
        }

        public MembershipMap(string schema)
        {
            ToTable(schema + ".MEMBERSHIP");
            HasKey(x => x.Id);
            Property(p => p.Role).HasColumnName("role").HasColumnType("nvarchar").HasMaxLength(255);
            Property(p => p.Type).HasColumnName("type_").HasColumnType("nvarchar").HasMaxLength(255);
        }
    }
}
