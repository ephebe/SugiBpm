using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SugiBpm.Delegation.Interface;
using SugiBpm.Execution.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.IntegrationTest.HelloWorld1
{
    [TestClass]
    public class HelloWorld1Test : BaseTest
    {
        private Guid actorId = Guid.Parse("F9EC5B98-0E98-491F-A5BC-08D457C1C5A8");
        private Guid processDefinitionId = Guid.Parse("5AE0881A-808E-43DF-569D-08D457C22FEA");

        [TestMethod]
        public void StartProcess()
        {
            var executionApplication = ServiceLocator.Current.GetInstance<IExecutionApplication>();
            var processInstance = executionApplication.StartProcessInstance(actorId, processDefinitionId);

            Assert.IsNotNull(processInstance);
        }

        [TestMethod]
        public void PerformProcess()
        {
            var executionApplication = ServiceLocator.Current.GetInstance<IExecutionApplication>();
            var flows = executionApplication.GetTaskList(actorId);

            Assert.AreEqual(1, flows.Count);

            executionApplication.PerformActivity(actorId, flows[0].Id);

            flows = executionApplication.GetTaskList(actorId);
            Assert.AreEqual(0, flows.Count);
        }


    }
}
