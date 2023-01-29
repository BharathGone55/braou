using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Database;
using WebUI.Extensions;
using WebUI.Models.Shared;

namespace WebUI.Models.At
{
    public class EventCount : EventMaster, ICountTable
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

    public class EventCountDB
    {
        public static List<EventCount> Get(string examcentrecode)
        {
            using (var db = Connection.Create())
            {
                return db.Query<EventCount>("at.dashboard_eventcount @examcentrecode", new { examcentrecode }).ToList();
            }
        }

        public static List<EventCount> Ad_Get(string username)
        {
            using (var db = Connection.Create())
            {
                return db.Query<EventCount>("at.ad_dashboard_eventcount @username", new { username }).ToList();
            }
        }
    }
}