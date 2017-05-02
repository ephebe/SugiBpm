using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components
{
    public class EvaluationInput : IHtmlFormatter
    {
        private IDictionary<string, object> configuration = null;
        public string ObjectToHtml(object valueObject, string parameterName, object request)
        {
            System.Text.StringBuilder htmlBuffer = new System.Text.StringBuilder();
            htmlBuffer.Append("<table border=0 cellspacing=0 cellpadding=0><tr><td nowrap style=\"background-color:transparent;\">");
            htmlBuffer.Append("<input type=radio name=\"");
            htmlBuffer.Append(parameterName);
            htmlBuffer.Append("\" value=\"");
            htmlBuffer.Append(Evaluation.APPROVE);
            htmlBuffer.Append("\">");
            htmlBuffer.Append("&nbsp; approve");
            htmlBuffer.Append("</td></tr><tr><td nowrap style=\"background-color:transparent;\">");
            htmlBuffer.Append("<input type=radio name=\"");
            htmlBuffer.Append(parameterName);
            htmlBuffer.Append("\" value=\"");
            htmlBuffer.Append(Evaluation.DISAPPROVE);
            htmlBuffer.Append("\">");
            htmlBuffer.Append("&nbsp; disapprove");
            htmlBuffer.Append("</td></tr></table>");

            return htmlBuffer.ToString();
        }

        public object ParseHttpParameter(string text, object request)
        {
            object evaluationResult = null;

            try
            {
                evaluationResult = Evaluation.ParseEvaluation(text);
            }
            catch (System.ArgumentException e)
            {
                throw new System.FormatException("couldn't parse the Evaluation from value " + text + " exception message: " + e.Message);
            }

            return evaluationResult;
        }

        public void SetConfiguration(IDictionary<string, object> configuration)
        {
            this.configuration = configuration;
        }
    }
}
