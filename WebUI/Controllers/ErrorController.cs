using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models.Shared;

namespace WebUI.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            var error = TempData["error"] as Error;
            LogOff();
            //Response.StatusCode = error.Code;
            return View(error);
        }

        public ActionResult AccessDenied()
        {
            TempData["error"] = new Error() { Code = 403, Description = "Access denied." };
            return RedirectToAction("index");
        }

        public ActionResult SessionTimeout()
        {
            TempData["error"] = new Error() { Code = 500, Description = "Session timeout." };
            return RedirectToAction("index");
        }

        public ActionResult UnAuthorizedUser()
        {
            TempData["error"] = new Error() { Code = 500, Description = "Unauthorized user." };
            return RedirectToAction("index");
        }

        public void LogOff()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Session.Abandon();
            }
        }


    }
}