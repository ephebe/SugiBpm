using SugiBpm.Execution.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SugiBpm.Definition.Domain;
using SugiBpm.Delegation.Interface;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Practices.ServiceLocation;
using SunStone.Data;
using SugiBpm.Delegation.Interface.Organization;
using Serilog;
using SunStone.Specifications;

namespace SugiBpm.Delegation.Domain
{
    public class DelegationHelper : IDelegationHelper
    {
        [ImportMany]
        public IEnumerable<System.Lazy<IActionHandler, IClassNameMetadata>> ActionHanders { get;set;}
        [ImportMany]
        public IEnumerable<System.Lazy<IDecisionHandler, IClassNameMetadata>> DecisionHandlers { get; set; }
        [ImportMany]
        public IEnumerable<System.Lazy<IForkHandler, IClassNameMetadata>> ForkHandlers { get; set; }
        [ImportMany]
        public IEnumerable<System.Lazy<IJoinHandler, IClassNameMetadata>> JoinHandlers { get; set; }
        [ImportMany]
        public IEnumerable<System.Lazy<ISerializer, IClassNameMetadata>> Serializers { get; set; }
        [ImportMany]
        public IEnumerable<System.Lazy<IAssignmentHandler, IClassNameMetadata>> AssignmentHandlers { get; set; }

        public DelegationHelper()
        {
            try
            {
                DirectoryCatalog catalog = null;
                if (SunStone.Storage.Store.IsWebApplication)
                    catalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory + "/bin");
                else
                    catalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory);

                var container = new CompositionContainer(catalog);
                container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DelegateAction(DelegationDef delegation, ExecutionContext executionContext)
        {
            try
            {
                //executionContext.CreateLog(EventType.ACTION);
                //executionContext.AddLogDetail(new DelegateCallImpl(delegation, typeof(IAction)));
                foreach (var actionHandler in ActionHanders)
                {
                    if (actionHandler.Metadata.ClassName == delegation.ClassName)
                    {
                        executionContext.Configuration = (ParseConfiguration(delegation));
                        actionHandler.Value.Run(executionContext);
                    }
                }
            }
            catch (Exception t)
            {
                HandleException(delegation, executionContext, t);
            }
        }

        public Transition DelegateDecision(DelegationDef delegation, ExecutionContext executionContext)
        {
            Transition selectedTransition = null;

            try
            {
                string transitionName = null;
                IDecisionHandler decision = null;
                foreach (var decisionHandler in DecisionHandlers)
                {
                    if ((string)decisionHandler.Metadata.ClassName == delegation.ClassName)
                    {
                        decision = decisionHandler.Value;
                        executionContext.Configuration = (ParseConfiguration(delegation));
                        transitionName = decision.Decide(executionContext);
                    }
                }


                if (string.IsNullOrEmpty(transitionName))
                {
                    throw new SystemException("Decision-delegate for decision '" + executionContext.Node + "' returned null instead of a transition-name : " + decision.GetType().FullName);
                }

                try
                {
                    var transitionRepository = ServiceLocator.Current.GetInstance<IRepository<Transition>>();
                   
                    selectedTransition = transitionRepository.With(s=>s.To)
                        .Query(new Specification<Transition>(s => s.From.Id == executionContext.Node.Id && s.Name == transitionName)).Single();
                }
                catch (Exception t)
                {
                    throw new SystemException("couldn't find transition '" + transitionName + "' that was selected by the decision-delegate of activity '" + executionContext.Node.Name + "' : " + t.Message);
                }
            }
            catch (Exception t)
            {
                HandleException(delegation, executionContext, t);
            }

            return selectedTransition;
        }

        public void DelegateFork(DelegationDef delegation, ExecutionContext executionContext)
        {
            try
            {
                foreach (var forkHandler in ForkHandlers)
                {
                    if ((string)forkHandler.Metadata.ClassName == delegation.ClassName)
                    {
                        executionContext.Configuration = (ParseConfiguration(delegation));
                        forkHandler.Value.Fork(executionContext);
                    }
                }
            }
            catch (Exception t)
            {
                HandleException(delegation, executionContext, t);
            }
        }

