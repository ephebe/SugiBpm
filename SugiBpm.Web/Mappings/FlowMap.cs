using SugiBpm.Execution.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Web.Mappings
{
    public class FlowMap : EntityTypeConfiguration<Flow>
    {
        public FlowMap()
            : this("dbo")
        {
        }

        public FlowMap(string schema)
        {
            ToTable(schema + ".FLOW");
            HasKey(x => x.Id);
            Property(p => p.Name).HasColumnName("name").HasColumnType("nvarchar").HasMaxLength(255).IsOptional();
            Property(p => p.Start).HasColumnName("start").HasColumnType("datetime").IsOptional();
            Property(p => p.End).HasColumnName("end").HasColumnType("datetime").IsOptional();
            Property(p => p.ActorId).HasColumnName("actorId").IsOptional();
            Property(p => p.ParentReactivation).HasColumnName("parentReactivation").IsOptional();
            Property(p => p.ProcessInstanceId).HasColumnName("processInstance");
            HasOptional(o => o.Node).WithMany().Map(m => m.MapKey("node"));
            HasMany(m => m.Children).WithOptional(o => o.Parent).Map(m => m.MapKey("parent"));
            HasMany(m => m.AttributeInstances).WithOptional(o => o.Scope).Map(m => m.MapKey("scope"));
        }
    }
}
