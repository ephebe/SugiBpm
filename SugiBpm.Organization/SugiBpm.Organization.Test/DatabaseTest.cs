using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SunStone.EntityFramework;
using Autofac;
using SunStone.Data;
using SugiBpm.Organization.Domain;
using Microsoft.Practices.ServiceLocation;
using SunStone.Util;
using System.Linq;

namespace SugiBpm.Organization.Test
{
    [TestClass]
    public class DatabaseTest
    {
        [TestInitialize]
        public void SetUp()
        {
            EFUnitOfWorkFactory.SetObjectContextProvider(() =>
            {
                var context = new SugiBpmContext();
                return context;
            });

            var builder = new ContainerBuilder();
            builder.RegisterType<EFUnitOfWorkFactory>().As<IUnitOfWorkFactory>();
            builder.RegisterType<EFRepository<Actor>>().As<IRepository<Actor>>();
            builder.RegisterType<EFRepository<Membership>>().As<IRepository<Membership>>();
            var container = builder.Build();

            Autofac.Extras.CommonServiceLocator.AutofacServiceLocator serviceLocator
                = new Autofac.Extras.CommonServiceLocator.AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        [TestMethod]
        public void CreateUser()
        {
            Guid id;
            using (var unitWork = UnitOfWork.Start())
            {
                User user = new User("hugo");
                user.FirstName = "Hugo";
                user.LastName = "Cheng";
                user.Email = "hugo@msn.com";

                id = user.Id;

                var userRepository = new EFRepository<User>();
                userRepository.Add(user);
                unitWork.Flush();

            }

            using (var unitWork = UnitOfWork.Start())
            {
                var userRepository = new EFRepository<User>();
                var user = userRepository.Get(id);

                Assert.IsNotNull(user);
            }
        }

        [TestMethod]
        public void CreateGroup()
        {
            Guid id;
            using (var unitWork = UnitOfWork.Start())
            {
                Group group = new Group("C#");
                group.Name = "C# Team";
               
                Group parentGroup = new Group("IT");
                parentGroup.Name = "IT Team";

                group.Parent = parentGroup;

                id = group.Id;

                var groupRepository = new EFRepository<Group>();
                groupRepository.Add(group);
                groupRepository.Add(parentGroup);
                unitWork.Flush();

            }

            using (var unitWork = UnitOfWork.Start())
            {
                var groupRepository = new EFRepository<Group>();
                var group = groupRepository.Get(id);

                Assert.IsNotNull(group);
            }
        }

        [TestMethod]
        public void CreateMemberShip()
        {
            using (var unitWork = UnitOfWork.Start())
            {
                var userRepository = new EFRepository<User>();

                Membership membership = new Membership(SequentialGuid.NewGuid());
                membership.Role = "What";
                membership.Type = "Who";
                membership.User = userRepository.Single(s => s.FirstName == "Hugo");

                var membershipRepository = new EFRepository<Membership>();
                membershipRepository.Add(membership);
                unitWork.Flush();
            }
        }
    }
}
