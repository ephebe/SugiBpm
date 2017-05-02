using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SugiBpm.Delegation.Domain;
using SugiBpm.Definition.Domain;
using SugiBpm.Execution.Domain;
using SugiBpm.Delegation.Interface.Definition;
using Autofac.Extras.FakeItEasy;
using Microsoft.Practices.ServiceLocation;
using SunStone.Data;
using System.Collections.Generic;
using FakeItEasy;
using SugiBpm.Delagation.Test;
using SugiBpm.Delegation.Interface;

namespace SugiBpm.Delegation.Test
{
    [TestClass]
    public class DelegationHelperTest
    {
        [TestMethod]
        public void TestDelegateAction()
        {
            DelegationHelper delegationHelper = new DelegationHelper();

            DelegationDef delegationDef = new DelegationDef();
            delegationDef.ClassName = "ActionTest";
            delegationDef.Configuration = "<cfg>" +
            "<parameter name = \"to\" > previousActor </parameter>" +
            "<parameter name = \"subject\" > you requested a holiday </parameter>" +
            "<parameter name = \"message\" > you requested a holiday from ${ start date}" +
            "to ${ end date}" +
            "with comment ${ comment}</parameter>" +
            "</cfg> ";
            ExecutionContext context = new ExecutionContext(null,null, null);
            delegationHelper.DelegateAction(delegationDef, context);

            Assert.IsTrue(context.Configuration.ContainsKey("test"));
            Assert.AreEqual("1234",context.Configuration["test"]);
        }

        [TestMethod]
        public void TestDelegateSerializer()
        {
            DelegationHelper delegationHelper = new DelegationHelper();

            DelegationDef delegationDef = new DelegationDef();
            delegationDef.ClassName = "SerializerTest";

            var serializer = delegationHelper.DelegateSerializer(delegationDef);
            Evaluation result = (Evaluation)serializer.Deserialize("approve");

            Assert.AreEqual(Evaluation.APPROVE, result);
        }

        [TestMethod]
        public void TestDelegateDecision()
        {
            using (var fake = new AutoFake())
            {
                var provider = fake.Resolve<IServiceLocator>();
                var transitionRepository = new InMemoryRepository<Transition>(new List<Transition>());
                FakeNode node = new FakeNode();
                transitionRepository.Add(new Transition() { From = node, Name = "pass" });
                A.CallTo(() => provider.GetInstance<IRepository<Transition>>()).Returns(transitionRepository);
                ServiceLocator.SetLocatorProvider(() => provider);

                DelegationHelper delegationHelper = new DelegationHelper();

                DelegationDef delegationDef = new DelegationDef();
                delegationDef.ClassName = "DecisionTest";

                ExecutionContext context = new ExecutionContext(null,null, null);
                context.Node = node;

                Assert.AreEqual("pass", delegationHelper.DelegateDecision(delegationDef, context).Name);
            }
        }

        [TestMethod]
        public void TestDelegateFork()
        {
            DelegationHelper delegationHelper = new DelegationHelper();

            DelegationDef delegationDef = new DelegationDef();
            delegationDef.ClassName = "ForkTest";

            ExecutionContext context = new ExecutionContext(null,null, null);

            delegationHelper.DelegateFork(delegationDef, context);
        }

        [TestMethod]
        public void TestDelegateJoin()
        {
            DelegationHelper delegationHelper = new DelegationHelper();

            DelegationDef delegationDef = new DelegationDef();
            delegationDef.ClassName = "JoinTest";

            ExecutionContext context = new ExecutionContext(null,null, null);

            bool reactiveFlow = delegationHelper.DelegateJoin(delegationDef, context);

            Assert.IsFalse(reactiveFlow);
        }
    }
}
