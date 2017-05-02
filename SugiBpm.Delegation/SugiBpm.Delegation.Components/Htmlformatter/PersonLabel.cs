using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components
{
    public class PersonLabel : IHtmlFormatter
    {
        public string ObjectToHtml(object valueObject, string parameterName, object request)
        {
            throw new NotImplementedException();
        }

        public object ParseHttpParameter(string text, object request)
        {
            return null;
        }


        public void SetConfiguration(IDictionary<string, object> configuration)
        {
            throw new NotImplementedException();
        }
    }
}
