using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Database;
using WebUI.Extensions;

namespace WebUI.Models.Shared
{
    public class SubjectMaster
    {
        public int Id { get; set; }
        public int EId { get; set; }
        public string ExamDate { get; set; }
        public string ExamTime { get; set; }
        public string Pattern { get; set; }
        public string YearSemester { get; set; }
        public string Subject { get; set; }
        public string PaperCode { get; set; }
        public string PaperName { get; set; }
        public DateTime StartTime { get; set; }
        public override string ToString()
        {
            return this.Serialize().Encrypt();
        }
        public static SubjectMaster ToObject(string encryptedString)
        {
            return encryptedString.Decrypt().DeSerialize<SubjectMaster>();
        }
    }

    public class SubjectMasterDB
    {
        public static List<SubjectMaster> Find_EventId(int eid)
        {
            using (var db = Connection.Create())
            {
                return db.Query<SubjectMaster>("select id, eid, examdate, examtime, subject, pattern, yearsemester, papercode, papername, starttime from at.subjectmaster where eid=@eid", new { eid}).ToList();
            }
        }

        public static SubjectMaster Get(int id)
        {
            using (var db = Connection.Create())
            {
                return db.Query<SubjectMaster>("select id, eid, examdate, examtime, subject, pattern, yearsemester, papercode, papername, starttime from at.subjectmaster where id=@id", new { id }).SingleOrDefault();
            }
        }
    }
}