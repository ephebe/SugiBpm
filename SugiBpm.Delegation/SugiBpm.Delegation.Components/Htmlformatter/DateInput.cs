using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components
{
    public class DateInput : IHtmlFormatter
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(DateInput));
        private static readonly CultureInfo enUS = new CultureInfo("en-US", false);
        private IDictionary<string, object> configuration = null;

        public string ObjectToHtml(object valueObject, string parameterName, object request)
        {
            string html = null;
            string dateFormat = (string)configuration["dateFormat"];
            //log.Debug("dateformat: " + dateFormat);

            html = "<input type=text size=11 name=\"" + parameterName + "\"";

            if (valueObject != null)
            {
                html += (" value=\"" + ((DateTime)valueObject).ToString(dateFormat, enUS) + "\"");
            }

            html += ("> (" + dateFormat + ")");

            return html;
        }

        public Object ParseHttpParameter(string text, object request)
        {
            string dateFormat = (string)configuration["dateFormat"];
            //log.Debug("dateformat: " + dateFormat);
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            return DateTime.ParseExact(text, dateFormat, enUS);
        }

        public void SetConfiguration(IDictionary<string, object> configuration)
        {
            this.configuration = configuration;
        }
    }
}
