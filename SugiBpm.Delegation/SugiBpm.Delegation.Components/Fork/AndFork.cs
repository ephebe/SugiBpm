using SugiBpm.Delegation.Interface;
using SugiBpm.Delegation.Interface.Definition;
using SugiBpm.Delegation.Interface.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components
{
    public class AndFork : IForkHandler
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(AndFork));

        public void Fork(IForkContext forkContext)
        {
            //log.Debug("starting to fork...");

            IFork fork = (IFork)forkContext.GetNode();
            foreach (ITransition transition in fork.LeavingTransitions)
            {
                //log.Debug("forking flow for transition " + leavingTransition.Name);
                forkContext.ForkFlow(transition.Name);
            }
        }
    }
}
