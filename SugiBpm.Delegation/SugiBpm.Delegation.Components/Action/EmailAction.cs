using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components
{
    [Export(typeof(IActionHandler))]
    [ExportMetadata("ClassName", "Email")]
    public class EmailAction : IActionHandler
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(EmailAction));
        private AttributeExpressionResolver _attributeExpressionResolver;
        //private static readonly ActorExpressionResolver _actorExpressionResolver;

        public EmailAction(AttributeExpressionResolver attributeExpressionResolver)
        {
            _attributeExpressionResolver = attributeExpressionResolver;
        }

        public void Run(IActionContext actionContext)
        {
            var configuration = actionContext.Configuration;
            string subject = (string)configuration["subject"];
            string message = (string)configuration["message"];
            string from = (string)configuration["from"];
            string to = (string)configuration["to"];

            // resolving the texts
            subject = _attributeExpressionResolver.ResolveAttributeExpression(subject, actionContext);
            message = _attributeExpressionResolver.ResolveAttributeExpression(message, actionContext);
            //IUser user = (IUser)_actorExpressionResolver.ResolveArgument(to, actionContext);
            //to = user.Email;
            if (string.IsNullOrEmpty(from))
            {
                from = actionContext.GetProcessDefinition().Name;
                from = from.ToLower();
                from = from.Replace(' ', '.');
                from += "@netbpm.org";
            }

            SendMail(from, to, subject, message, actionContext);
        }

        public void SendMail(String from, String to, String subject, String body, IActionContext interactionContext)
        {
            //log.Info("sending mail from '+ from +'to '" + to + "' with subject '" + subject + "' and body '" + body + "'");

        }
    }
}
