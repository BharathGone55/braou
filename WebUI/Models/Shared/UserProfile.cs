using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Database;

namespace WebUI.Models.Shared
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string ExamCentreCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    public class UserProfileDB
    {
        public static UserProfile GetAtAdmin(string username)
        {
            using (var db = Connection.Create())
            {
                return db.Query<UserProfile>("select id, username, displayname, examcentrecode, createdon, createdby from userprofile where username=@username", new { username }).SingleOrDefault();
            }
        }
        public static UserProfile GetAtUser(string username)
        {
            UserProfile userprofile = null;
            var ecm = ExamCentreMasterDB.Get(username);
            if (ecm != null)
            {
                userprofile = new UserProfile() {
                    UserName = username,
                    ExamCentreCode = ecm.ExamCentreCode,
                    DisplayName = ecm.ExamCentreName
                };
            }
            return userprofile;
        }
    }

    public class UserProfileBO
    {
        public static UserProfile Instance
        {
            get
            {
                if (HttpContext.Current.Session["User"] == null)
                    HttpContext.Current.Session["User"] = new UserProfile();
                return (UserProfile)HttpContext.Current.Session["User"];
            }
            set { HttpContext.Current.Session["User"] = value; }
        }
    }
}