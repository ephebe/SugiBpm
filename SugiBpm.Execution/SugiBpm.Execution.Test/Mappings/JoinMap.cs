using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Test.Mappings
{
    public class JoinMap : EntityTypeConfiguration<Join>
    {
        public JoinMap()
        {
            HasOptional(o => o.JoinDelegation).WithOptionalDependent().Map(m => m.MapKey("joinDelegation"));
        }
    }
}
