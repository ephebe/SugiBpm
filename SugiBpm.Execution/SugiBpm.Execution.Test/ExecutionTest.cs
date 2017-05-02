using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SugiBpm.Definition.Domain;
using SugiBpm.Execution.Domain;
using System.Linq;
using System.Collections.Generic;
using Autofac.Extras.FakeItEasy;
using Microsoft.Practices.ServiceLocation;
using FakeItEasy;
using SunStone.Data;
using SugiBpm.Organization.Domain;

namespace SugiBpm.Execution.Test
{
    [TestClass]
    public class ExecutionTest
    {
        [TestMethod]
        public void HelloWorld0()
        {
            ProcessDefinition processDefinition = ProcessDefinitionFactory.getHelloWorld0();
            ExecutionContext executionContext = new ExecutionContext(new User("ae"),processDefinition);
            ProcessInstance processInstance = executionContext.StartProcess();

            Assert.IsNotNull(processInstance.RootFlow);
            Assert.AreEqual(processInstance.RootFlow, executionContext.Flow);
            var endState = processDefinition.Nodes.Single(q => q is EndState);
            Assert.AreEqual(endState, executionContext.Node);
            Assert.AreEqual(endState, processInstance.RootFlow.Node);
        }

        [TestMethod]
        public void HelloWorld1()
        {
            using (var fake = new AutoFake())
            {
                var provider = fake.Resolve<IServiceLocator>();
                var actionRepository = new InMemoryRepository<ActionDef>(new List<ActionDef>());
                A.CallTo(() => provider.GetInstance<IRepository<ActionDef>>()).Returns(actionRepository);
                ServiceLocator.SetLocatorProvider(() => provider);

                ProcessDefinition processDefinition = ProcessDefinitionFactory.getHelloWorld1();
                ExecutionContext executionContext = new ExecutionContext(new User("ae"),processDefinition);
                ProcessInstance processInstance = executionContext.StartProcess();

                var activityState = processDefinition.Nodes.Single(q => q.GetType() == typeof(ActivityState));
                Assert.AreEqual(activityState, processInstance.RootFlow.Node);

                executionContext.StartProcess();
                executionContext.PerformActivity();

                var endState = processDefinition.Nodes.Single(q => q is EndState);
                Assert.AreEqual(endState, processInstance.RootFlow.Node);
            }
        }

        [TestMethod]
        public void HelloWorld2()
        {
            using (var fake = new AutoFake())
            {
                var provider = fake.Resolve<IServiceLocator>();
                var actionRepository = new InMemoryRepository<ActionDef>(new List<ActionDef>());
                A.CallTo(() => provider.GetInstance<IRepository<ActionDef>>()).Returns(actionRepository);
                ServiceLocator.SetLocatorProvider(() => provider);

                ProcessDefinition processDefinition = ProcessDefinitionFactory.getHelloWorld2();
                ExecutionContext executionContext = new ExecutionContext(new User("ae"),processDefinition);
                ProcessInstance processInstance  = executionContext.StartProcess();

                var activityState = processDefinition.Nodes.Single(q => q.GetType() == typeof(ActivityState));
                Assert.AreEqual(activityState, processInstance.RootFlow.Node);

                executionContext.StartProcess();
                Assert.IsTrue(executionContext.Flow.AttributeInstances.Count > 0);

                IDictionary<string, object> attributeValues = new Dictionary<string, object>();
                attributeValues.Add("the text attrib", ":-(");
                executionContext.PerformActivity(attributeValues);

                var endState = processDefinition.Nodes.Single(q => q is EndState);
                Assert.AreEqual(":-(", executionContext.Flow.AttributeInstances.First().ValueText);
                Assert.AreEqual(endState, processInstance.RootFlow.Node);
            }
        }

