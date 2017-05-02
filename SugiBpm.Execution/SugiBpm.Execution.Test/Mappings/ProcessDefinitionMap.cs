using SugiBpm.Definition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Execution.Test.Mappings
{
    public class ProcessDefinitionMap : EntityTypeConfiguration<ProcessDefinition>
    {
        public ProcessDefinitionMap()
        {
            Property(p => p.StartStateId).HasColumnName("startState");
            Property(p => p.EndStateId).HasColumnName("endState");
            //HasOptional(s => s.StartState).WithOptionalDependent().Map(m => m.MapKey("startState"));
            //HasOptional(s => s.EndState).WithOptionalDependent().Map(m => m.MapKey("endState"));
            Property(p => p.AuthorizationDelegationId).HasColumnName("authorizationDelegation").IsOptional();
            Property(p => p.ResponsibleUserName).HasColumnName("responsibleUserName").HasColumnType("nvarchar").HasMaxLength(255).IsOptional();
        }
    }
}
