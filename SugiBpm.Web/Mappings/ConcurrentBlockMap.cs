using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Web.Mappings
{
    public class ConcurrentBlockMap : EntityTypeConfiguration<ConcurrentBlock>
    {
        public ConcurrentBlockMap()
            : this("dbo")
        {
        }

        public ConcurrentBlockMap(string schema)
        {
            Property(p => p.ForkId).HasColumnName("fork_");
            Property(p => p.JoinId).HasColumnName("join_");
            //HasOptional(s => s.Fork).WithOptionalDependent().Map(m => m.MapKey("fork"));
            //HasOptional(s => s.Join).WithOptionalDependent().Map(m => m.MapKey("join_"));
        }
    }
}
