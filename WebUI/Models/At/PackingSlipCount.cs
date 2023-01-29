using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Database;
using WebUI.Models.Shared;

namespace WebUI.Models.At
{
    public class PackingSlipCount: SubjectMaster, ICountTable
    {
        public string PackingSlip { get; set; }
        public int Total { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Malpractice { get; set; }
        public int Buffer { get; set; }
        public int Pending { get; set; }
        public int IOPending { get; set; }
        public int ScanPending { get; set; }
    }

    public class PackingSlipCountDB
    {
        public static PackingSlipCount Get(string examcentrecode, int subid)
        {
            using (var db = Connection.Create())
            {
                return db.Query<PackingSlipCount>("at.courselist_packingslipcount_examcentrecode_subid @examcentrecode, @subid", new { examcentrecode, subid }).SingleOrDefault();
            }
        }

        public static List<PackingSlipCount> Get(string examcentrecode, int eid, string examdate, string examtime)
        {
            using (var db = Connection.Create())
            {
                return db.Query<PackingSlipCount>("at.courselist_packingslipcount_examcentrecode_eid_examdate_examtime @examcentrecode, @eid, @examdate, @examtime", new { examcentrecode, eid, examdate, examtime }).ToList();
            }
        }
    }
}