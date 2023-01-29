using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Database;
using WebUI.Models.Shared;

namespace WebUI.Models.At
{
    public class DForm : SubjectMaster
    {
        public string IHTNO { get; set; }      
        public string StickerNo { get; set; }
        public int PageNo { get; set; }
        public int PageRecSlno { get; set; }
        public int RoomNo { get; set; }        
    }

    class DFormDB
    {
        public static List<DForm> Get(int eid, string examcentrecode, string examdate = null, string examtime = null, bool islatecomer = false, bool isbuffer = false)
        {
            using (var db = Connection.Create())
            {
                return db.Query<DForm>("at.formdata_dform @eid,@examcentrecode,@examdate,@examtime,@islatecomer,@isbuffer", new { eid, examcentrecode, examdate, examtime, islatecomer, isbuffer }).ToList();
            }
        }
    }
}