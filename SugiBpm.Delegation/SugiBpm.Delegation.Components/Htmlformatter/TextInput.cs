using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components
{
    public class TextInput : IHtmlFormatter
    {
        private IDictionary<string, object> configuration = null;
        public string ObjectToHtml(object valueObject, string parameterName, object request)
        {
            string html = null;
            string size = "10";

            if (configuration.ContainsKey("size"))
            {
                size = ((string)configuration["size"]);
            }

            string text = "";
            if (valueObject != null)
                text = ((string)valueObject);

            html = "<input type=text size=" + size + " name=\"" + parameterName + "\" value=\"" + text + "\">";

            return html;
        }

        public object ParseHttpParameter(string text, object request)
        {
            return text;
        }

        public void SetConfiguration(IDictionary<string, object> configuration)
        {
            this.configuration = configuration;
        }
    }
}
