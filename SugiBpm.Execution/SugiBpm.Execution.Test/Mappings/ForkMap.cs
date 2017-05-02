using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Test.Mappings
{
    public class ForkMap : EntityTypeConfiguration<Fork>
    {
        public ForkMap()
        {
            HasOptional(o => o.ForkDelegation).WithOptionalDependent().Map(m => m.MapKey("forkDelegation"));
        }
    }
}
