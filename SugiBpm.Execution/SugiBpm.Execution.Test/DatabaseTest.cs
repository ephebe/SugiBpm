using Autofac;
using FakeItEasy;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SugiBpm.Definition.Domain;
using SugiBpm.Execution.Domain;
using SugiBpm.Organization.Domain;
using SunStone.Data;
using SunStone.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SugiBpm.Execution.Test
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
            builder.RegisterType<EFRepository<ActionDef>>().As<IRepository<ActionDef>>();
            builder.RegisterType<EFRepository<Transition>>().As<IRepository<Transition>>();
            builder.RegisterType<EFRepository<ProcessBlock>>().As<IRepository<ProcessBlock>>();
            builder.RegisterType<EFRepository<Flow>>().As<IRepository<Flow>>();
            builder.RegisterType<FakeDelegationHelper>().As<IDelegationHelper>();
            var container = builder.Build();

            Autofac.Extras.CommonServiceLocator.AutofacServiceLocator serviceLocator
                = new Autofac.Extras.CommonServiceLocator.AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        [TestMethod]
        public void HelloWorld0()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Definitions\\HelloWorld0.xml");

            using (var unitWork = UnitOfWork.Start())
            {
                ProcessDefinitionCreationContext creationContext = new ProcessDefinitionCreationContext();
                ProcessDefinition processDefinition = creationContext.CreateProcessDefinition(xmlDocument);
                creationContext.ResolveReferences();

                var processBlockRepository = new EFRepository<ProcessBlock>();
                processBlockRepository.Add(processDefinition);
                unitWork.Flush();
            }
            using (var unitWork = UnitOfWork.Start())
            { 
                var processDefinitionRepository = new EFRepository<ProcessDefinition>();
                ProcessDefinition processDefinition = processDefinitionRepository.With(w=>w.Nodes).First();
                ExecutionContext executionContext = new ExecutionContext(new User("ae"),processDefinition);
                ProcessInstance processInstance = executionContext.StartProcess();

                var processInstanceRepository = new EFRepository<ProcessInstance>();
                processInstanceRepository.Add(processInstance);
                unitWork.Flush();
            }
        }

        [TestMethod]
        public void HelloWorld1()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Definitions\\HelloWorld1.xml");

            //using (var unitWork = UnitOfWork.Start())
            //{
            //    ProcessDefinitionCreationContext creationContext = new ProcessDefinitionCreationContext();
            //    ProcessDefinition processDefinition = creationContext.CreateProcessDefinition(xmlDocument);
            //    creationContext.ResolveReferences();

            //    var processBlockRepository = new EFRepository<ProcessBlock>();
            //    processBlockRepository.Add(processDefinition);
            //    unitWork.Flush();
            //}
            using (var unitWork = UnitOfWork.Start())
            {
                var processDefinitionRepository = new EFRepository<ProcessDefinition>();
                ProcessDefinition processDefinition = processDefinitionRepository.With(w => w.Nodes).First();
                ExecutionContext executionContext = new ExecutionContext(new User("ae"),processDefinition);
                ProcessInstance processInstance = executionContext.StartProcess();

                var processInstanceRepository = new EFRepository<ProcessInstance>();
                processInstanceRepository.Add(processInstance);
                unitWork.Flush();
            }
            //using (var unitWork = UnitOfWork.Start())
            //{
            //    var processDefinitionRepository = new EFRepository<ProcessDefinition>();
            //    ProcessDefinition processDefinition = processDefinitionRepository.First();
            //    var processInstanceRepository = new EFRepository<ProcessInstance>();
            //    ProcessInstance processInstance = processInstanceRepository.With(w => w.RootFlow).Get(Guid.Parse("893D8212-B3CD-452C-B1D6-08D4391B7662"));
            //    ExecutionContext executionContext = new ExecutionContext(new User("ae"),processDefinition, processInstance, processInstance.RootFlow);

            //    executionContext.PerformActivity();
            //    processInstanceRepository.Save(processInstance);
            //    unitWork.Flush();
            //}
        }

        [TestMethod]
        public void HelloWorld2()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Definitions\\HelloWorld2.xml");

            //using (var unitWork = UnitOfWork.Start())
            //{
            //    ProcessDefinitionCreationContext creationContext = new ProcessDefinitionCreationContext();
            //    ProcessDefinition processDefinition = creationContext.CreateProcessDefinition(xmlDocument);
            //    creationContext.ResolveReferences();

            //    var processBlockRepository = new EFRepository<ProcessBlock>();
            //    processBlockRepository.Add(processDefinition);
            //    unitWork.Flush();
            //}

            //using (var unitWork = UnitOfWork.Start())
            //{
            //    var processDefinitionRepository = new EFRepository<ProcessDefinition>();
            //    ProcessDefinition processDefinition = 
            //        processDefinitionRepository.With(w => w.Nodes).With(w=>w.Attributes).First();
            //    ProcessInstance processInstance = new ProcessInstance(processDefinition);
            //    ExecutionContext executionContext = new ExecutionContext(processDefinition, processInstance);
            //    executionContext.StartProcess();

            //    var processInstanceRepository = new EFRepository<ProcessInstance>();
            //    processInstanceRepository.Add(processInstance);
            //    unitWork.Flush();
            //}

            using (var unitWork = UnitOfWork.Start())
            {
                var processDefinitionRepository = new EFRepository<ProcessDefinition>();
                ProcessDefinition processDefinition = processDefinitionRepository.First();
                var processInstanceRepository = new EFRepository<ProcessInstance>();
                ProcessInstance processInstance = processInstanceRepository.With(w => w.RootFlow).First();

                IRepository<Flow> flowRepository = new EFRepository<Flow>();
                var rootFlow = 
                    flowRepository.With(w => w.Node)
                    .With(w => w.AttributeInstances)
                    .Single(q => q.ProcessInstanceId == processInstance.Id);
                ExecutionContext executionContext = new ExecutionContext(new User("ae"),processDefinition, processInstance, rootFlow);

                IDictionary<string, object> attributeValues = new Dictionary<string, object>();
                attributeValues.Add("the text attrib", ":-(");
                executionContext.PerformActivity(attributeValues);
                processInstanceRepository.Save(processInstance);
                unitWork.Flush();
            }
        }

        [TestMethod]
        public void HelloWorld3_approve()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Definitions\\HelloWorld3.xml");

            //using (var unitWork = UnitOfWork.Start())
            //{
            //    ProcessDefinitionCreationContext creationContext = new ProcessDefinitionCreationContext();
            //    ProcessDefinition processDefinition = creationContext.CreateProcessDefinition(xmlDocument);
            //    creationContext.ResolveReferences();

            //    var processBlockRepository = new EFRepository<ProcessBlock>();
            //    processBlockRepository.Add(processDefinition);
            //    unitWork.Flush();
            //}

            //using (var unitWork = UnitOfWork.Start())
            //{
            //    var processDefinitionRepository = new EFRepository<ProcessDefinition>();
            //    ProcessDefinition processDefinition =
            //        processDefinitionRepository.With(w => w.Nodes).With(w => w.Attributes).First();
            //    ProcessInstance processInstance = new ProcessInstance(processDefinition);
            //    ExecutionContext executionContext = new ExecutionContext(processDefinition, processInstance);
            //    executionContext.StartProcess();

            //    var processInstanceRepository = new EFRepository<ProcessInstance>();
            //    processInstanceRepository.Add(processInstance);
            //    unitWork.Flush();
            //}

            using (var unitWork = UnitOfWork.Start())
            {
                var processDefinitionRepository = new EFRepository<ProcessDefinition>();
                ProcessDefinition processDefinition = processDefinitionRepository.First();
                var processInstanceRepository = new EFRepository<ProcessInstance>();
                ProcessInstance processInstance = processInstanceRepository.With(w => w.RootFlow).First();

                IRepository<Flow> flowRepository = new EFRepository<Flow>();
                var rootFlow =
                    flowRepository.With(w => w.Node)
                    .With(w => w.AttributeInstances)
                    .Single(s => s.Id == processInstance.RootFlow.Id);
                ExecutionContext executionContext = new ExecutionContext(new User("ae"),processDefinition, processInstance, rootFlow);

                IDictionary<string, object> attributeValues = new Dictionary<string, object>();
                attributeValues.Add("evaluation result", "approve");
                executionContext.PerformActivity(attributeValues);
                processInstanceRepository.Save(processInstance);
                unitWork.Flush();
            }
        }

        [TestMethod]
        public void Holiday()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Definitions\\Holiday.xml");

            //using (var unitWork = UnitOfWork.Start())
            //{
            //    ProcessDefinitionCreationContext creationContext = new ProcessDefinitionCreationContext();
            //    ProcessDefinition processDefinition = creationContext.CreateProcessDefinition(xmlDocument);
            //    creationContext.ResolveReferences();

            //    var processBlockRepository = new EFRepository<ProcessBlock>();
            //    processBlockRepository.Add(processDefinition);
            //    unitWork.Flush();
            //}

            //using (var unitWork = UnitOfWork.Start())
            //{
            //    var processDefinitionRepository = new EFRepository<ProcessDefinition>();
            //    ProcessDefinition processDefinition =
            //        processDefinitionRepository.With(w => w.Nodes).With(w => w.Attributes).First();
            //    ProcessInstance processInstance = new ProcessInstance(processDefinition);
            //    ExecutionContext executionContext = new ExecutionContext(processDefinition, processInstance);

            //    IDictionary<string, object> attributeValues = new Dictionary<string, object>();
            //    attributeValues.Add("requester", "hugo");
            //    attributeValues.Add("start date", DateTime.Today);
            //    attributeValues.Add("end dat", DateTime.Today);
            //    executionContext.StartProcess();

            //    var processInstanceRepository = new EFRepository<ProcessInstance>();
            //    processInstanceRepository.Add(processInstance);
            //    unitWork.Flush();
            //}

            //進入evaluation，分成兩個分支
            //using (var unitWork = UnitOfWork.Start())
            //{
            //    var processDefinitionRepository = new EFRepository<ProcessDefinition>();
            //    ProcessDefinition processDefinition = processDefinitionRepository.First();
            //    var processInstanceRepository = new EFRepository<ProcessInstance>();
            //    ProcessInstance processInstance = processInstanceRepository.With(w => w.RootFlow).First();

            //    IRepository<Flow> flowRepository = new EFRepository<Flow>();
            //    var rootFlow =
            //        flowRepository.With(w => w.Node)
            //        .With(w => w.AttributeInstances)
            //        .With<AttributeInstance>(w => w.Attribute)
            //        .With(w =>w.Children)
            //        .Single(s => s.Id == processInstance.RootFlow.Id);
            //    ExecutionContext executionContext = new ExecutionContext(processDefinition, processInstance, rootFlow);

            //    IDictionary<string, object> attributeValues = new Dictionary<string, object>();
            //    attributeValues.Add("evaluation result", "approve");
            //    executionContext.PerformActivity(attributeValues);
            //    processInstanceRepository.Save(processInstance);
            //    unitWork.Flush();
            //}

            //HR notification -> Join
            //using (var unitWork = UnitOfWork.Start())
            //{
            //    var processDefinitionRepository = new EFRepository<ProcessDefinition>();
            //    ProcessDefinition processDefinition = processDefinitionRepository.First();
            //    var processInstanceRepository = new EFRepository<ProcessInstance>();
            //    ProcessInstance processInstance = processInstanceRepository.First();

            //    IRepository<Flow> flowRepository = new EFRepository<Flow>();
            //    var hrFlow =
            //        flowRepository.With(w => w.Node)
            //        .With(w => w.AttributeInstances)
            //        .With<AttributeInstance>(w => w.Attribute)
            //        .With(w => w.Children)
            //        .With(w => w.Parent)
            //        .Single(s => s.Name == "hr");
            //    ExecutionContext executionContext = new ExecutionContext(processDefinition, processInstance, hrFlow);

            //    executionContext.PerformActivity();
            //    processInstanceRepository.Save(processInstance);
            //    unitWork.Flush();
            //}

            //approval notification -> Join
            using (var unitWork = UnitOfWork.Start())
            {
                var processDefinitionRepository = new EFRepository<ProcessDefinition>();
                ProcessDefinition processDefinition = processDefinitionRepository.First();
                var processInstanceRepository = new EFRepository<ProcessInstance>();
                ProcessInstance processInstance = processInstanceRepository.First();

                IRepository<Flow> flowRepository = new EFRepository<Flow>();
                var requesterFlow =
                    flowRepository.With(w => w.Node)
                    .With(w => w.AttributeInstances)
                    .With(w => w.Children)
                    .With(w => w.Parent)
                    .Single(s => s.Name == "requester");
                ExecutionContext executionContext = new ExecutionContext(new User("ae"),processDefinition, processInstance, requesterFlow);

                executionContext.PerformActivity();
                processInstanceRepository.Save(processInstance);
                unitWork.Flush();
            }
        }
    }
}
