using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SugiBpm.Delegation.Interface.Organization;

namespace SugiBpm.Delegation.Interface
{
    public interface IOrganizationApplication
    {
        IActor FindActor(Guid actorId);
        IActor FindActorByUniqueName(string uniqueName);
        IGroup FindGroupByUniqueName(string uniqueName);
        IGroup FindGroupByUserMembership(string userUniqueName, string membershipType);
        IList<IUser> FindUsersByGroupAndRole(string groupUniqueName, string roleName);
        void CreateUser(string uniqueName, string firstName, string lastName, string email);
        void CreateGroup(string uniqueName, string groupName);
    }
}
