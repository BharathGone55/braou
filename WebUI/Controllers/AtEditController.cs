using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebUI.Models.Shared;
using WebUI.Utility;

namespace WebUI.Controllers
{

    [UserSessionActionFilter]
    [UserRoleAuthorize(Roles = "AtUser")]
    [NoDirectAccess]
    public class AtEditController : Controller
    {
        internal string EcCode { get { return UserProfileBO.Instance.ExamCentreCode; } }
        public ActionResult Index(string esub)
        {
            var sm = SubjectMaster.ToObject(esub);
            Sitemap.Add(new Sitemap() { Name = $"{sm.Subject} | {sm.PaperCode} | {sm.PaperName}", Action = "subjectcount", Controller = "dashboard", RouteValue = new { esub } }, "Students");

            ViewBag.CourseList = CourseListDB.Get(EcCode, sm.Id, EcCode);

            var fd = FormDataDB.Get(EcCode, sm.Id);
            return View(fd);
        }

        public JsonResult UpdateAtStatus(int id, string status)
        {
            var isUpdated = FormDataDB.UpdateAtStatus(id, status, EcCode);
            return Json(new { OK = isUpdated }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubmitAtStatus(int subid)
        {
            var isSubmitted = FormDataDB.SubmitAtStatus(EcCode, subid, EcCode);
            return Json(new { OK = isSubmitted }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ResetSubmitAtStatus(int subid)
        {
            var isReset = FormDataDB.ResetSubmitAtStatus(EcCode, subid, EcCode);
            return Json(new { OK = isReset }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidateBuffer(int subid, string htno)
        {
            var sm = SubjectMasterDB.Get(subid);

            if (!FormDataDB.ValidateBuffer(htno, sm.EId))
                return Json(new { OK = false, Error = "Hall ticket number is invalid." }, JsonRequestBehavior.AllowGet);

            var fds = FormDataDB.Get_IHTNO(htno, sm.EId);

            var fd = fds.Where(x => x.SubId == sm.Id).SingleOrDefault();
            if (fd != null)
            {
                if (fd.ExamCentreCode == EcCode)
                    return Json(new { OK = false, Error = "Hall ticket number exists already." }, JsonRequestBehavior.AllowGet);
                else
                {
                    var ecm = ExamCentreMasterDB.Get(EcCode);
                    return Json(new { OK = false, Error = $"Hall ticket number exists already in examcentre [ ({ecm.ExamCentreCode}) {ecm.ExamCentreName} ]." }, JsonRequestBehavior.AllowGet);
                }
            }

            string SName = fds.FirstOrDefault()?.SName;
            return Json(new { OK = true, SName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddBuffer(int subid, string htno, string sname, string medium)
        {
            dynamic result = ValidateBuffer(subid, htno);
            if (!result.Data.OK)
                return result;
            if (string.IsNullOrEmpty(result.Data.SName))
            {
                var regex = new Regex(@"^[a-zA-Z .]{2,30}$");
                if (!regex.IsMatch(sname))
                    return Json(new { OK = false, Error = "Student name is invalid." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (result.Data.SName.ToUpper() != sname.ToUpper())
                    return Json(new { OK = false, Error = "Student name is invalid." }, JsonRequestBehavior.AllowGet);
            }

            var isAdded = FormDataDB.Addbuffer(EcCode, subid, htno, sname.ToUpper(), medium, EcCode);
            return Json(new { OK = isAdded }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteBuffer(int id)
        {
            var isDeleted = FormDataDB.DeleteBuffer(id, UserProfileBO.Instance.ExamCentreCode);
            return Json(new { OK = isDeleted }, JsonRequestBehavior.AllowGet);
        }
    }
}