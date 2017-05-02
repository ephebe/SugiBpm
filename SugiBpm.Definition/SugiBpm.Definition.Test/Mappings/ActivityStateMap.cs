using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Definition.Test.Mappings
{
    public class ActivityStateMap : EntityTypeConfiguration<ActivityState>
    {
        public ActivityStateMap()
        {
            Property(p => p.ActorRoleName).HasColumnName("actorRoleName").HasColumnType("nvarchar").HasMaxLength(255).IsOptional();
            Property(p => p.AssignmentDelegationId).HasColumnName("assignmentDelegation").IsOptional();
            HasOptional(o => o.AssignmentDelegation).WithMany().HasForeignKey(f => f.AssignmentDelegationId);

        }
    }
}
