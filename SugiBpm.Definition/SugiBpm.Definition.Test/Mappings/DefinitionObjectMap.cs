using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Definition.Test.Mappings
{
    /// <summary>
    /// 不可能，這種會產生自關聯，且包含條件是processDefinition的
    /// </summary>
    //public class DefinitionObjectMap : EntityTypeConfiguration<DefinitionObject>
    //{
    //    public DefinitionObjectMap()
    //    {
    //        HasKey(x => x.Id);
    //        Property(p => p.Name).HasColumnName("name").HasColumnType("nvarchar").HasMaxLength(255);
    //        Property(p => p.Description).HasColumnName("desciption").HasColumnType("nvarchar").HasMaxLength(255);
    //        HasOptional(s => s.ProcessDefinition).WithOptionalDependent().Map(m => m.MapKey("processDefinition"));

    //        Map<ProcessBlock>(m =>
    //        {
    //            m.MapInheritedProperties();
    //            m.ToTable("PROCESSBLOCK");
    //        });

    //        Map<Node>(m =>
    //        {
    //            m.MapInheritedProperties();
    //            m.ToTable("NODE");
    //        });

    //        Map<AttributeDef>(m =>
    //        {
    //            m.MapInheritedProperties();
    //            m.ToTable("ATTRIBUTE");
    //        });

    //        Map<Transition>(m =>
    //        {
    //            m.MapInheritedProperties();
    //            m.ToTable("TRANSITION");
    //        });
    //    }
    //}
}
