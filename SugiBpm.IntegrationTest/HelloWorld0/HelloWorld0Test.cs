using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SugiBpm.Delegation.Interface;
using SugiBpm.Execution.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.IntegrationTest.HelloWorld0
{
    [TestClass]
    public class HelloWorld0Test : BaseTest
    {
        [TestMethod]
        public void StartProcess() 
        {
            var executionApplication = ServiceLocator.Current.GetInstance<IExecutionApplication>();
            var processInstance = executionApplication.StartProcessInstance(Guid.Parse("F9EC5B98-0E98-491F-A5BC-08D457C1C5A8"), Guid.Parse("08AA1F22-F220-4954-5699-08D457C22FEA"));

            Assert.IsNotNull(processInstance);
        }
    }
}
