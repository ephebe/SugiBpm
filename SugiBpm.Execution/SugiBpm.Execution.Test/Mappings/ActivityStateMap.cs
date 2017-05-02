using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Test.Mappings
{
    public class ActivityStateMap : EntityTypeConfiguration<ActivityState>
    {
        public ActivityStateMap()
        {
            Property(p => p.ActorRoleName).HasColumnName("actorRoleName").HasColumnType("nvarchar").HasMaxLength(255).IsOptional();
            //HasOptional(o => o.AssignmentDelegation).WithOptionalDependent().Map(m => m.MapKey("assignmentDelegation"));

        }
    }
}
