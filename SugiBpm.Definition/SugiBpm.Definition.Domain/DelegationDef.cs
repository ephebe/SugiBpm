using SugiBpm.Delegation.Interface;
using SunStone.Util;
using System;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace SugiBpm.Definition.Domain
{
    public class DelegationDef
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; }
        public string Configuration { get; set; }
        public ProcessDefinition ProcessDefinition { get; set; }
        public ExceptionHandlingType ExceptionHandlingType { get; set; }

        private ProcessDefinitionCreationContext creationContext = null;
        public DelegationDef()
        {
        }

        public DelegationDef(ProcessDefinitionCreationContext creationContext)
        {
            this.Id = SequentialGuid.NewGuid();
            this.creationContext = creationContext;
        }

        public void ReadProcessData(XmlElement xmlElement)
        {
            this.ProcessDefinition = creationContext.ProcessDefinition;

            Type delegatingObjectClass = creationContext.DelegatingObject.GetType();
            if (delegatingObjectClass == typeof(AttributeDef))
            {
                string type = xmlElement.GetProperty("type");
                if (!string.IsNullOrEmpty(type))
                {
                    this.ClassName = AttributeTypes.FindAttributeTypes(type);
                }
                else
                {
                    this.ClassName = xmlElement.GetProperty("serializer");
                }
            }
            else if (delegatingObjectClass == typeof(Field))
            {
                this.ClassName = xmlElement.GetProperty("class");
            }
            else
            {
                this.ClassName = xmlElement.GetProperty("handler");
            }

            // create the configuration string
            XElement configurationXml = new XElement("cfg");
            XElement xml = new XElement("cfg");
            foreach (XmlElement childElement in xmlElement.GetChildElements("parameter"))
            {
                configurationXml.Add(new XElement("parameter",
                    new XAttribute("name",childElement.GetProperty("name")),childElement.FirstChild.Value));
            }
            Configuration = configurationXml.ToString();
        }
    }
}
