using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebUI.Models.At;
using WebUI.Models.Shared;
using WebUI.Utility;

namespace WebUI.Controllers
{
    [UserSessionActionFilter]
    [UserRoleAuthorize(Roles = "AtUser")]
    [NoDirectAccess]
    public class DashboardController : Controller
    {
        internal string EcCode { get { return UserProfileBO.Instance.ExamCentreCode; } }
        public ActionResult EventCount()
        {
            Sitemap.Clear();
            Sitemap.Add(new Sitemap() { Name = "Home", Action = "index", Controller = "home" }, "Events");
            var eventCountList = EventCountDB.Get(EcCode);
            return View(eventCountList);
        }

        public ActionResult SessionCount(int eid)
        {
            var em = EventMasterDB.Get(eid);
            Sitemap.Add(new Sitemap() { Name = $"{em.EventName} | {em.ExamMonthYear}", Action = "eventcount", Controller = "dashboard" }, "Exam Sessions");
            
            var sessionCountList = SubjectCountDB.Get(EcCode, eid);
            return View(sessionCountList);
        }
        public ActionResult SubjectCount(string esub)
        {
            var sm = SubjectMaster.ToObject(esub);
            Sitemap.Add(new Sitemap() { Name = $"{sm.ExamDate} | {sm.ExamTime}", Action = "sessioncount", Controller = "dashboard", RouteValue = new { eid = sm.EId } }, "Subjects");
                        
            var subjectCountList = SubjectCountDB.Get(EcCode, sm.EId, sm.ExamDate, sm.ExamTime);
            ViewBag.havecoverslip = !subjectCountList.Where(x => x.Pending > 0).Any();
            ViewBag.esub = esub;
            return View(subjectCountList);
        }
    }
}