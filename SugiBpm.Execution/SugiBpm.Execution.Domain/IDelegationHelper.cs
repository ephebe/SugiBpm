using SugiBpm.Definition.Domain;
using SugiBpm.Delegation.Interface;
using SugiBpm.Delegation.Interface.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Domain
{
    public interface IDelegationHelper
    {
        void DelegateAction(DelegationDef delegation, ExecutionContext executionContext);
        Transition DelegateDecision(DelegationDef delegation, ExecutionContext executionContext);
        void DelegateFork(DelegationDef delegation, ExecutionContext executionContext);
        bool DelegateJoin(DelegationDef delegation, ExecutionContext executionContext);
        ISerializer DelegateSerializer(DelegationDef delegation);
        IActor DelegateAssignment(DelegationDef delegation, ExecutionContext executionContext);
    }
}
