using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Database;
using WebUI.Extensions;

namespace WebUI.Models.Shared
{
    public class EventMaster
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string ExamMonthYear { get; set; }
        public bool CanView { get; set; }
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return this.Serialize().Encrypt();
        }
        public static EventMaster ToObject(string encryptedString)
        {
            return encryptedString.Decrypt().DeSerialize<EventMaster>();
        }
    }

    public class EventMasterDB
    {
        public static List<EventMaster> Get()
        {
            using (var db = Connection.Create())
            {
                return db.Query<EventMaster>("select id, eventname, exammonthyear, canview, isactive from at.eventmaster").ToList();
            }
        }

        public static EventMaster Get(int id)
        {
            using (var db = Connection.Create())
            {
                return db.Query<EventMaster>("select id, eventname, exammonthyear, canview, isactive from at.eventmaster where id=@id", new { id }).SingleOrDefault();
            }
        }
    }
}