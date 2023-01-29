using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using WebUI.Database;
using WebUI.Extensions;
using WebUI.Models.Shared;

namespace WebUI.Models.At
{
    public class SubjectCount : SubjectMaster, ICountTable
    {
        public int Total { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Malpractice { get; set; }
        public int Buffer { get; set; }
        public int Pending { get; set; }
        public int IOPending { get; set; }
        public int ScanPending { get; set; }
    }

    public class SubjectCountDB
    {
        public static List<SubjectCount> Get(string examcentrecode, int eid)
        {
            using (var db = Connection.Create())
            {
                return db.Query<SubjectCount>("at.dashboard_sessioncount @examcentrecode, @eid", new { examcentrecode, eid }).ToList();
            }
        }
        public static List<SubjectCount> Get(string examcentrecode, int eid, string examdate, string examtime)
        {
            using (var db = Connection.Create())
            {
                return db.Query<SubjectCount>("at.dashboard_subjectcount @examcentrecode, @eid, @examdate, @examtime", new { examcentrecode, eid, examdate, examtime }).ToList();
            }
        }

        public static List<SubjectCount> Ad_Get(int eid)
        {
            using (var db = Connection.Create())
            {
                return db.Query<SubjectCount>("at.ad_dashboard_sessioncount @eid", new { eid }).ToList();
            }
        }
        public static List<SubjectCount> Ad_Get(int eid, string examdate, string examtime)
        {
            using (var db = Connection.Create())
            {
                return db.Query<SubjectCount>("at.ad_dashboard_subjectcount @eid, @examdate, @examtime", new { eid, examdate, examtime }).ToList();
            }
        }
    }
}