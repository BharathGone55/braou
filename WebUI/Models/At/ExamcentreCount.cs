using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Database;
using WebUI.Models.Shared;

namespace WebUI.Models.At
{
    public class ExamcentreCount: ExamCentreMaster, ICountTable
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

    public class ExamcentreCountDB
    {
        public static List<ExamcentreCount> Ad_Get(int subid)
        {
            using (var db = Connection.Create())
            {
                return db.Query<ExamcentreCount>("at.ad_dashboard_examcentrecount @subid", new { subid }).ToList();
            }
        }
    }
}