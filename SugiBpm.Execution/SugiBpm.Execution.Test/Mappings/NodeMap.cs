using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Test.Mappings
{
    public class NodeMap : EntityTypeConfiguration<Node>
    {
        public NodeMap()
            : this("dbo")
        {
        }

        public NodeMap(string schema)
        {
            ToTable(schema + ".NODE");
            HasKey(x => x.Id);
            Property(p => p.Name).HasColumnName("name").HasColumnType("nvarchar").HasMaxLength(255);
            Property(p => p.Description).HasColumnName("desciption").HasColumnType("nvarchar").HasMaxLength(255);
            Property(p => p.ProcessBlockId).HasColumnName("processBlock");
            //Property(p => p.ProcessDefinitionId).HasColumnName("processDefinition");
            HasOptional(s => s.ProcessDefinition).WithMany().Map(m => m.MapKey("processDefinition"));
            Map<State>(m => m.Requires("subclass").HasValue("State"));
            Map<ActivityState>(m => m.Requires("subclass").HasValue("ActivityState"));
            Map<StartState>(m => m.Requires("subclass").HasValue("StartState"));
            Map<EndState>(m => m.Requires("subclass").HasValue("EndState"));
            Map<ProcessState>(m => m.Requires("subclass").HasValue("ProcessState"));
            Map<Decision>(m => m.Requires("subclass").HasValue("Decision"));
            Map<Fork>(m => m.Requires("subclass").HasValue("Fork"));
            Map<Join>(m => m.Requires("subclass").HasValue("Join"));
            //HasMany(m => m.LeavingTransitions).WithOptional(r => r.From).Map(m => m.MapKey("from_"));
            //HasMany(m => m.LeavingTransitions).WithRequired(r => r.From).HasForeignKey(f => f.FromId);
            //HasMany(m => m.ArrivingTransitions).WithOptional(r => r.To).Map(m => m.MapKey("to_"));
            //HasMany(m => m.ArrivingTransitions).WithRequired(r => r.To).HasForeignKey(f => f.ToId);
            //HasOptional(o => o.ProcessBlock).WithMany(m => m.Nodes).Map(m => m.MapKey("processBlock"));
            //HasRequired(o => o.ProcessBlock).WithMany(m => m.Nodes).HasForeignKey(f => f.ProcessBlockId);
        }
    }
}
