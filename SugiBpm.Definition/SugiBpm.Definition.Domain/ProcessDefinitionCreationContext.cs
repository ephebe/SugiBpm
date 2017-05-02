using System;
using System.Collections.Generic;
using System.Xml;

namespace SugiBpm.Definition.Domain
{
    public class ProcessDefinitionCreationContext
    {
        public ProcessBlock ProcessBlock { get; internal set; }
        public object DelegatingObject { get; internal set; }
        public DefinitionObject DefinitionObject { get; internal set; }
        public State State { get; internal set; }
        public Node Node { get; internal set; }
        public ProcessDefinition ProcessDefinition { get; internal set; }
        public ProcessBlock TransitionDestinationScope { get; internal set; }

        private List<UnresolvedReference> unresolvedReferences;
        private IDictionary<ReferencableObject, IDictionary<string, object>> referencableObjects;


        public ProcessDefinitionCreationContext()
        {
            unresolvedReferences = new List<UnresolvedReference>();
            referencableObjects = new Dictionary<ReferencableObject, IDictionary<string, object>>();
        }

        public ProcessDefinition CreateProcessDefinition(XmlDocument xmlDocument)
        {
            ProcessDefinition processDefinition = new ProcessDefinition(this);
            //processDefinition.ProcessDefinitionId = processDefinition.Id;
            this.ProcessDefinition = processDefinition;
            processDefinition.ReadProcessData((XmlElement)xmlDocument.GetElementsByTagName("process-definition").Item(0));
            return processDefinition;
        }

        public void AddUnresolvedReference(object referencingObject, string destinationName, ProcessBlock destinationScope, string property, Type destinationType)
        {
            unresolvedReferences.Add(new UnresolvedReference(referencingObject, destinationName, destinationScope, property, destinationType));
        }

        public void AddReferencableObject(string name, ProcessBlock scope, Type type, object referencableObject)
        {
            ReferencableObject referenceType = new ReferencableObject(scope, type);
            IDictionary<string, object> referencables = null;
            if (!referencableObjects.ContainsKey(referenceType))
            {
                referencables = new Dictionary<string, object>();
                referencableObjects.Add(referenceType,referencables);
            }
            else
                referencables = referencableObjects[referenceType];

            referencables.Add(name,referencableObject);
        }

        public void ResolveReferences()
        {
            foreach (UnresolvedReference unresolvedReference in unresolvedReferences)
            {
                Object referencingObject = unresolvedReference.ReferencingObject;
                String referenceDestinationName = unresolvedReference.DestinationName;
                ProcessBlock scope = unresolvedReference.DestinationScope;
                String property = unresolvedReference.Property;

                Object referencedObject = FindInScope(unresolvedReference, unresolvedReference.DestinationScope);
                if (referencedObject == null)
                {
                   // AddError("failed to deploy process archive : couldn't resolve " + property + "=\"" + referenceDestinationName + "\" from " + referencingObject + " in scope " + scope);
                }
                else
                {
                    if (referencingObject is Transition)
                    {
                        if (property.Equals("to"))
                        {
                            Transition transition = (Transition)referencingObject;
                            transition.To = (Node)referencedObject;
                            //transition.ToId = transition.To.Id;
                        }
                    }
                    if (referencingObject is Field)
                    {
                        if (property.Equals("attribute"))
                        {
                            Field field = (Field)referencingObject;
                            field.Attribute = (AttributeDef)referencedObject;
                        }
                    }
                }
            }
        }

        private Object FindInScope(UnresolvedReference unresolvedReference, ProcessBlock scope)
        {
            Object referencedObject = null;

            if (scope != null)
            {
                ReferencableObject referenceType = new ReferencableObject(scope, unresolvedReference.DestinationType);

                var referencables = referencableObjects[referenceType];

                if ((referencables != null) && (referencables.ContainsKey(unresolvedReference.DestinationName)))
                {
                    referencedObject = referencables[unresolvedReference.DestinationName];
                }
                else
                {
                    referencedObject = FindInScope(unresolvedReference, scope.ParentBlock);
                }
            }

            return referencedObject;
        }

        private object FindInScope(UnresolvedReference unresolvedReference, object parentBlock)
        {
            throw new NotImplementedException();
        }
    }
}
