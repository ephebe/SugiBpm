using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public interface IHtmlFormatter : IConfigurable
    {
        string ObjectToHtml(object valueObject, string parameterName, object request);

        object ParseHttpParameter(string text, object request);
    }
}
