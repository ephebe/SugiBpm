using System.Collections.Generic;

namespace SugiBpm.Definition.Domain
{
    public static class AttributeTypes
    {
        private static readonly IDictionary<string, string> attributeTypes = new Dictionary<string, string>();

        static AttributeTypes()
        {
            attributeTypes.Add("actor", "ActorSerializer");
            attributeTypes.Add("text", "TextSerializer");
            attributeTypes.Add("long", "LongSerializer");
            attributeTypes.Add("integer", "IntegerSerializer");
            attributeTypes.Add("float", "FloatSerializer");
            attributeTypes.Add("date", "DateSerializer");
            attributeTypes.Add("evaluation", "EvaluationSerializer");
        }

        public static string FindAttributeTypes(string key)
        {
            return attributeTypes[key];
        }
    }
}
