using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Database;
using WebUI.Extensions;

namespace WebUI.Models.Shared
{
    public class FormData
    {
        public int Id { get; set; }
        public int SubId { get; set; }
        public string ExamCentreCode { get; set; }
        public string ExamDate { get; set; }
        public string ExamTime { get; set; }
        public string Pattern { get; set; }
        public string YearSemester { get; set; }
        public string Subject { get; set; }
        public string PaperCode { get; set; }
        public string PaperName { get; set; }
        public string IHTNO { get; set; }
        public string SName { get; set; }
        public string Medium { get; set; }
        public string AtStatus { get; set; }
        public string StickerNo { get; set; }
        public bool IsBuffer { get; set; }
        public bool IsLocked { get; set; }
        public string ScanStatus { get; set; }
        public string DForm { get; set; }

        public static FormData ToObject(string encryptedString)
        {
            return encryptedString.Decrypt().DeSerialize<FormData>();
        }

        public string Encrypt()
        {
            return this.Serialize().Encrypt();
        }
    }

    class FormDataDB
    {
        public static List<FormData> Get(string examcentrecode, int subid)
        {
            using (var db = Connection.Create())
            {
                return db.Query<FormData>("at.formdata_find_examcentrecode_subid @examcentrecode, @subid", new { examcentrecode, subid }).ToList();
            }
        }

        internal static List<FormData> Get_IHTNO(string ihtno, int eid)
        {
            using (var db = Connection.Create())
            {
                return db.Query<FormData>("at.formdata_find_ihtno_eid @ihtno,@eid", new { ihtno, eid }).ToList();
            }
        }

        public static bool UpdateAtStatus(int id, string atstatus, string updatedby)
        {
            using (var db = Connection.Create())
            {
                return db.Execute("at.formdata_updateatstatus_id @id, @atstatus, @updatedby", new { id, atstatus, updatedby }) >= 1;
            }
        }

        public static bool SubmitAtStatus(string examcentrecode, int subid, string updatedby)
        {
            using (var db = Connection.Create())
            {
                return db.Execute("at.formdata_submitatstatus_examcentrecode_subid @examcentrecode, @subid, @updatedby", new { examcentrecode, subid, updatedby }) >= 1;
            }
        }

        public static bool ResetSubmitAtStatus(string examcentrecode, int subid, string updatedby)
        {
            using (var db = Connection.Create())
            {
                return db.Execute("at.formdata_resetsubmitatstatus_examcentrecode_subid @examcentrecode, @subid, @updatedby", new { examcentrecode, subid, updatedby }) >= 1;
            }
        }

        internal static bool ValidateBuffer(string ihtno, int eid)
        {
            using (var db = Connection.Create())
            {
                return Convert.ToBoolean(db.ExecuteScalar("at.formdata_validatebuffer_ihtno_eid @ihtno,@eid", new { ihtno, eid }));
            }
        }

       
        internal static bool Addbuffer(string examcentrecode, int subid, string ihtno, string sname, string medium, string updatedby)
        {
            using (var db = Connection.Create())
            {
                return db.Execute("at.formdata_addbuffer @examcentrecode,@subid,@ihtno,@sname,@medium,@updatedby", new { examcentrecode, subid, ihtno, sname, medium, updatedby }) == 1;
            }
        }

        internal static bool DeleteBuffer(int id, string updatedby)
        {
            using (var db = Connection.Create())
            {
                return db.Execute("at.formdata_deletebuffer_id @id,@updatedby", new { id, updatedby }) == 1;
            }
        }

        public static List<FormData> Ad_Get(string examcentrecode, int subid)
        {
            using (var db = Connection.Create())
            {
                return db.Query<FormData>("at.ad_dashboard_formdata @examcentrecode, @subid", new { examcentrecode, subid }).ToList();
            }
        }

        public static List<FormData> Ad_Query(string username, string query, int eid, string examdate, string examtime, string subid, string examcentrecode)
        {
            using (var db = Connection.Create())
            {
                return db.Query<FormData>("at.ad_dashboard_formdata_query @query, @username, @eid, @examdate, @examtime, @subid, @examcentrecode", new { query, username, eid, examdate, examtime, subid, examcentrecode }).ToList();
            }
        }
    }
}