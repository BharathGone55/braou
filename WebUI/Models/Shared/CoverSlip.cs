using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Database;

namespace WebUI.Models.Shared
{
    public class CoverSlip
    {
        public int Id { get; set; }
        public string ExamCentreCode { get; set; }
        public int EId { get; set; }
        public string ExamDate { get; set; }
        public string ExamTime { get; set; }
    }

    public class CoverSlipDB
    {
        public static CoverSlip Get(string examcentrecode, int eid, string examdate, string examtime)
        {
            using (var db = Connection.Create())
            {
                return db.Query<CoverSlip>("at.coverslip_find_examcentrecode_eid_examdate_examtime @examcentrecode,@eid,@examdate,@examtime", new { examcentrecode, eid, examdate, examtime }).SingleOrDefault();
            }
        }       
    }
}