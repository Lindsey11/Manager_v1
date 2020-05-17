using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Invoice.WebUI.CustomAttritube
{
    public class CookieEnableChecker : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cartCookie = filterContext.HttpContext.Request.Cookies["ProductList"];
            if (cartCookie == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                                         {{ "controller", "Error" },
                                        { "action", "Cookie" }});
            }
            base.OnActionExecuting(filterContext);
        }
    }
}