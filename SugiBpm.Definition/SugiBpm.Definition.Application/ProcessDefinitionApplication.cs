using System;
using System.Collections;
using System.Xml;
using SugiBpm.Definition.Domain;
using SunStone.Data;
using Microsoft.Practices.ServiceLocation;
using SugiBpm.Delegation.Interface.Definition;
using SugiBpm.Delegation.Interface;
using System.Collections.Generic;
using System.Linq;

namespace SugiBpm.Definition.Application
{
    public class ProcessDefinitionApplication : IProcessDefinitionApplication
    {
        public ProcessDefinitionApplication()
        {
        }

        public bool ValidateConfig(string path,out XmlDocument xmlDocument,out IDictionary error)
        {
            throw new NotImplementedException();
        }

        public void DeployProcessArchive(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            using (var scope = new UnitOfWorkScope())
            {
                ProcessDefinitionCreationContext creationContext = new ProcessDefinitionCreationContext();
                ProcessDefinition processDefinition = creationContext.CreateProcessDefinition(xmlDocument);
                creationContext.ResolveReferences();

                var processBlockRepository = ServiceLocator.Current.GetInstance<IRepository<ProcessBlock>>();
                processBlockRepository.Add(processDefinition);

                scope.Commit();
            }
        }

        public IProcessDefinition GetProcessDefinition(Guid processDefinitionId)
        {
            using (var scope = new UnitOfWorkScope())
            {
                var processDefinitionRepository = ServiceLocator.Current.GetInstance<IRepository<ProcessDefinition>>();
                var processDefinition =  processDefinitionRepository.Get(processDefinitionId);
                return processDefinition;
            }
        }

        public IList<IProcessDefinition> GetProcessDefinitions()
        {
            using (var scope = new UnitOfWorkScope())
            {
                var processDefinitionRepository = ServiceLocator.Current.GetInstance<IRepository<ProcessDefinition>>();
                var processDefinitions = processDefinitionRepository.ToList();
                return processDefinitions.ConvertAll(s=>s as IProcessDefinition);
            }
        }
    }
}
