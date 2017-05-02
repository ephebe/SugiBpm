using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components
{
    public class AndJoin : IJoinHandler
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(AndJoin));

        public bool Join(IJoinContext joinContext)
        {
            bool reactivateParent = false;

            if (joinContext.GetOtherActiveConcurrentFlows().Count == 0)
            {
                reactivateParent = true;
            }

            //log.Debug("flow " + joinContext.GetFlow().Name + " is joining : " + reactivateParent);

            return reactivateParent;
        }
    }
}
