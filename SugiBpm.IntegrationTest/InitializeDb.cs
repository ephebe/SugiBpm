using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SugiBpm.Delegation.Interface;
using SugiBpm.Organization.Domain;
using SunStone.Data;
using SunStone.EntityFramework;
using SunStone.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.IntegrationTest
{
    [TestClass]
    public class InitializeDb : BaseTest
    {
        [TestMethod]
        public void InitOrganAndBuildDBSchema()
        {
            using (var unitWork = UnitOfWork.Start())
            {
                User user = new User("hugo");
                user.FirstName = "Hugo";
                user.LastName = "Cheng";
                user.Email = "hugo@gmail.com";

                User user1 = new User("jimmy");
                user1.FirstName = "Jimmy";
                user1.LastName = "Yang";
                user1.Email = "jimmy@hotmail.com";

                User user2 = new User("jenny");
                user2.FirstName = "jenny";
                user2.LastName = "Chen";
                user2.Email = "jenny@gmail.com";

                var userRepository = new EFRepository<User>();
                userRepository.Add(user);
                userRepository.Add(user1);
                userRepository.Add(user2);

                Group group = new Group("C#");
                group.Name = "C# Team";

                var groupRepository = new EFRepository<Group>();
                groupRepository.Add(group);

                Membership membership = new Membership(SequentialGuid.NewGuid());
                membership.Type = "hierarchy";
                membership.Role = "Member";
                membership.User = user;
                membership.Group = group;

                Membership membership1 = new Membership(SequentialGuid.NewGuid());
                membership1.Type = "hierarchy";
                membership1.Role = "boss";
                membership1.User = user1;
                membership1.Group = group;

                Membership membership2 = new Membership(SequentialGuid.NewGuid());
                membership2.Type = "hierarchy";
                membership2.Role = "hr-responsible";
                membership2.User = user2;
                membership2.Group = group;

                var membershipRepository = new EFRepository<Membership>();
                membershipRepository.Add(membership);
                membershipRepository.Add(membership1);
                membershipRepository.Add(membership2);

                unitWork.Flush();
            }
        }

        [TestMethod]
        public void InitProcessDefinition()
        {
            var processDefinitionApplication = ServiceLocator.Current.GetInstance<IProcessDefinitionApplication>();
            processDefinitionApplication.DeployProcessArchive("Definitions\\HelloWorld0.xml");
            processDefinitionApplication.DeployProcessArchive("Definitions\\HelloWorld1.xml");
            processDefinitionApplication.DeployProcessArchive("Definitions\\HelloWorld2.xml");
            processDefinitionApplication.DeployProcessArchive("Definitions\\HelloWorld3.xml");
            processDefinitionApplication.DeployProcessArchive("Definitions\\Holiday.xml");
        }
    }
}
