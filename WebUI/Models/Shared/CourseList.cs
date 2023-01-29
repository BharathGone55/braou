using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Database;
using WebUI.Extensions;

namespace WebUI.Models.Shared
{
    public class CourseList
    {
        public int Id { get; set; }
        public int SubId { get; set; }
        public string ExamCentreCode { get; set; }
        public bool IsSubmit { get; set; }
        public bool IsReset { get; set; }
        public bool CanEdit { get; set; }
        public DateTime UpdatedOn { get; set; }

        public override string ToString()
        {
            return this.Serialize().Encrypt();
        }
        public static CourseList ToObject(string encryptedString)
        {
            return encryptedString.Decrypt().DeSerialize<CourseList>();
        }
    }

    public class CourseListDB
    {
        public static CourseList Get(string examcentrecode, int subid, string updatedby)
        {
            using (var db = Connection.Create())
            {
                return db.Query<CourseList>("at.courselist_find_examcentrecode_subid @examcentrecode,@subid,@updatedby", new { examcentrecode, subid, updatedby }).SingleOrDefault();
            }
        }
    }


}