using Microsoft.Practices.ServiceLocation;
using SugiBpm.Delegation.Interface;
using SugiBpm.Delegation.Interface.Organization;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components
{
    [Export(typeof(ISerializer))]
    [ExportMetadata("ClassName", "ActorSerializer")]
    public class ActorSerializer : ISerializer
    {
        public object Deserialize(string text)
        {
            if (text == null)
                return null;

            IActor actor = null;
            IOrganizationApplication organizationApplication = ServiceLocator.Current.GetInstance<IOrganizationApplication>();
            try
            {
                actor = organizationApplication.FindActorByUniqueName(text);
            }
            catch (Exception t)
            {
                throw new ArgumentException("couldn't deserialize " + text + " to a User : " + t.GetType().FullName + " : " + t.Message);
            }

            return actor;
        }

        public string Serialize(object object_Renamed)
        {
            string serialized = null;

            if ((!(object_Renamed is IUser)) && (!(object_Renamed is IGroup)))
            {
                throw new ArgumentException("couldn't serialize " + object_Renamed);
            }

            if (object_Renamed != null)
            {
                IActor actor = (IActor)object_Renamed;
                serialized = actor.UniqueName;
            }

            return serialized;
        }

        public void SetConfiguration(IDictionary<string, object> configuration)
        {
            throw new NotImplementedException();
        }
    }
}
