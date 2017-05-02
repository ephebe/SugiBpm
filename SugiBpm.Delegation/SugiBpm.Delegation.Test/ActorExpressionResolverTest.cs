using Autofac;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SugiBpm.Delegation.Components;
using SugiBpm.Delegation.Domain;
using SugiBpm.Delegation.Interface;
using SugiBpm.Execution.Domain;
using SugiBpm.Organization.Application;
using SugiBpm.Organization.Domain;
using SunStone.Data;
using SunStone.EntityFramework;
using SunStone.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Test
{
    [TestClass]
    public class ActorExpressionResolverTest
    {
        [TestInitialize]
        public void Setup()
        {
            EFUnitOfWorkFactory.SetObjectContextProvider(() =>
            {
                var context = new SugiBpmContext();
                return context;
            });

            var builder = new ContainerBuilder();
            builder.RegisterType<EFUnitOfWorkFactory>().As<IUnitOfWorkFactory>();
            builder.RegisterType<EFRepository<Actor>>().As<IRepository<Actor>>();
            builder.RegisterType<EFRepository<Group>>().As<IRepository<Group>>();
            builder.RegisterType<EFRepository<Membership>>().As<IRepository<Membership>>();
            builder.RegisterType<OrganizationApplication>().As<IOrganizationApplication>();
            var container = builder.Build();

            Autofac.Extras.CommonServiceLocator.AutofacServiceLocator serviceLocator
                = new Autofac.Extras.CommonServiceLocator.AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        [TestMethod]
        public void Init()
        {
            using (var unitWork = UnitOfWork.Start())
            {
                User user = new User("h.c");
                user.FirstName = "Hugo";
                user.LastName = "Cheng";
                user.Email = "ephebe@msn.com";

                Group group = new Group("c#");
                group.Name = "C# Team";

                var userRepository = new EFRepository<User>();
                var groupRepository = new EFRepository<Group>();

                //Type=hierarchy，似乎就是表示從屬的關係，Role則是表示已為從屬，且在此組織中扮演何角色
                Membership membership = new Membership(SequentialGuid.NewGuid());
                membership.Type = "hierarchy";
                membership.Role = "Member";
                membership.User = userRepository.Single(s => s.FirstName == "Hugo");
                membership.Group = groupRepository.Single(s => s.GroupName == "C# Team");

                var membershipRepository = new EFRepository<Membership>();
                membershipRepository.Add(membership);
                unitWork.Flush();
            }
        }

        [TestMethod]
        public void TestResolveArgumentActor()
        {
            ActorExpressionResolver resolver = new ActorExpressionResolver();
            ExecutionContext context = new ExecutionContext(null,null, null);
            var actor = resolver.ResolveArgument("Actor(h.c)", context);

            Assert.IsNotNull(actor);
        }

        [TestMethod]
        public void TestResolveArgumentGroup()
        {
            ActorExpressionResolver resolver = new ActorExpressionResolver();
            ExecutionContext context = new ExecutionContext(null,null, null);
            var group = resolver.ResolveArgument("Group(c#)", context);

            Assert.IsNotNull(group);

            var group2 = resolver.ResolveArgument("Actor(h.c) -> Group(hierarchy)", context);
            Assert.IsNotNull(group2);
        }

        [TestMethod]
        public void TestResolveArgumentRole()
        {
            ActorExpressionResolver resolver = new ActorExpressionResolver();
            ExecutionContext context = new ExecutionContext(null,null, null);

            var user = resolver.ResolveArgument("Actor(h.c) -> Group(hierarchy) -> Role(Member)", context);
            Assert.IsNotNull(user);
        }
    }
}
