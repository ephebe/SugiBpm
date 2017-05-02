using Microsoft.Practices.ServiceLocation;
using SugiBpm.Delegation.Interface;
using SugiBpm.Delegation.Interface.Definition;
using SugiBpm.Delegation.Interface.Organization;
using SugiBpm.Execution.Application;
using SugiBpm.Execution.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SugiBpm.Web.Controllers
{
    [Authorize]
    public class FormController : Controller
    {
        public ActionResult StartForm(Guid processDefinitionId)
        {
            IExecutionApplication executionApplication = null;
            IProcessDefinitionApplication definitionApplication = null;
            
            executionApplication = ServiceLocator.Current.GetInstance<IExecutionApplication>();
            definitionApplication = ServiceLocator.Current.GetInstance<IProcessDefinitionApplication>();
            IProcessDefinition processDefinition = definitionApplication.GetProcessDefinition(processDefinitionId);

            //create Form
            ActivityFormView activityForm = executionApplication.GetStartForm(processDefinitionId);

            return View(processDefinition.Name.Replace(" ",""),activityForm);
        }

        public ActionResult ActivityForm(Guid flowId)
        {
            IExecutionApplication executionApplication = null;

            executionApplication = ServiceLocator.Current.GetInstance<IExecutionApplication>();
            FlowView flow = executionApplication.GetFlow(flowId);

            //create Form
            ActivityFormView activityForm = executionApplication.GetActivityForm(flowId);

            return View(flow.ProcessDefinitionName.Replace(" ", ""), activityForm);
        }

        [HttpPost]
        public ActionResult SubmitForm(ActivityFormView model)
        {
            IExecutionApplication executionApplication = null;
            executionApplication = ServiceLocator.Current.GetInstance<IExecutionApplication>();
            var userId = Guid.Parse(this.User.Identity.Name);

            if (model.Parameters != null)
            {
                model.AttributeValues = new Dictionary<string, object>();
                foreach (var item in model.Parameters)
                {
                    Evaluation evaluation;
                    if (Evaluation.TryParseEvaluation(item.Value, out evaluation))
                    {
                        model.AttributeValues.Add(item.Key, evaluation);
                        continue;
                    }
                    DateTime selectDateTime;
                    if (DateTime.TryParse(item.Value, out selectDateTime))
                    {
                        model.AttributeValues.Add(item.Key, selectDateTime);
                        continue;
                    }
                    model.AttributeValues.Add(item.Key, item.Value);
                }
            }

            if (model.ProcessDefinitionId != null)
                executionApplication.StartProcessInstance(userId, model.ProcessDefinitionId.Value, model.AttributeValues);
            if (model.FlowId != null)
                executionApplication.PerformActivity(userId, model.FlowId.Value, model.AttributeValues);

            return this.Redirect("/User");
        }


    }
}