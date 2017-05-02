using System;

namespace SugiBpm.Definition.Domain
{
    public class UnresolvedReference
    {
        public object ReferencingObject { get; private set; }
        public string DestinationName { get; private set; }
        public ProcessBlock DestinationScope { get; private set; }
        public string Property { get; private set; }
        public Type DestinationType { get; private set; }

        public UnresolvedReference(object referencingObject,
            string destinationName, 
            ProcessBlock destinationScope,
            string property, 
            Type destinationType)
        {
            this.ReferencingObject = referencingObject;
            this.DestinationName = destinationName;
            this.DestinationScope = destinationScope;
            this.Property = property;
            this.DestinationType = destinationType;
        }

    }
}
