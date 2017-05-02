using System;

namespace SugiBpm.Definition.Domain
{
    public class ReferencableObject
    {
        public ProcessBlock Scope { get; set; }
        public Type Type { get; set; }

        public ReferencableObject(ProcessBlock scope, Type type)
        {
            Type = type;
            Scope = scope;
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            ReferencableObject refObject = obj as ReferencableObject;
            if (refObject == null) return false;

            return (refObject.Type.Equals(this.Type)
                && refObject.Scope.Equals(this.Scope));
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
    }
}
