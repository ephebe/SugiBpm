using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Test.Mappings
{
    public class StateMap : EntityTypeConfiguration<State>
    {
        public StateMap()
        {
            HasMany(m => m.Fields).WithOptional(o => o.State).Map(m => m.MapKey("state"));
        }
    }
}
