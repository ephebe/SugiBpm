using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace SugiBpm.Definition.Domain
{
    public static class XmlExtension
    {
        public static XmlElement GetChildElement(this XmlElement xmlElement,string childName)
        {
            var elements = xmlElement.GetElementsByTagName(childName);

            return (XmlElement)elements.Item(0);
        }

        public static XmlNodeList GetChildElements(this XmlElement xmlElement, string childName)
        {
            return xmlElement.SelectNodes(childName);
        }

        public static string GetProperty(this XmlElement xmlElement, string propertyName)
        {
            if (xmlElement.Attributes.GetNamedItem(propertyName) != null)
                return xmlElement.GetAttribute(propertyName);
            else if (xmlElement.GetElementsByTagName(propertyName).Count > 0)
                return xmlElement.GetChildElement(propertyName).FirstChild.Value;
            else
                return string.Empty;
        }
    }
}
