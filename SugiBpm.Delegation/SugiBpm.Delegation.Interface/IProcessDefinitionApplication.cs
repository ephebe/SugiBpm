using SugiBpm.Delegation.Interface.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public interface IProcessDefinitionApplication
    {
        void DeployProcessArchive(string path);

        IProcessDefinition GetProcessDefinition(Guid processDefinitionId);
        IList<IProcessDefinition> GetProcessDefinitions();
    }
}
