using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Organization.Domain
{
    public class Membership
    {
        public Guid Id { get; protected set; }

        public string Role { get; set; }

        public string Type { get; set; }

        public Group Group { get; set; }

        public User User { get; set; }

        public Membership() { }

        public Membership(Guid id)
        {
            this.Id = id;
        }

        public override string ToString()
        {
            return "Membership[" + Id + "|" + User.Name + "|" + Group.Name + "]";
        }
    }
}
