using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components
{
    public class TextAreaInput : IHtmlFormatter
    {
        private IDictionary<string, object> configuration = null;
        public string ObjectToHtml(object valueObject, string parameterName, object request)
        {
            string html = null;

            string cols = (string)configuration["cols"];
            string rows = (string)configuration["rows"];

            html = "<textarea rows=" + rows + " cols=" + cols + " name=\"" + parameterName + "\">";

            if (valueObject != null)
            {
                html += valueObject.ToString();
            }

            html += "</textarea>";

            //log.Debug("generated text area control : " + html);

            return html;
        }

        public Object ParseHttpParameter(string text, object request)
        {
            return text;
        }

        public void SetConfiguration(IDictionary<string, object> configuration)
        {
            this.configuration = configuration;
        }
    }
}
