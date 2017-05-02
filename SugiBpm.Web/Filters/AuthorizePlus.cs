using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SugiBpm.Web.Filters
{
    public class AuthorizePlus : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Convert.ToBoolean(filterContext.HttpContext.Session["auth"]))
            {

            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}