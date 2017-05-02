using Microsoft.Practices.ServiceLocation;
using SugiBpm.Delegation.Interface;
using SugiBpm.Delegation.Interface.Definition;
using SugiBpm.Delegation.Interface.Execution;
using SugiBpm.Delegation.Interface.Organization;
using SugiBpm.Execution.Application;
using SugiBpm.Execution.Application.ViewModel;
using SugiBpm.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SugiBpm.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public ActionResult Index(string preview, Guid? processDefinitionId)
        {
            IProcessDefinitionApplication definitionApplication = null;
            IExecutionApplication executionApplication = null;
            var userId = Guid.Parse(this.User.Identity.Name);

            definitionApplication = ServiceLocator.Current.GetInstance<IProcessDefinitionApplication>();
            executionApplication = ServiceLocator.Current.GetInstance<IExecutionApplication>();
            IList<FlowView> taskList = executionApplication.GetTaskList(userId);
            IList<IProcessDefinition> processDefinitions = definitionApplication.GetProcessDefinitions();

            var userViewModel = new UserViewModel();
            userViewModel.TaskList = taskList;
            userViewModel.ProcessDefinitions = processDefinitions;
            userViewModel.Preview = preview;

            if (!string.IsNullOrEmpty(preview))
            {
                if (preview == "process") 
                {
                    userViewModel.ProcessDefinitionName = "In Fly";
                }
            }

            return View(userViewModel);
        }


        public void ShowHome(string preview, Guid processDefinitionId, Guid flowId)
        {
            
        }
    }
}