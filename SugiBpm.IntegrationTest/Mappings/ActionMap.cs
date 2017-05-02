using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.IntegrationTest.Mappings
{
    public class ActionMap : EntityTypeConfiguration<ActionDef>
    {
        public ActionMap()
            : this("dbo")
        {
        }

        public ActionMap(string schema)
        {
            ToTable(schema + ".ACTION");
            Property(p => p.DefinitionObjectId).HasColumnName("definitionObject").HasColumnType("uniqueidentifier").IsRequired();
            Property(p => p.EventType).HasColumnName("eventType").HasColumnType("int").IsRequired();
            HasOptional(r => r.ActionDelegation).WithOptionalDependent().Map(m => m.MapKey("actionDelegation"));
        }
    }
}
