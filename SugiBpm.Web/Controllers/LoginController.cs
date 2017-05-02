using Microsoft.Practices.ServiceLocation;
using SugiBpm.Delegation.Interface;
using SugiBpm.Delegation.Interface.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SugiBpm.Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Intro
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            var organizationApplication = ServiceLocator.Current.GetInstance<IOrganizationApplication>();
            IActor user = organizationApplication.FindActorByUniqueName(username);

            if (user != null)
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                    user.Id.ToString(),
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    true,
                    user.UniqueName,
                    FormsAuthentication.FormsCookiePath);

                string encTicket = FormsAuthentication.Encrypt(ticket);

                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);

                return this.Redirect("/User");
            }
            else
            {
                return RedirectToAction("Index");
            }
          
        }
    }
}