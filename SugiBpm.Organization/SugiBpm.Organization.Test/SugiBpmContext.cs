using SugiBpm.Organization.Test.Mappings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Organization.Test
{
    public class SugiBpmContext : DbContext
    {
        public SugiBpmContext()
           : base("Name=SugiBpmContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new ActorMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new GroupMap());
            modelBuilder.Configurations.Add(new MembershipMap());
        }
    }
}
