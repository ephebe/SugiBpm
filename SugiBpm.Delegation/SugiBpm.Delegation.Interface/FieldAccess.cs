using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Interface
{
    public class FieldAccess
    {
        private static readonly int NOT_ACCESSIBLE = 1;
        private static readonly int READ_ONLY = 2;
        private static readonly int WRITE_ONLY = 3;
        private static readonly int WRITE_ONLY_REQUIRED = 4;
        private static readonly int READ_WRITE = 5;
        private static readonly int READ_WRITE_REQUIRED = 6;

        public int AccessType { get; set; }

        protected FieldAccess()
        {
        }

        public FieldAccess(int accessType)
        {
            this.AccessType = accessType;
        }

        public bool IsAccessible()
        {
            return (!(AccessType == FieldAccess.NOT_ACCESSIBLE));
        }

        public bool IsReadable()
        {
            return ((AccessType == FieldAccess.READ_ONLY) || (AccessType == FieldAccess.READ_WRITE) || (AccessType == FieldAccess.READ_WRITE_REQUIRED));
        }

        public bool IsWritable()
        {
            return ((AccessType == FieldAccess.WRITE_ONLY) || (AccessType == FieldAccess.READ_WRITE) || (AccessType == FieldAccess.WRITE_ONLY_REQUIRED) || (AccessType == FieldAccess.READ_WRITE_REQUIRED));
        }

        public bool IsRequired()
        {
            return ((AccessType == FieldAccess.WRITE_ONLY_REQUIRED) || (AccessType == FieldAccess.READ_WRITE_REQUIRED));
        }

        public static FieldAccess FromText(string accessText)
        {
            if (accessText == "not-accessible")
            {
                return new FieldAccess(FieldAccess.NOT_ACCESSIBLE);
            }
            else if (accessText.Equals("read-only"))
            {
                return new FieldAccess(FieldAccess.READ_ONLY);
            }
            else if (accessText.Equals("write-only"))
            {
                return new FieldAccess(FieldAccess.WRITE_ONLY);
            }
            else if (accessText.Equals("write-only-required"))
            {
                return new FieldAccess(FieldAccess.WRITE_ONLY_REQUIRED);
            }
            else if (accessText.Equals("read-write"))
            {
                return new FieldAccess(FieldAccess.READ_WRITE);
            }
            else if (accessText.Equals("read-write-required"))
            {
                return new FieldAccess(FieldAccess.READ_WRITE_REQUIRED);
            }
            throw new ArgumentException(string.Format("accessText {0} can't found.",accessText));
        }
    }
}
