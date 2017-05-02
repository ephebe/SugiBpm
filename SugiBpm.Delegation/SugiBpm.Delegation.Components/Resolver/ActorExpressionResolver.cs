using Microsoft.Practices.ServiceLocation;
using Serilog;
using SugiBpm.Delegation.Interface;
using SugiBpm.Delegation.Interface.Organization;
using SunStone.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SugiBpm.Delegation.Components
{
    public class ActorExpressionResolver
    {
        private static Type[] RESOLVE_METHOD_ARGUMENT_TYPES = new Type[] { typeof(IActor), typeof(string[]), typeof(IHandlerContext) };
        private IOrganizationApplication organizationApplication = null;

        public ActorExpressionResolver()
        {
            organizationApplication = ServiceLocator.Current.GetInstance<IOrganizationApplication>();
        }

        public IActor ResolveArgument(string expression, IHandlerContext handlerContext)
        {
            return ResolveArgument(null, expression, 0, handlerContext);
        }

        private IActor ResolveArgument(IActor resolvedActor, string expression, int index, IHandlerContext handlerContext)
        {
            string argument = null;
            string[] parameters = null;

            int argumentEndIndex = expression.IndexOf("->", index);
            if (argumentEndIndex == -1)
            {
                argumentEndIndex = expression.Length;
            }

            int parametersStartIndex = expression.IndexOf("(", index);
            int parametersEndIndex = -1;
            if ((parametersStartIndex != -1) && (parametersStartIndex < argumentEndIndex))
            {
                argument = expression.Substring(index, (parametersStartIndex) - (index)).Trim();
                parametersEndIndex = expression.IndexOf(")", parametersStartIndex + 1);

                if (parametersEndIndex > argumentEndIndex)
                {
                    throw new SystemException("can't resolve assigner expression : couldn't find closing bracket for bracket on index '" + parametersStartIndex + "' in expression '" + expression + "'");
                }

                // the next exception happens when a parameter contains a right bracket.
                string shouldBewhiteSpace = expression.Substring(parametersEndIndex + 1, (argumentEndIndex) - (parametersEndIndex + 1));
                if (!"".Equals(shouldBewhiteSpace.Trim()))
                {
                    throw new SystemException("can't resolve assigner expression : only whitespace allowed between closing bracket of the parameterlist of an argument and the end of the argument : closing bracket position '" + parametersEndIndex + "' in expression '" + expression + "'");
                }

                string parametersText = expression.Substring(parametersStartIndex + 1, (parametersEndIndex) - (parametersStartIndex + 1));
                var parameterList = new List<string>();

                foreach (var token in parametersText.Split(','))
                {
                    parameterList.Add(token.Trim());
                }

                if (parameterList.Count > 0)
                {
                    parameters = parameterList.ToArray();
                }
                else
                {
                    parameters = new string[0];
                }
            }
            else
            {
                argument = expression.Substring(index, (argumentEndIndex) - (index)).Trim();
                parameters = new string[0];
            }

            if ("".Equals(argument))
            {
                throw new SystemException("can't resolve assigner expression : can't resolve empty argument on index '" + index + "' for expression '" + expression + "'");
            }

            string methodName = "ResolveArgument" + argument.Substring(0, 1).ToUpper() + argument.Substring(1);
            try
            {
                MethodInfo method = this.GetType().GetMethod(methodName, (Type[])RESOLVE_METHOD_ARGUMENT_TYPES);
                object[] args = new object[] { resolvedActor, parameters, handlerContext };

                resolvedActor = (IActor)method.Invoke(this, (Object[])args);

                Log.Debug("resolving actor expression '" + expression + "',  resolvedActor is " + resolvedActor);
                string whiteSpace = "                                                                                      ".Substring(0, index);
                Log.Debug("                            " + whiteSpace + "^");
            }
            catch (Exception t)
            {
                throw new SystemException("can't resolve assigner expression : couldn't resolve argument '" + argument + "' : " + t.Message, t);
            }


            if (argumentEndIndex != expression.Length)
            {
                if (argumentEndIndex < expression.Length)
                {
                    argumentEndIndex = expression.IndexOf("->", argumentEndIndex) + 2;
                }
                resolvedActor = ResolveArgument(resolvedActor, expression, argumentEndIndex, handlerContext);
            }

            return resolvedActor;
        }

        public object ResolveArgumentPreviousActor(IActor resolvedActor, string[] parameters, IHandlerContext handlerContext)
        {
            if (parameters.Length != 0)
                throw new SystemException("argument previousActor expects exactly zero parameters instead of " + parameters.Length);
            IActor actor = handlerContext.GetPreviousActor();
            if (actor == null)
                throw new SystemException("argument previousActor could not be resolve because there is no previous actor");
            return actor;
        }

        public IActor ResolveArgumentProcessInitiator(IActor resolvedActor, string[] parameters, IHandlerContext handlerContext)
        {
            Guid actorId = handlerContext.GetProcessInstance().InitiatorActorId;
            IActor actor = organizationApplication.FindActor(actorId);
            return actor;
        }

        public IActor ResolveArgumentActor(IActor resolvedActor, string[] parameters, IHandlerContext handlerContext)
        {
            if (parameters.Length != 1)
                throw new SystemException("argument actor expects exactly one (the actor-id) parameter instead of " + parameters.Length);

            IActor actor = null;

            try
            {
                string shortName = parameters[0];
                actor = organizationApplication.FindActorByUniqueName(shortName);
            }
            catch(SystemException e)
            {
                throw new SystemException("can't resolve actor-argument with parameter " + parameters[0], e);
            }

            return actor;
        }

        public IGroup ResolveArgumentGroup(IActor resolvedActor, string[] parameters, IHandlerContext handlerContext)
        {
            //log.Debug("resolvedActor inside resolveArgumentGroup: " + resolvedActor);

            if (resolvedActor == null)
            {
                if (parameters.Length != 1)
                    throw new SystemException("argument group expects exactly one parameter instead of " + parameters.Length);

                string shortName = parameters[0];
                IGroup group = null;

                try
                {
                    group = organizationApplication.FindGroupByUniqueName(shortName);
                }
                catch(Exception e)
                {
                    //throw new SystemException("can't resolve group-argument with parameter " + groupId + " : " + e.Message);
                }

                return group;
            }
            else
            {
                if (parameters.Length != 1)
                    throw new SystemException("argument group expects exactly one parameter (membership-type) instead of " + parameters.Length);

                IGroup group = null;
                string membershipType = parameters[0];

                try
                {
                    group = organizationApplication.FindGroupByUserMembership(resolvedActor.UniqueName, membershipType);
                }
                catch (InvalidCastException e)
                {
                    throw new SystemException("can't resolve group-argument : a group must be calculated from a User, not a " + resolvedActor.GetType().FullName, e);
                }
                catch
                {
                    //throw new SystemException("can't resolve group-argument : can't find the hierarchy-memberschip of User " + user.Id + " and membership-type " + membershipType + " : " + e.Message, e);
                }

                return group;
            }
        }

        public IActor ResolveArgumentRole(IActor resolvedActor, string[] parameters, IHandlerContext handlerContext)
        {
            if (parameters.Length != 1)
                throw new SystemException("argument role expects exactly one parameter (role-name) instead of " + parameters.Length);

            IActor actor = null;

            if (resolvedActor == null)
            {
                try
                {
                    actor = (IActor)handlerContext.GetAttribute(parameters[0]);
                }
                catch (InvalidCastException e)
                {
                    throw new SystemException("argument attribute(" + parameters[0] + ") does not contain an actor : " + handlerContext.GetAttribute(parameters[0]).GetType().FullName, e);
                }
            }
            else
            {
                string roleName = parameters[0].Trim();

                try
                {
                    IList<IUser> users = organizationApplication.FindUsersByGroupAndRole(resolvedActor.UniqueName, roleName);

                    if (users.Count < 1)
                        throw new SystemException("no users have role " + roleName + " for group " + resolvedActor.UniqueName);
                    actor = users[0];

                    // TODO : create a new group if more then one user is returned on the query...
                }
                catch (InvalidCastException e)
                {
                    throw new SystemException("can't resolve role-argument : a role must be calculated from a Group, not a " + resolvedActor.GetType().FullName, e);
                }
                catch
                {
                    //throw new SystemException("can't resolve role-argument : can't find the users that perform role " + roleName + " for group " + resolvedActor.Id + " : " + e.Message);
                }
            }

            return actor;
        }

        public IGroup ResolveArgumentParentGroup(IGroup resolvedActor, string[] parameters, IHandlerContext handlerContext)
        {
            if (parameters.Length != 0)
                throw new SystemException("argument parentGroup expects exactly zero parameters instead of " + parameters.Length);

            IGroup group = null;

            try
            {
                group = group.Parent;
            }
            catch (InvalidCastException e)
            {
                throw new SystemException("can't resolve parentGroup-argument : a role must be calculated from a Group, not a " + resolvedActor.GetType().FullName, e);
            }

            return group;
        }
    }
}
