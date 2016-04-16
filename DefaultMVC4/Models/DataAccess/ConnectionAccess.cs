using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DefaultMVC4.Models.DataAccess
{
    public abstract class ConnectionAccess
    {
        public SqlConnection SQLConnection;
        public SqlCommand SQLCommand;

        public ConnectionAccess()
        {
            SQLConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLDBConnection"].ToString());
        }
    }
}