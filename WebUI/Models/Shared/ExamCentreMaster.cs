using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Database;
using WebUI.Extensions;

namespace WebUI.Models.Shared
{
    public class ExamCentreMaster
    {
        public int Id { get; set; }
        public string ExamCentreCode { get; set; }
        public string ExamCentreName { get; set; }

        public override string ToString()
        {
            return this.Serialize().Encrypt();
        }
        public static ExamCentreMaster ToObject(string encryptedString)
        {
            return encryptedString.Decrypt().DeSerialize<ExamCentreMaster>();
        }
    }

    public class ExamCentreMasterDB
    {
        public static List<ExamCentreMaster> Get()
        {
            using (var db = Connection.Create())
            {
                return db.Query<ExamCentreMaster>("select id, examcentrecode, examcentrename from at.examcentremaster").ToList();
            }
        }

        public static ExamCentreMaster Get(string examcentrecode)
        {
            using (var db = Connection.Create())
            {
                return db.Query<ExamCentreMaster>("select id, examcentrecode, examcentrename from at.examcentremaster where examcentrecode=@examcentrecode", new { examcentrecode }).SingleOrDefault();
            }
        }        
    }
}