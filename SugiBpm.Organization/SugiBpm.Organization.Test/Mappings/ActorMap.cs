using SugiBpm.Organization.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Organization.Test.Mappings
{
    public class ActorMap : EntityTypeConfiguration<Actor>
    {
        public ActorMap()
            : this("dbo")
        {
        }

        public ActorMap(string schema)
        {
            ToTable(schema + ".ACTOR");
            HasKey(x => x.Id);
            Ignore(p => p.Name);

            Property(p => p.UniqueName).HasColumnName("ShortName").HasColumnType("nvarchar").HasMaxLength(255).IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_ShortName", 2) { IsUnique = true}));
            Map<User>(m => m.Requires("subclass").HasValue("User"));
            Map<Group>(m => m.Requires("subclass").HasValue("Group"));
        }
    }
}
