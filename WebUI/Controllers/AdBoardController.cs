using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using WebUI.Extensions;
using WebUI.Models.At;
using WebUI.Models.Shared;
using WebUI.Utility;

namespace WebUI.Controllers
{


    [UserSessionActionFilter]
    [UserRoleAuthorize(Roles = "AtAdmin")]
    [NoDirectAccess]
    public class AdBoardController : Controller
    {

        internal string UserName { get { return UserProfileBO.Instance.UserName; } }
        public ActionResult EventCount()
        {
            Sitemap.Clear();
            Sitemap.Add(new Sitemap() { Name = "Home", Action = "index", Controller = "home" }, "Events");
            var eventCounts = EventCountDB.Ad_Get(UserName);
            return View(eventCounts);
        }

        public ActionResult SessionCount(int eid)
        {
            var em = EventMasterDB.Get(eid);
            Sitemap.Add(new Sitemap() { Name = $"{em.EventName} | {em.ExamMonthYear}", Action = "eventcount", Controller = "adboard" }, "Exam Sessions");

            var sessionCounts = SubjectCountDB.Ad_Get(eid);
            return View(sessionCounts);
        }
        public ActionResult SubjectCount(string esub)
        {
            var sm = SubjectMaster.ToObject(esub);
            Sitemap.Add(new Sitemap() { Name = $"{sm.ExamDate} | {sm.ExamTime}", Action = "sessioncount", Controller = "adboard", RouteValue = new { eid = sm.EId } }, "Subjects");

            var subjectCounts = SubjectCountDB.Ad_Get(sm.EId, sm.ExamDate, sm.ExamTime);
            ViewBag.esub = esub;
            return View(subjectCounts);
        }

        public ActionResult ExamcentreCount(string esub)
        {
            var sm = SubjectMaster.ToObject(esub);
            Sitemap.Add(new Sitemap() { Name = $"{sm.Subject} | {sm.PaperCode} | {sm.PaperName}", Action = "subjectcount", Controller = "adboard", RouteValue = new { esub } }, "Examcentres");

            var examcentreCounts = ExamcentreCountDB.Ad_Get(sm.Id);
            ViewBag.esub = esub;
            return View(examcentreCounts);
        }

        public ActionResult FormDataDetail(string eccode, string esub)
        {
            var ecm = ExamCentreMasterDB.Get(eccode);
            var sm = SubjectMaster.ToObject(esub);
            Sitemap.Add(new Sitemap() { Name = $"{ecm.ExamCentreCode} | {ecm.ExamCentreName}", Action = "examcentrecount", Controller = "adboard", RouteValue = new { esub } }, "Students");

            var formdatas = FormDataDB.Ad_Get(eccode, sm.Id);
            return View(formdatas);
        }

        public ActionResult FormDataQuery(string screen, string query, string esub, string eccode=null)
        {
            var sm = SubjectMaster.ToObject(esub);

            FormDataQueryName queryName;
            Enum.TryParse(query, out queryName);

            switch (screen.ToLower())
            {
                case "eventcount":
                    var em = EventMasterDB.Get(sm.EId);
                    Sitemap.Add(new Sitemap() { Name = $"{em.EventName} | {em.ExamMonthYear}", Action = "eventcount", Controller = "adboard" }, queryName.ToEnumMemberAttrValue());
                    break;
                case "sessioncount":
                    Sitemap.Add(new Sitemap() { Name = $"{sm.ExamDate} | {sm.ExamTime}", Action = "sessioncount", Controller = "adboard", RouteValue = new { eid = sm.EId } }, queryName.ToEnumMemberAttrValue());
                    break;
                case "subjectcount":
                    Sitemap.Add(new Sitemap() { Name = $"{sm.Subject} | {sm.PaperCode} | {sm.PaperName}", Action = "subjectcount", Controller = "adboard", RouteValue = new { esub } }, queryName.ToEnumMemberAttrValue());
                    break;
                case "examcentrecount":
                    var ecm = ExamCentreMasterDB.Get(eccode);
                    Sitemap.Add(new Sitemap() { Name = $"{ecm.ExamCentreCode} | {ecm.ExamCentreName}", Action = "examcentrecount", Controller = "adboard", RouteValue = new { esub } }, queryName.ToEnumMemberAttrValue());
                    break;
                default:
                    break;
            }

            var formdatas = FormDataDB.Ad_Query(UserName, query, sm.EId, sm.ExamDate, sm.ExamTime, sm.Id == 0 ? null : sm.Id.ToString(), eccode);
            return View(formdatas);
        }

        public enum FormDataQueryName
        {
            [EnumMember(Value = "Total")]
            T,
            [EnumMember(Value = "Present")]
            P,
            [EnumMember(Value = "Absent")]
            A,
            [EnumMember(Value = "Malpractice")]
            MP,
            [EnumMember(Value = "Buffer")]
            B,
            [EnumMember(Value = "Attendance Pending")]
            ATPENDING,
            [EnumMember(Value = "IO Pending")]
            IOPENDING,
            [EnumMember(Value = "Scan Pending")]
            SCANPENDING
        }


    }
}