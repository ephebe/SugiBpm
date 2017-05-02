using SugiBpm.Delegation.Interface.Organization;
using SunStone.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Organization.Domain
{
    public abstract class Actor : IActor,IComparable
    {
        public Guid Id { get; protected set; }

        public string UniqueName { get; set; }

        public abstract string Name { get; set; }

        public Actor()
        {
            this.Id = SequentialGuid.NewGuid();
        }

        public Actor(string uniqueName)
            :this()
        {
            this.UniqueName = uniqueName;
        }

        public override string ToString()
        {
            string className = this.GetType().FullName;
            int to = className.Length;

            int from = className.LastIndexOf('.') + 1;
            className = className.Substring(from, (to) - (from));
            return className + "[" + UniqueName + "]";
        }

        // equals
        public override bool Equals(object object_Renamed)
        {
            bool isEqual = false;
            Actor actor = (Actor)object_Renamed;
            if ((object_Renamed != null))
            {
                isEqual = this.Id.Equals(actor.Id);
                
            }
            return isEqual;
        }

        // hashCode
        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode = Id.GetHashCode();

            return hashCode;
        }

        // compareTo  
        public Int32 CompareTo(Object object_Renamed)
        {
            Int32 difference = -1;

            Actor actor = (Actor)object_Renamed;
            if ((actor != null))
            {
                difference = this.Id.CompareTo(actor.Id);
            }
            else
            {
                throw new SystemException("can't compare two actors this(" + this + ") and object(" + object_Renamed + ")");
            }

            return difference;
        }
    }
}
