using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Reporting;
using Telerik.Reporting.Processing;
using Telerik.Reporting.XmlSerialization;
using WebUI.Extensions;
using WebUI.Models.At;
using WebUI.Models.Shared;

namespace WebUI.Controllers
{
    [NoDirectAccess]
    [UserSessionActionFilter]
    [UserRoleAuthorize(Roles = "AtUser")]
    public class PrintController : Controller
    {
        internal string EcCode { get { return UserProfileBO.Instance.ExamCentreCode; } }

        private string path_DForm { get { return Server.MapPath(@"report\at_dform.trdx"); } }
        private string path_Sticker { get { return Server.MapPath(@"report\at_sticker.trdx"); } }
        private string path_Statement { get { return Server.MapPath(@"report\at_statement.trdx"); } }
        private string path_Packingslip { get { return Server.MapPath(@"report\at_packingslip.trdx"); } }
        private string path_CoverSlip { get { return Server.MapPath(@"report\at_coverslip.trdx"); } }

        public ActionResult Index()
        {
            return View();
        }


        private void Download(Telerik.Reporting.Report report, string destination)
        {
            var instanceReportSource = new InstanceReportSource();
            instanceReportSource.ReportDocument = report;

            ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);

            Response.Clear();
            Response.ContentType = result.MimeType;
            Response.Cache.SetCacheability(HttpCacheability.Private);
            Response.Expires = -1;
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", $"inline;FileName=\"{destination}\"");
            Response.BinaryWrite(result.DocumentBytes);
            Response.End();
        }

        private Telerik.Reporting.Report ToTelerikReport(string source)
        {
            System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings() { IgnoreWhitespace = true };
            using (System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(source, settings))
            {
                var xmlSerializer = new ReportXmlSerializer();
                return (Telerik.Reporting.Report)xmlSerializer.Deserialize(xmlReader);
            }
        }

        public void PrintDform(SubjectMaster sm, string examcentrecode, bool islatecomer, bool isbuffer)
        {
            var report = ToTelerikReport(path_DForm);

            var ecm = ExamCentreMasterDB.Get(examcentrecode);
            report.ReportParameters["ExamCentreCode"].Value = ecm.ExamCentreCode;
            report.ReportParameters["ExamCentreName"].Value = ecm.ExamCentreName;

            var em = EventMasterDB.Get(sm.EId);
            report.ReportParameters["EId"].Value = em.Id;
            report.ReportParameters["ExamMonthYear"].Value = em.ExamMonthYear;

            report.ReportParameters["ExamDate"].Value = sm.ExamDate;
            report.ReportParameters["ExamTime"].Value = sm.ExamTime;
            report.ReportParameters["IsLateComer"].Value = islatecomer;
            report.ReportParameters["IsBuffer"].Value = isbuffer;

            report.DataSource = DFormDB.Get(sm.EId, examcentrecode, sm.ExamDate, sm.ExamTime, islatecomer, isbuffer);

            Download(report, $"DForm_{Guid.NewGuid()}.pdf");
        }

        public void PreprintedDform(string esub)
        {
            var sm = SubjectMaster.ToObject(esub);
            PrintDform(sm, EcCode, false, false);
        }

        public void LateComerDform(string esub)
        {
            var sm = SubjectMaster.ToObject(esub);
            PrintDform(sm, EcCode, true, false);
        }
        public void BufferDform(string esub)
        {
            var sm = SubjectMaster.ToObject(esub);
            PrintDform(sm, EcCode, false, true);
        }

        public void PrintSticker(SubjectMaster sm, string examcentrecode, bool islatecomer, bool isbuffer)
        {
            var report = ToTelerikReport(path_Sticker);

            report.ReportParameters["ExamCentreCode"].Value = examcentrecode;
            report.ReportParameters["EId"].Value = sm.EId;
            report.ReportParameters["ExamDate"].Value = sm.ExamDate;
            report.ReportParameters["ExamTime"].Value = sm.ExamTime;
            report.ReportParameters["IsLateComer"].Value = islatecomer;
            report.ReportParameters["IsBuffer"].Value = isbuffer;

            report.DataSource = Sticker.Get(sm.EId, examcentrecode, sm.ExamDate, sm.ExamTime, islatecomer, isbuffer);

            Download(report, $"Sticker_{Guid.NewGuid()}.pdf");
        }

        public void PreprintedSticker(string esub)
        {
            var sm = SubjectMaster.ToObject(esub);
            PrintSticker(sm, EcCode, false, false);
        }

        public void LateComerSticker(string esub)
        {
            var sm = SubjectMaster.ToObject(esub);
            PrintSticker(sm, EcCode, true, false);
        }
        public void BufferSticker(string esub)
        {
            var sm = SubjectMaster.ToObject(esub);
            PrintSticker(sm, EcCode, false, true);
        }

        public void PrintAtStatement(int subid,  string examcentrecode)
        {
            var report = ToTelerikReport(path_Statement);

            var ecm = ExamCentreMasterDB.Get(examcentrecode);
            report.ReportParameters["ExamCentreCode"].Value = ecm.ExamCentreCode;
            report.ReportParameters["ExamCentreName"].Value = ecm.ExamCentreName;

            var sm = SubjectMasterDB.Get(subid);
            report.ReportParameters["SubId"].Value = sm.Id;
            report.ReportParameters["Subject"].Value = sm.Subject;
            report.ReportParameters["Pattern"].Value = sm.Pattern;
            report.ReportParameters["YearSemester"].Value = sm.YearSemester;
            report.ReportParameters["PaperCode"].Value = sm.PaperCode;
            report.ReportParameters["PaperName"].Value = sm.PaperName;
            report.ReportParameters["ExamDate"].Value = sm.ExamDate;
            report.ReportParameters["ExamTime"].Value = sm.ExamTime;

            var em = EventMasterDB.Get(sm.EId);
            report.ReportParameters["ExamMonthYear"].Value = em.ExamMonthYear;

            var cl = CourseListDB.Get(examcentrecode, subid, examcentrecode);
            if (cl.IsSubmit)
            {
                if(cl.IsReset)
                    report.ReportParameters["SubmitStatus"].Value = $"Resubmitted on {cl.UpdatedOn.ToString("dd/MM/yyyy hh:mm:ss tt")}";
                else
                    report.ReportParameters["SubmitStatus"].Value = $"Submitted on {cl.UpdatedOn.ToString("dd/MM/yyyy hh:mm:ss tt")}";
            }
            else
                report.ReportParameters["SubmitStatus"].Value = string.Empty;

            report.DataSource = FormDataDB.Get(examcentrecode, subid);
            Download(report, $"Statement_{Guid.NewGuid()}.pdf");
        }
        public void AtStatement(int subid)
        {
            PrintAtStatement(subid, EcCode);
        }

        public void PrintAtPackingSlip(int subid, string examcentrecode)
        {
            var report = ToTelerikReport(path_Packingslip);

            var ecm = ExamCentreMasterDB.Get(examcentrecode);
            report.ReportParameters["ExamCentreCode"].Value = ecm.ExamCentreCode;
            report.ReportParameters["ExamCentreName"].Value = ecm.ExamCentreName;

            var sm = SubjectMasterDB.Get(subid);
            report.ReportParameters["SubId"].Value = sm.Id;
            report.ReportParameters["Subject"].Value = sm.Subject;
            report.ReportParameters["Pattern"].Value = sm.Pattern;
            report.ReportParameters["YearSemester"].Value = sm.YearSemester;
            report.ReportParameters["PaperCode"].Value = sm.PaperCode;
            report.ReportParameters["PaperName"].Value = sm.PaperName;
            report.ReportParameters["ExamDate"].Value = sm.ExamDate;
            report.ReportParameters["ExamTime"].Value = sm.ExamTime;

            var em = EventMasterDB.Get(sm.EId);
            report.ReportParameters["ExamMonthYear"].Value = em.ExamMonthYear;

            var cl = CourseListDB.Get(examcentrecode, subid, examcentrecode);
            if (cl.IsSubmit)
            {
                if (cl.IsReset)
                    report.ReportParameters["SubmitStatus"].Value = $"Resubmitted on {cl.UpdatedOn.ToString("dd/MM/yyyy hh:mm:ss tt")}";
                else
                    report.ReportParameters["SubmitStatus"].Value = $"Submitted on {cl.UpdatedOn.ToString("dd/MM/yyyy hh:mm:ss tt")}";
            }
            else
                report.ReportParameters["SubmitStatus"].Value = string.Empty;

            report.DataSource = PackingSlipCountDB.Get(examcentrecode, subid);
            Download(report, $"PackingSlip_{Guid.NewGuid()}.pdf");
        }
        public void AtPackingSlip(int subid)
        {
            PrintAtPackingSlip(subid, EcCode);
        }

        public void PrintAtCoverSlip(SubjectMaster sm, string examcentrecode)
        {
            var report = ToTelerikReport(path_CoverSlip);

            var ecm = ExamCentreMasterDB.Get(examcentrecode);
            report.ReportParameters["ExamCentreCode"].Value = ecm.ExamCentreCode;
            report.ReportParameters["ExamCentreName"].Value = ecm.ExamCentreName;

            var coverslip = CoverSlipDB.Get(examcentrecode, sm.EId, sm.ExamDate, sm.ExamTime);
            report.ReportParameters["CoverSlip"].Value = coverslip.Id;

            var em = EventMasterDB.Get(coverslip.EId);
            report.ReportParameters["EId"].Value = em.Id;
            report.ReportParameters["ExamMonthYear"].Value = em.ExamMonthYear;

            report.ReportParameters["ExamDate"].Value = coverslip.ExamDate;
            report.ReportParameters["ExamTime"].Value = coverslip.ExamTime;

            report.DataSource = PackingSlipCountDB.Get(coverslip.ExamCentreCode, coverslip.EId, coverslip.ExamDate, coverslip.ExamTime);

            Download(report, $"CoverSlip_{Guid.NewGuid()}.pdf");
        }

        public void AtCoverSlip(string esub)
        {
            var sm = SubjectMaster.ToObject(esub);
            PrintAtCoverSlip(sm, EcCode);
        }


    }
}