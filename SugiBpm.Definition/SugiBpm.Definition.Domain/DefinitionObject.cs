using Microsoft.Practices.ServiceLocation;
using SunStone.Data;
using SunStone.Util;
using System;
using System.Xml;
namespace SugiBpm.Definition.Domain
{
    public class DefinitionObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Id { get; protected set; }
        public ProcessDefinition ProcessDefinition { get; set; }

        protected ProcessDefinitionCreationContext creationContext;
        public DefinitionObject()
        {
        }

        public DefinitionObject(ProcessDefinitionCreationContext creationContext)
        {
            this.Id = SequentialGuid.NewGuid();
            this.creationContext = creationContext;
        }

        public virtual void ReadProcessData(XmlElement xmlElement) 
        {
            Name = xmlElement.GetProperty("name");
            Description = xmlElement.GetProperty("description");
            this.ProcessDefinition = creationContext.ProcessDefinition;

            var actionRepository = ServiceLocator.Current.GetInstance<IRepository<ActionDef>>();

            foreach (XmlElement actionElement in xmlElement.GetChildElements("action"))
            {
                creationContext.DefinitionObject = this;
                ActionDef action = new ActionDef(creationContext);
                action.ReadProcessData(actionElement);
                actionRepository.Add(action);
                creationContext.DefinitionObject = null;
            }
        }

        public virtual ProcessBlock RegisteredScope()
        {
            return creationContext.ProcessBlock;
        }

        public override string ToString()
        {
            string className = this.GetType().FullName;
         
            return className.Substring(0,className.IndexOf("_")) + "[" + Id + "|" + Name + "]";
        }
    }
}
