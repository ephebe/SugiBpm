using SugiBpm.Delegation.Interface.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Organization.Domain
{
    public class User : Actor,IUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public ICollection<Membership> Memberships { get; set; }

        public override string Name
        {
            get { return FirstName + " " + LastName; }
            set
            {
            }
        }

        protected User()
            : base()
        { }

        public User(string uniqueName)
            : base(uniqueName)
        { }
    }
}
