using SugiBpm.Delegation.Interface.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Organization.Domain
{
    public class Group : Actor, IGroup
    {
        public string Type { get; set; }

        public override string Name
        {
            get { return this.GroupName; }
            set { this.GroupName = value; }
        }

        public string GroupName { get; set; }

        public ICollection<Membership> Memberships { get; set; }

        public Group Parent { get; set; }

        public ICollection<Group> Children { get; set; }

        IGroup IGroup.Parent
        {
            get
            {
                return this.Parent;
            }
        }

        protected Group()
           : base()
        { }

        public Group(string uniqueName)
            : base(uniqueName)
        { }
    }
}
