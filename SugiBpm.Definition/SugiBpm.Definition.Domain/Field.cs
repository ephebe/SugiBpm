using SugiBpm.Delegation.Interface.Definition;
using SunStone.Util;
using System;
using System.Xml;
using SugiBpm.Delegation.Interface;

namespace SugiBpm.Definition.Domain
{
    public class Field : IField
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public State State { get; set; }
        public AttributeDef Attribute { get; set; }

        public FieldAccess Access { get; set; }

        IAttribute IField.Attribute
        {
            get { return this.Attribute; }
        }

        IState IField.State
        {
            get { return this.State; }
        }

        private ProcessDefinitionCreationContext creationContext;
        public Field()
        {
        }

        public Field(ProcessDefinitionCreationContext creationContext)
        {
            this.Id = SequentialGuid.NewGuid();
            this.creationContext = creationContext;
        }

        public void ReadProcessData(XmlElement xmlElement)
        {
            string attributeName = xmlElement.GetProperty("attribute");

            creationContext.AddUnresolvedReference(this, attributeName, creationContext.ProcessBlock, "attribute", typeof(AttributeDef));

            this.State = creationContext.State;

            string accessText = xmlElement.GetProperty("access");
            this.Access = FieldAccess.FromText(accessText);
        }

        public IHtmlFormatter GetHtmlFormatter()
        {
            throw new NotImplementedException();
        }
    }
}
