using Microsoft.Practices.ServiceLocation;
using SugiBpm.Definition.Domain;
using SugiBpm.Delegation.Interface;
using SunStone.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Domain
{
    public class AttributeInstance
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Flow Scope { get; set; }
        public Guid? AttributeId { get; protected set; }
        public string AttributeName { get; set; }
        public string ValueText { get; set; }

        //已解析過的不必再解析，讀取暫存值
        private bool valueInitialized = false;
        private object attributeValue = null;
        private ISerializer serializer = null;

        public AttributeInstance()
        {
        }

        public AttributeInstance(AttributeDef attribute, Flow scope)
        {
            this.Id = SunStone.Util.SequentialGuid.NewGuid();
            ValueText = attribute.InitialValue;
            AttributeId = attribute.Id;
            AttributeName = attribute.Name;
            Scope = scope;
        }

        public object GetValue()
        {
            if (valueInitialized)
            {
                return this.attributeValue;
            }
            else
            {
                if (!string.IsNullOrEmpty(ValueText))
                {
                    var attributeRepository = ServiceLocator.Current.GetInstance<IRepository<AttributeDef>>();
                    var attribute = attributeRepository.With(w => w.SerializerDelegation).Get(this.AttributeId);

                    var delegationHelper = ServiceLocator.Current.GetInstance<IDelegationHelper>();
                    this.serializer = delegationHelper.DelegateSerializer(attribute.SerializerDelegation);
                    this.attributeValue = serializer.Deserialize(this.ValueText);
                    this.valueInitialized = true;
                    return this.attributeValue;
                }
                else
                {
                    return null;
                }
            }
        }

        public void SetValue(object valueObject)
        {
            this.attributeValue = valueObject;
            this.valueInitialized = true;

            if (valueObject != null)
            {
                var attributeRepository = ServiceLocator.Current.GetInstance<IRepository<AttributeDef>>();
                var attribute = attributeRepository.With(w => w.SerializerDelegation).Get(this.AttributeId);

                var delegationHelper = ServiceLocator.Current.GetInstance<IDelegationHelper>();
                this.serializer = delegationHelper.DelegateSerializer(attribute.SerializerDelegation);
                ValueText = this.serializer.Serialize(valueObject);
              
            }
            else
            {
                ValueText = null;
            }
        }
    }
}
