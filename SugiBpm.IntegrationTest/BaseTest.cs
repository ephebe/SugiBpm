using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SunStone.EntityFramework;
using Autofac;
using SunStone.Data;
using Microsoft.Practices.ServiceLocation;
using SugiBpm.Definition.Domain;
using SugiBpm.Execution.Domain;
using SugiBpm.Delegation.Domain;
using SugiBpm.Organization.Domain;
using SugiBpm.Delegation.Interface;
using SugiBpm.Organization.Application;
using SugiBpm.Definition.Application;
using SugiBpm.Execution.Application;
using Serilog;

namespace SugiBpm.IntegrationTest
{
    [TestClass]
    public class BaseTest
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
            builder.RegisterType<EFRepository<ActionDef>>().As<IRepository<ActionDef>>();
            builder.RegisterType<EFRepository<Transition>>().As<IRepository<Transition>>();
            builder.RegisterType<EFRepository<ProcessBlock>>().As<IRepository<ProcessBlock>>();
            builder.RegisterType<EFRepository<ProcessDefinition>>().As<IRepository<ProcessDefinition>>();
            builder.RegisterType<EFRepository<AttributeDef>>().As<IRepository<AttributeDef>>();
            builder.RegisterType<EFRepository<Field>>().As<IRepository<Field>>();

            builder.RegisterType<EFRepository<User>>().As<IRepository<User>>();
            builder.RegisterType<EFRepository<Actor>>().As<IRepository<Actor>>();
            builder.RegisterType<EFRepository<Group>>().As<IRepository<Group>>();
            builder.RegisterType<EFRepository<Membership>>().As<IRepository<Membership>>();

            builder.RegisterType<EFRepository<ProcessInstance>>().As<IRepository<ProcessInstance>>();
            builder.RegisterType<EFRepository<Flow>>().As<IRepository<Flow>>();
            builder.RegisterType<EFRepository<AttributeInstance>>().As<IRepository<AttributeInstance>>();

            builder.RegisterType<DelegationHelper>().As<IDelegationHelper>();
            builder.RegisterType<EFRepository<DelegationDef>>().As<IRepository<DelegationDef>>();

            builder.RegisterType<OrganizationApplication>().As<IOrganizationApplication>();
            builder.RegisterType<ProcessDefinitionApplication>().As<IProcessDefinitionApplication>();
            builder.RegisterType<ExecutionApplication>().As<IExecutionApplication>();

            var container = builder.Build();

            Autofac.Extras.CommonServiceLocator.AutofacServiceLocator serviceLocator
                = new Autofac.Extras.CommonServiceLocator.AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
