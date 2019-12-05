using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace TitleLeader.Custom
{
    public class TitleLeaderDatabaseConnection
    {
        private SqlConnection getDBConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TitleLeaderDBConnection"].ConnectionString);
        public SqlConnection GetDBConnection
        {
            get
            {
                return getDBConnection;
            }
        }
    }
}