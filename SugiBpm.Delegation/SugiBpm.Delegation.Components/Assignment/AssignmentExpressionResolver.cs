using Microsoft.Practices.ServiceLocation;
using SugiBpm.Delegation.Interface;
using SugiBpm.Delegation.Interface.Organization;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components.Assignment
{
    [Export(typeof(IAssignmentHandler))]
    [ExportMetadata("ClassName", "AssignmentExpressionResolver")]
    public class AssignmentExpressionResolver : IAssignmentHandler
    {
        private ActorExpressionResolver actorExpressionResolver = new ActorExpressionResolver();
        //private static readonly ILog log = LogManager.GetLogger(typeof(AssignmentExpressionResolver));

        public IActor SelectActor(IAssignmentContext assignmentContext)
        {
            IActor actor = null;

            string expression = (string)assignmentContext.Configuration["expression"];

            try
            {
                actor = actorExpressionResolver.ResolveArgument(expression, assignmentContext);
            }
            catch (Exception e)
            {
                //log.Error("error selecting an actor :", e);
                throw new SystemException(e.Message);
            }

            return actor;
        }
    }
}
