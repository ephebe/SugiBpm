using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Test.Mappings
{
    public class DecisionMap : EntityTypeConfiguration<Decision>
    {
        public DecisionMap()
        {
            HasOptional(o => o.DecisionDelegation).WithOptionalDependent().Map(m => m.MapKey("decisionDelegation"));
        }
    }
}
