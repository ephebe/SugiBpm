using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SugiBpm.Delegation.Interface;
using SugiBpm.Execution.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.IntegrationTest.HelloWorld2
{
    [TestClass]
    public class HelloWorld2Test : BaseTest
    {
        private Guid actorId = Guid.Parse("B688E764-0072-4CC4-6FF0-08D457D2C429");
        private Guid processDefinitionId = Guid.Parse("4ED1291C-E124-4102-CD5B-08D457D2D0E2");

        [TestMethod]
        public void StartProcess()
        {
            var executionApplication = ServiceLocator.Current.GetInstance<IExecutionApplication>();

            IDictionary<string, object> attributeValues = new Dictionary<string, object>();
            attributeValues.Add("the text attrib", ":-|");
            var processInstance = executionApplication.StartProcessInstance(actorId, processDefinitionId, attributeValues);

            Assert.IsNotNull(processInstance);
        }


        [TestMethod]
        public void PerformProcessAttribute()
        {
            var executionApplication = ServiceLocator.Current.GetInstance<IExecutionApplication>();
            var flows = executionApplication.GetTaskList(actorId);

            Assert.AreEqual(1, flows.Count);

            IDictionary<string, object> attributeValues = new Dictionary<string, object>();
            attributeValues.Add("the text attrib", ":-(");
            executionApplication.PerformActivity(actorId, flows[0].Id, attributeValues);

            flows = executionApplication.GetTaskList(actorId);
            Assert.AreEqual(0, flows.Count);
        }
    }
}
