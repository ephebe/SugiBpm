using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using SugiBpm.Definition.Domain;
using System.Text.RegularExpressions;
using SunStone.EntityFramework;
using SunStone.Data;
using System.Linq;

namespace SugiBpm.Definition.Test
{
    [TestClass]
    public class ReadXmlTest : BaseTest
    {
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

                Assert.AreEqual("Hello world 0", processDefinition.Name);
                Assert.AreEqual("This is the simplest process.", processDefinition.Description);
                Assert.AreEqual(2, processDefinition.Nodes.Count);
                Node start = null, end = null;
                foreach (Node node in processDefinition.Nodes)
                {
                    if (node.Name == "start")
                        start = node;
                    else if (node.Name == "end")
                        end = node;
                }
                Assert.IsNotNull(start);
                Assert.AreEqual(1, start.LeavingTransitions.Count);

                foreach (Transition transition in start.LeavingTransitions)
                {
                    Assert.AreEqual(start, transition.From);
                    Assert.AreEqual(end, transition.To);
                }

                var processBlockRepository = new EFRepository<ProcessBlock>();
                processBlockRepository.Add(processDefinition);
                unitWork.Flush();
            }
        }

        [TestMethod]
        public void HelloWorld1()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Definitions\\HelloWorld1.xml");

            using (var unitWork = UnitOfWork.Start())
            {
                ProcessDefinitionCreationContext creationContext = new ProcessDefinitionCreationContext();
                ProcessDefinition processDefinition = creationContext.CreateProcessDefinition(xmlDocument);
                creationContext.ResolveReferences();

                Assert.AreEqual("Hello world 1", processDefinition.Name);
                Assert.AreEqual("This is the simples process.", processDefinition.Description);
                Assert.AreEqual(3, processDefinition.Nodes.Count);
                Node start = null, end = null;
                ActivityState activityState = null;
                foreach (Node node in processDefinition.Nodes)
                {
                    if (node.Name == "start")
                        start = node;
                    else if (node.Name == "first activity state")
                        activityState = node as ActivityState;
                    else if (node.Name == "end")
                        end = node;
                }
                Assert.IsNotNull(start);
                Assert.AreEqual(1, start.LeavingTransitions.Count);
                foreach (Transition transition in start.LeavingTransitions)
                {
                    Assert.AreEqual(start, transition.From);
                    Assert.AreEqual(activityState, transition.To);
                }

                Assert.IsNotNull(activityState);
                Assert.AreEqual("this is the first state", activityState.Description);
                Assert.IsNotNull(activityState.AssignmentDelegation);

                Assert.AreEqual(1, activityState.LeavingTransitions.Count);
                foreach (Transition transition in activityState.LeavingTransitions)
                {
                    Assert.AreEqual(activityState, transition.From);
                    Assert.AreEqual(end, transition.To);
                }

                var processBlockRepository = new EFRepository<ProcessBlock>();
                processBlockRepository.Add(processDefinition);
                unitWork.Flush();
            }
        }

        [TestMethod]
        public void HelloWorld2()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Definitions\\HelloWorld2.xml");

            using (var unitWork = UnitOfWork.Start())
            {
                ProcessDefinitionCreationContext creationContext = new ProcessDefinitionCreationContext();
                ProcessDefinition processDefinition = creationContext.CreateProcessDefinition(xmlDocument);
                creationContext.ResolveReferences();

                Node start = null, end = null;
                ActivityState activityState = null;
                foreach (Node node in processDefinition.Nodes)
                {
                    if (node.Name == "start")
                        start = node;
                    else if (node.Name == "first activity state")
                        activityState = node as ActivityState;
                    else if (node.Name == "end")
                        end = node;
                }
                Assert.IsNotNull(start);
                Assert.IsNotNull(activityState);
                Assert.IsNotNull(activityState.AssignmentDelegation);

                DelegationDef delegationDef = activityState.AssignmentDelegation;
                Assert.AreEqual("ActorAssignment", delegationDef.ClassName);

                Assert.AreEqual("<cfg><parametername=\"expression\">processInitiator</parameter></cfg>", Regex.Replace(delegationDef.Configuration, @"\s|\t|\n|\r", ""));

                var processBlockRepository = new EFRepository<ProcessBlock>();
                processBlockRepository.Add(processDefinition);
                unitWork.Flush();
            }
        }

        [TestMethod]
        public void HelloWorld3()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Definitions\\HelloWorld3.xml");

            using (var unitWork = UnitOfWork.Start())
            {
                ProcessDefinitionCreationContext creationContext = new ProcessDefinitionCreationContext();
                ProcessDefinition processDefinition = creationContext.CreateProcessDefinition(xmlDocument);
                creationContext.ResolveReferences();

                AttributeDef evaluation = null, text = null;
                foreach (AttributeDef attribute in processDefinition.Attributes)
                {
                    if (attribute.Name == "evaluation result")
                        evaluation = attribute;
                    else if (attribute.Name == "the text attrib")
                        text = attribute;
                }

                Assert.IsNotNull(evaluation);
                Assert.IsNotNull(evaluation.SerializerDelegation);
                Assert.AreEqual("EvaluationSerializer", evaluation.SerializerDelegation.ClassName);

                Assert.IsNotNull(text);
                Assert.IsNotNull(text.SerializerDelegation);
                Assert.AreEqual("TextSerializer", text.SerializerDelegation.ClassName);
                Assert.AreEqual(":-)", text.InitialValue);

                var processBlockRepository = new EFRepository<ProcessBlock>();
                processBlockRepository.Add(processDefinition);
                unitWork.Flush();
            }
        }

        [TestMethod]
        public void HolidayTest()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Definitions\\Holiday.xml");

            using (var unitWork = UnitOfWork.Start())
            {
                ProcessDefinitionCreationContext creationContext = new ProcessDefinitionCreationContext();
                ProcessDefinition processDefinition = creationContext.CreateProcessDefinition(xmlDocument);
                creationContext.ResolveReferences();

                Assert.AreEqual(1, processDefinition.ChildBlocks.Count);
                ProcessBlock concurrentProcessBlock = processDefinition.ChildBlocks.First();

                Node fork = null, join = null;
                ActivityState hrNotify = null, approvalNotify = null;
                foreach (Node node in concurrentProcessBlock.Nodes)
                {
                    if (node.Name == "approved holiday fork")
                        fork = node;
                    else if (node.Name == "join before finish")
                        join = node;
                    else if (node.Name == "HR notification")
                        hrNotify = node as ActivityState;
                    else if (node.Name == "approval notification")
                        approvalNotify = node as ActivityState;
                }

                Assert.IsNotNull(fork);
                Assert.IsNotNull(join);
                Assert.IsNotNull(hrNotify);
                Assert.IsNotNull(approvalNotify);
                Assert.IsNotNull(fork.LeavingTransitions.First().To);

                var processBlockRepository = new EFRepository<ProcessBlock>();
                processBlockRepository.Add(processDefinition);
                unitWork.Flush();
            }
        }
    }
}
