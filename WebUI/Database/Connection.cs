using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebUI.Database
{
    public static class Connection
    {
        static readonly string connectonString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public static IDbConnection Create()
        {
            return new SqlConnection(connectonString);
        } 
    }


}
