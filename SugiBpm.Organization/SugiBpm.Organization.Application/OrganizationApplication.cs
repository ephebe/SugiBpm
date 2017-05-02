using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SugiBpm.Delegation.Interface.Organization;
using SunStone.Data;
using Microsoft.Practices.ServiceLocation;
using SugiBpm.Organization.Domain;

namespace SugiBpm.Organization.Application
{
    public class OrganizationApplication : IOrganizationApplication
    {
        public OrganizationApplication()
        {
        }

        public IActor FindActor(Guid actorId)
        {
            using (var scope = new UnitOfWorkScope())
            {
                var actorRepository = ServiceLocator.Current.GetInstance<IRepository<Actor>>();
                return actorRepository.Get(actorId);
            }
        }

        public IActor FindActorByUniqueName(string uniqueName)
        {
            using (var scope = new UnitOfWorkScope())
            {
                var actorRepository = ServiceLocator.Current.GetInstance<IRepository<Actor>>();
                return actorRepository.SingleOrDefault(q => q.UniqueName == uniqueName);
            }
        }

        public IGroup FindGroupByUserMembership(string userUniqueName, string membershipType)
        {
            using (var scope = new UnitOfWorkScope())
            {
                var memberShipRepository = ServiceLocator.Current.GetInstance<IRepository<Membership>>();

                var query =
                    from m in memberShipRepository
                    where m.User.UniqueName == userUniqueName && m.Type == membershipType
                    select m.Group;

                return query.SingleOrDefault();
            }
        }

        public IGroup FindGroupByUniqueName(string uniqueName)
        {
            using (var scope = new UnitOfWorkScope())
            {
                var groupRepository = ServiceLocator.Current.GetInstance<IRepository<Group>>();
                return groupRepository.SingleOrDefault(q => q.UniqueName == uniqueName);
            }
        }

        public IList<IUser> FindUsersByGroupAndRole(string groupUniqueName, string roleName)
        {
            using (var scope = new UnitOfWorkScope())
            {
                var memberShipRepository = ServiceLocator.Current.GetInstance<IRepository<Membership>>();

                var query =
                   from m in memberShipRepository
                   where m.Group.UniqueName == groupUniqueName && m.Role == roleName
                   select m.User;

                var result = query.ToList();
                return result.ConvertAll<IUser>((u) => { return u as IUser; });
            }
        }

        public void CreateUser(string uniqueName, string firstName, string lastName, string email)
        {
            using (var scope = new UnitOfWorkScope())
            {
                User user = new User(uniqueName);
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Email = email;

                var userRepository = ServiceLocator.Current.GetInstance<IRepository<User>>();
                userRepository.Add(user);
            }
        }

        public void CreateGroup(string uniqueName, string groupName)
        {
            using (var scope = new UnitOfWorkScope())
            {
                Group group = new Group(uniqueName);
                group.Name = groupName;

                var groupRepository = ServiceLocator.Current.GetInstance<IRepository<Group>>();
                groupRepository.Add(group);
            }
        }
    }
}
