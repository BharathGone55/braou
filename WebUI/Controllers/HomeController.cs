using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    [NoDirectAccess]
    public class HomeController : Controller
    {
        [UserSessionActionFilter]
        [UserRoleAuthorize(Roles = "AtUser,AtAdmin")]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}