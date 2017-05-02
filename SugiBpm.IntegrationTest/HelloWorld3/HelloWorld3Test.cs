using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SugiBpm.Delegation.Interface;
using SugiBpm.Execution.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.IntegrationTest.HelloWorld3
{
    [TestClass]
    public class HelloWorld3Test : BaseTest
    {
        private Guid actorId = Guid.Parse("A9746084-A8FC-459C-65F5-08D45A3DE461");
        private Guid processDefinitionId = Guid.Parse("D1D89710-339E-4CFF-9FFE-08D45A3DF5E3");

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

            IDictionary<string, object> attributeValues = new Dictionary<string, object>();
            attributeValues.Add("evaluation result", "approve");
            executionApplication.PerformActivity(actorId, flows[0].Id, attributeValues);

            flows = executionApplication.GetTaskList(actorId);
            Assert.AreEqual(0, flows.Count);
        }
    }
}
