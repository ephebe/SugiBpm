using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SugiBpm.Delegation.Interface;
using SugiBpm.Execution.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.IntegrationTest.Holiday
{
    [TestClass]
    public class HolidayTest : BaseTest
    {
        private Guid actorId = Guid.Parse("8919274F-0DBB-462A-86EE-08D49107DBA0");
        private Guid bossId = Guid.Parse("8A0B2072-C6CD-45BF-86EF-08D49107DBA0");
        private Guid hrId = Guid.Parse("8F24514F-E049-46D6-86F0-08D49107DBA0");
        private Guid processDefinitionId = Guid.Parse("CA3F486C-B2A5-41D0-6E8B-08D49107FF86");

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
            var flows = executionApplication.GetTaskList(bossId);

            Assert.AreEqual(1, flows.Count);

            IDictionary<string, object> attributeValues = new Dictionary<string, object>();
            attributeValues.Add("evaluation result", Evaluation.APPROVE);
            executionApplication.PerformActivity(actorId, flows[0].Id, attributeValues);

            flows = executionApplication.GetTaskList(actorId);
            Assert.AreEqual(1, flows.Count);

            flows = executionApplication.GetTaskList(hrId);
            Assert.AreEqual(1, flows.Count);
        }

        [TestMethod]
        public void fork1Process()
        {
            var executionApplication = ServiceLocator.Current.GetInstance<IExecutionApplication>();
            var flows = executionApplication.GetTaskList(actorId);

            executionApplication.PerformActivity(actorId, flows[0].Id);

            flows = executionApplication.GetTaskList(actorId);
            Assert.AreEqual(0, flows.Count);
        }

        [TestMethod]
        public void fork2Process()
        {
            var executionApplication = ServiceLocator.Current.GetInstance<IExecutionApplication>();
            var flows = executionApplication.GetTaskList(hrId);

            executionApplication.PerformActivity(actorId, flows[0].Id);

            flows = executionApplication.GetTaskList(hrId);
            Assert.AreEqual(0, flows.Count);
        }

        [TestMethod]
        public void getStartForm()
        {
            var executionApplication = ServiceLocator.Current.GetInstance<IExecutionApplication>();
            var activityForm = executionApplication.GetStartForm(Guid.Parse("6BE76E6C-7048-4657-B01F-08D46A7F9A8F"));

            Assert.IsNotNull(activityForm);
        }

    }
}
