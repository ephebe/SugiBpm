using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Definition.Test.Mappings
{
    public class JoinMap : EntityTypeConfiguration<Join>
    {
        public JoinMap()
        {
            Property(p => p.JoinDelegationId).IsOptional();
            HasOptional(o => o.JoinDelegation).WithMany().HasForeignKey(f => f.JoinDelegationId);
        }
    }
}
