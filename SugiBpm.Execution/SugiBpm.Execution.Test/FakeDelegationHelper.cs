using SugiBpm.Execution.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SugiBpm.Definition.Domain;
using SunStone.Data;
using Microsoft.Practices.ServiceLocation;
using SugiBpm.Delegation.Interface.Organization;
using SugiBpm.Delegation.Interface;

namespace SugiBpm.Execution.Test
{
    public class FakeDelegationHelper : IDelegationHelper
    {
        public void DelegateAction(DelegationDef delegation, ExecutionContext executionContext)
        {
            throw new NotImplementedException();
        }

        public IActor DelegateAssignment(DelegationDef delegation, ExecutionContext executionContext)
        {
            throw new NotImplementedException();
        }

        public Transition DelegateDecision(DelegationDef delegation, ExecutionContext executionContext)
        {
            IRepository<Transition> transitionRepository = ServiceLocator.Current.GetInstance<IRepository<Transition>>();
            //return transitionRepository.With(s => s.To).SingleOrDefault(s => s.To is EndState);
            return transitionRepository.With(s=>s.To).SingleOrDefault(s => s.To.Name == "approved holiday fork");
        }

        public void DelegateFork(DelegationDef delegation, ExecutionContext executionContext)
        {
            
        }

        public bool DelegateJoin(DelegationDef delegation, ExecutionContext executionContext)
        {
            return false;
        }

        public ISerializer DelegateSerializer(DelegationDef delegation)
        {
            throw new NotImplementedException();
        }
    }
}