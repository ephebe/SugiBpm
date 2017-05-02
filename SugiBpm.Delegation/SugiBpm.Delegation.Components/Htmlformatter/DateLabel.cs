using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components
{
    public class DateLabel : IHtmlFormatter
    {
        private IDictionary<string, object> configuration = null;
        public string ObjectToHtml(object valueObject, string parameterName, object request)
        {
            System.String html = null;

            if (valueObject != null)
            {
                String dateFormat = (string)configuration["dateFormat"];
                html = ((DateTime)valueObject).ToString(dateFormat) + " (" + dateFormat + ")";
            }
            else
            {
                html = "";
            }

            return html;
        }

        public object ParseHttpParameter(string text, object request)
        {
            return null;
        }

        public void SetConfiguration(IDictionary<string, object> configuration)
        {
            this.configuration = configuration;
        }
    }
}
