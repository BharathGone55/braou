using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebUI.Models.Shared;

namespace WebUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    public class UserSessionActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["User"] == null)
            {
                /// this handles session when data is requested through Ajax json
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.HttpContext.Response.Headers.Add("X-Session-Timeout", "Session Timeout!");
                    filterContext.Result = new JsonResult
                    {
                        Data = new { Error = true, ErrorMessage = "Session Timeout!" },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    /// If session is expired then redirected to logout page which further redirect to login page. 
                    //filterContext.Result = new RedirectResult("~/error/sessiontimeout");
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "error", action = "sessiontimeout" }));
                    return;
                }
            }
        }
    }

    public class UserRoleAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //if not logged, it will work as normal Authorize and redirect to the Login
                base.HandleUnauthorizedRequest(filterContext);

            }
            else
            {
                //logged and wihout the role to access it - redirect to the custom controller action
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "error", action = "accessdenied" }));
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.UrlReferrer == null ||
     filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
            {
                filterContext.Result = new RedirectToRouteResult(new
                                          RouteValueDictionary(new { controller = "Account", action = "Logoff", area = "" }));
            }
        }
    }
}
