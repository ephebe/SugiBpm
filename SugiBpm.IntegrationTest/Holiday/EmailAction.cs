using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.IntegrationTest.Holiday
{
    [Export(typeof(IActionHandler))]
    [ExportMetadata("ClassName", "EmailAction")]
    public class EmailAction : IActionHandler
    {
        public void Run(IActionContext actionContext)
        {
            using (var stream = System.IO.File.CreateText("emailAction.txt"))
            {
                stream.WriteLine("Email Send!!");
                stream.Close();
            }
        }
    }
}
