using SugiBpm.Organization.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.IntegrationTest.Mappings
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            //Ignore(p => p.Name);
            Property(p => p.FirstName).HasColumnName("firstName").HasColumnType("nvarchar").HasMaxLength(255).IsOptional();
            Property(p => p.LastName).HasColumnName("lastName").HasColumnType("nvarchar").HasMaxLength(255).IsOptional();
            Property(p => p.Email).HasColumnName("email").HasColumnType("nvarchar").HasMaxLength(255).IsOptional();
            HasMany(s => s.Memberships).WithOptional(s=>s.User).Map(m => m.MapKey("user_"));
           
        }
    }
}