        public bool DelegateJoin(DelegationDef delegation, ExecutionContext executionContext)
        {
            bool reactivateParent = false;

            try
            {
                foreach (var joinHandler in JoinHandlers)
                {
                    if ((string)joinHandler.Metadata.ClassName == delegation.ClassName)
                    {
                        executionContext.Configuration = (ParseConfiguration(delegation));
                        reactivateParent = joinHandler.Value.Join(executionContext);
                    }
                }
            }
            catch (Exception t)
            {
                HandleException(delegation, executionContext, t);
            }

            return reactivateParent;
        }

        public IActor DelegateAssignment(DelegationDef delegation, ExecutionContext executionContext)
        {
            IActor actor = null;
            try
            {
                foreach (var assignmentHandler in AssignmentHandlers)
                {
                    if ((string)assignmentHandler.Metadata.ClassName == delegation.ClassName)
                    {
                        executionContext.Configuration = (ParseConfiguration(delegation));
                        actor = assignmentHandler.Value.SelectActor(executionContext);
                    }
                }
            }
            catch (Exception t)
            {
                HandleException(delegation, executionContext, t);
            }

            return actor;
        }

        public ISerializer DelegateSerializer(DelegationDef delegation)
        {
            try
            {
                foreach (var serializer in Serializers)
                {
                    if ((string)serializer.Metadata.ClassName == delegation.ClassName)
                    {
                        return serializer.Value;
                    }
                }
            }
            catch (Exception t)
            {
                //HandleException(delegation, executionContext, t);
            }

            return null;
        }

        private IDictionary<string, object> ParseConfiguration(DelegationDef delegation)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            try
            {
                string configuration = delegation.Configuration;
                if (!string.IsNullOrEmpty(configuration))
                {
                    XElement xElement = XElement.Parse(configuration);

                    var parameterXmlElements = xElement.Elements("parameter");
                    foreach (XElement element in parameterXmlElements)
                    {
                        string name = element.Attribute("name").Value;
                        if (string.IsNullOrEmpty(name))
                        {
                            throw new SystemException("invalid delegation-configuration : " + configuration);
                        }

                        parameters.Add(name, element.Value);
                    }
                }
            }
            catch (Exception t)
            {
                Log.Error("can't parse configuration : ", t);
                throw new SystemException("can't parse configuration : " + t.Message);
            }

            return parameters;
        }

        private void HandleException(DelegationDef delegation, ExecutionContext executionContext, Exception exception)
        {
            Log.Debug("handling delegation exception :", exception);

            string exceptionClassName = exception.GetType().FullName;
            string delegationClassName = delegation.ClassName;

            ExceptionHandlingType exceptionHandlingType = delegation.ExceptionHandlingType;

            if (exceptionHandlingType != 0)
            {
                if (exceptionHandlingType == ExceptionHandlingType.IGNORE)
                {
                    Log.Debug("ignoring '" + exceptionClassName + "' in delegation '" + delegationClassName + "' : " + exception.Message);
                }
                else if (exceptionHandlingType == ExceptionHandlingType.LOG)
                {
                    Log.Debug("logging '" + exceptionClassName + "' in delegation '" + delegationClassName + "' : " + exception.Message);
                    //executionContext.AddLogDetail(new ExceptionReportImpl(exception));
                }
                else if (exceptionHandlingType == ExceptionHandlingType.ROLLBACK)
                {
                    Log.Debug("rolling back for '" + exceptionClassName + "' in delegation '" + delegationClassName + "' : " + exception.Message);
                    throw new SystemException("rolling back for '" + exceptionClassName + "' in delegation '" + delegationClassName + "' : " + exception.Message);
                }
                else
                {
                    throw new SystemException("unknown exception handler '" + exceptionHandlingType + "' : " + exception.Message);
                }
            }
            else
            {
                Log.Debug("'" + exceptionClassName + "' in delegation '" + delegationClassName + "' : " + exception.Message);
                //executionContext.AddLogDetail(new ExceptionReportImpl(exception));
            }
        }
    }
}