        [TestMethod]
        public void HelloWorld3_approve()
        {
            using (var fake = new AutoFake())
            {
                var provider = fake.Resolve<IServiceLocator>();

                var actionRepository = new InMemoryRepository<ActionDef>(new List<ActionDef>());
                A.CallTo(() => provider.GetInstance<IRepository<ActionDef>>()).Returns(actionRepository);
                ServiceLocator.SetLocatorProvider(() => provider);

                ProcessDefinition processDefinition = ProcessDefinitionFactory.getHelloWorld3();

                var delegationHelper = fake.Resolve<IDelegationHelper>();
                Transition transition = processDefinition.Nodes.Single(s => s is Decision).LeavingTransitions.Single(l => l.To is EndState);
                A.CallTo(delegationHelper).WithReturnType<Transition>().Returns(transition);
                A.CallTo(() => provider.GetInstance<IDelegationHelper>()).Returns(delegationHelper);

                ExecutionContext executionContext = new ExecutionContext(new User("ae"),processDefinition);

                ProcessInstance processInstance = executionContext.StartProcess();
                Assert.AreEqual(2, executionContext.Flow.AttributeInstances.Count);

                IDictionary<string, object> attributeValues = new Dictionary<string, object>();
                attributeValues.Add("evaluation result", "approve");
                executionContext.PerformActivity(attributeValues);

                var endState = processDefinition.Nodes.Single(q => q is EndState);
                Assert.AreEqual(endState, processInstance.RootFlow.Node);
            }
        }

        [TestMethod]
        public void Holiday()
        {
            using (var fake = new AutoFake())
            {
                var provider = fake.Resolve<IServiceLocator>();

                var actionRepository = new InMemoryRepository<ActionDef>(new List<ActionDef>());
                A.CallTo(() => provider.GetInstance<IRepository<ActionDef>>()).Returns(actionRepository);
                ServiceLocator.SetLocatorProvider(() => provider);

                ProcessDefinition processDefinition = ProcessDefinitionFactory.getHoliday();

                var delegationHelper = fake.Resolve<IDelegationHelper>();
                Transition transition = processDefinition.Nodes.Single(s => s is Decision).LeavingTransitions.Single(l => l.To is Fork);
                A.CallTo(delegationHelper).WithReturnType<Transition>().Returns(transition);
                A.CallTo(() => provider.GetInstance<IDelegationHelper>()).Returns(delegationHelper);

                ExecutionContext executionContext = new ExecutionContext(new User("ae"),processDefinition);

                IDictionary<string, object> attributeValues = new Dictionary<string, object>();
                attributeValues.Add("requester", "hugo");
                attributeValues.Add("start date", DateTime.Today);
                attributeValues.Add("end dat", DateTime.Today);
                //進入evaluating
                ProcessInstance processInstance = executionContext.StartProcess();
                Assert.AreEqual(7, executionContext.Flow.AttributeInstances.Count);

                //有個地方要從Node取到ProcessBlock
                var processBlockRepository = new InMemoryRepository<ProcessBlock>(new List<ProcessBlock>() { processDefinition.ChildBlocks.First() });
                A.CallTo(() => provider.GetInstance<IRepository<ProcessBlock>>()).Returns(processBlockRepository);

                //進入evaluation，分成兩個分支
                attributeValues.Clear();
                attributeValues.Add("evaluation result", "approve");
                executionContext.PerformActivity(attributeValues);

                Assert.AreEqual(2, executionContext.ForkedFlows.Count);
                Assert.AreEqual(2, processInstance.RootFlow.Children.Count);
                var childProcessBlock = processDefinition.ChildBlocks.First();
                Assert.AreEqual(childProcessBlock.Nodes.Single(s=>s.Name == "HR notification"), processInstance.RootFlow.Children.First().Node);
                Assert.AreEqual(childProcessBlock.Nodes.Single(s => s.Name == "approval notification"), processInstance.RootFlow.Children.Last().Node);

                //有個地方要取得還未完成的ChildFlow
                var childFlow1 = processInstance.RootFlow.Children.First();
                var childFlow2 = processInstance.RootFlow.Children.Last();
                var flowRepository = new InMemoryRepository<Flow>(new List<Flow>() { childFlow1, childFlow2 });
                A.CallTo(() => provider.GetInstance<IRepository<Flow>>()).Returns(flowRepository);

                //HR notification -> Join
                ExecutionContext childExecutionContext1 = new ExecutionContext(new User("ae"),processDefinition, processInstance, childFlow1);
                childExecutionContext1.PerformActivity();

                //approval notification -> Join
                ExecutionContext childExecutionContext2 = new ExecutionContext(new User("ae"),processDefinition, processInstance, childFlow2);
                childExecutionContext2.PerformActivity();

                var endState = processDefinition.Nodes.Single(q => q is EndState);
                Assert.AreEqual(endState, childExecutionContext2.ProcessInstance.RootFlow.Node);
            }
        }
    }
}
