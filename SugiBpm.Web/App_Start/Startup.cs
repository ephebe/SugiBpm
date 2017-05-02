using Autofac;
using Microsoft.Practices.ServiceLocation;
using Serilog;
using SugiBpm.Definition.Application;
using SugiBpm.Definition.Domain;
using SugiBpm.Delegation.Domain;
using SugiBpm.Delegation.Interface;
using SugiBpm.Execution.Application;
using SugiBpm.Execution.Domain;
using SugiBpm.Organization.Application;
using SugiBpm.Organization.Domain;
using SunStone.Data;
using SunStone.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SugiBpm.Web.App_Start
{
    public static class Startup
    {
        public static void ConfigureContextProvider()
        {
            EFUnitOfWorkFactory.SetObjectContextProvider(() =>
            {
                var context = new SugiBpmContext();
                return context;
            });
        }

        public static void ConfigureAutofac()
        {
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
        }

        public static void ConfigureLog()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.RollingFile("logfiles/sugibmp-{Date}.txt")
                .CreateLogger();
        }
    }
}