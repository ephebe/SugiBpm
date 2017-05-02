using System.Xml;
using SugiBpm.Definition.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using Microsoft.Practices.ServiceLocation;
using SunStone.Data;
using FakeItEasy;
using SugiBpm.Execution.Domain;
using SunStone.EntityFramework;

namespace SugiBpm.Execution.Test
{
    public static class ProcessDefinitionFactory
    {
        public static ProcessDefinition getHelloWorld0()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Definitions\\HelloWorld0.xml");

            ProcessDefinitionCreationContext creationContext = new ProcessDefinitionCreationContext();
            ProcessDefinition processDefinition = creationContext.CreateProcessDefinition(xmlDocument);
            creationContext.ResolveReferences();

            return processDefinition;
        }

        public static ProcessDefinition getHelloWorld1()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Definitions\\HelloWorld1.xml");

            ProcessDefinitionCreationContext creationContext = new ProcessDefinitionCreationContext();
            ProcessDefinition processDefinition = creationContext.CreateProcessDefinition(xmlDocument);
            creationContext.ResolveReferences();

            return processDefinition;
        }

        public static ProcessDefinition getHelloWorld2()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Definitions\\HelloWorld2.xml");

            ProcessDefinitionCreationContext creationContext = new ProcessDefinitionCreationContext();
            ProcessDefinition processDefinition = creationContext.CreateProcessDefinition(xmlDocument);
            creationContext.ResolveReferences();

            return processDefinition;
        }

        public static ProcessDefinition getHelloWorld3()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Definitions\\HelloWorld3.xml");

            ProcessDefinitionCreationContext creationContext = new ProcessDefinitionCreationContext();
            ProcessDefinition processDefinition = creationContext.CreateProcessDefinition(xmlDocument);
            creationContext.ResolveReferences();

            return processDefinition;
        }

        public static ProcessDefinition getHoliday()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Definitions\\Holiday.xml");

            ProcessDefinitionCreationContext creationContext = new ProcessDefinitionCreationContext();
            ProcessDefinition processDefinition = creationContext.CreateProcessDefinition(xmlDocument);
            creationContext.ResolveReferences();

            return processDefinition;
        }

        
    }
}
