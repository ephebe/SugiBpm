using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Web.Mappings
{
    public class DecisionMap : EntityTypeConfiguration<Decision>
    {
        public DecisionMap()
        {
            Property(p => p.DecisionDelegationId).HasColumnName("decisionDelegation").IsOptional();
            HasOptional(o => o.DecisionDelegation).WithMany().HasForeignKey(f => f.DecisionDelegationId);
        }
    }
}
