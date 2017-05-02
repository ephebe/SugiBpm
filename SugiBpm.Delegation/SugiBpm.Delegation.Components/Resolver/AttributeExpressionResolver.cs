using SugiBpm.Delegation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components
{
    public class AttributeExpressionResolver
    {
        private const string LEFT_MARKER = "${";
        private const string RIGHT_MARKER = "}";

        // the singleton pattern constructor
        private AttributeExpressionResolver()
        {
        }

        public string ResolveAttributeExpression(string expression, IHandlerContext handlerContext)
        {
            string text = expression;

            int leftMarkerIndex = text.IndexOf(LEFT_MARKER);
            int rightMarkerIndex = text.IndexOf(RIGHT_MARKER, leftMarkerIndex + LEFT_MARKER.Length);

            while ((leftMarkerIndex != -1) && (rightMarkerIndex != -1))
            {
                string attributeName = text.Substring(leftMarkerIndex + LEFT_MARKER.Length, (rightMarkerIndex) - (leftMarkerIndex + LEFT_MARKER.Length)).Trim();


                try
                {
                    object attribute = handlerContext.GetAttribute(attributeName);
                    if (attribute != null)
                    {
                        string attributeString = attribute.ToString();
                        text = text.Substring(0, leftMarkerIndex) + attributeString + text.Substring(rightMarkerIndex + RIGHT_MARKER.Length);
                        rightMarkerIndex = rightMarkerIndex + attributeString.Length - attributeName.Length - LEFT_MARKER.Length - RIGHT_MARKER.Length;
                    }
                }
                catch (Exception e)
                {
                    //log.Debug("attribute '" + attributeName + "' could not be resolved in attribute expression '" + expression + "'. Exception: " + e.Message);
                }

                leftMarkerIndex = text.IndexOf(LEFT_MARKER, rightMarkerIndex + RIGHT_MARKER.Length);
                rightMarkerIndex = text.IndexOf(RIGHT_MARKER, leftMarkerIndex + LEFT_MARKER.Length);
            }

            return text;
        }
    }
}
