using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using ParentKardData.DataObjects.Session;

namespace ParentKardData.DataAccess
{
    public class SessionAccess : ConnectionAccess
    {
        public void CreateSession(string SessionID, MemoryStream SessionData)
        {
            SQLCommand = new SqlCommand("DCO_CreateSession", SQLConnection);
            SQLCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SQLCommand.Parameters.Add(new SqlParameter("@SessionID", SessionID));
            SQLCommand.Parameters.Add(new SqlParameter("@SessionData", SqlDbType.VarBinary, Int32.MaxValue));
            SQLCommand.Parameters["@SessionData"].Value = SessionData.ToArray();

            SQLConnection.Open();
            SQLCommand.ExecuteNonQuery();
            SQLConnection.Close();
        }

        public void DestroySession(string SessionID)
        {
            SQLCommand = new SqlCommand("DCO_DestroySession", SQLConnection);
            SQLCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SQLCommand.Parameters.Add(new SqlParameter("@SessionID", SessionID));

            SQLConnection.Open();
            SQLCommand.ExecuteNonQuery();
            SQLConnection.Close();
        }

        public DCOSession GetSessionByID(string SessionID)
        {
            DataTable dt = new DataTable();

            SQLCommand = new SqlCommand("WEB_GetWebSessionByID", SQLConnection);
            SQLCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SQLCommand.Parameters.Add(new SqlParameter("@SessionID", SessionID));

            SQLConnection.Open();
            SqlDataReader SQLReader = SQLCommand.ExecuteReader();
            dt.Load(SQLReader);
            SQLConnection.Close();

            DCOSession dcs = new DCOSession();
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                dcs.SessionID = dr["SessionID"].ToString();
                dcs.SessionData = (SessionInformation)ByteArrayToObject((byte[])dr["SessionData"]);
            }
            return dcs;
        }

        public DCOSession LoginUser(string username, string password)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream memstream = new MemoryStream();
            DCOSession dcs = new DCOSession();
            dcs.SessionID = "DCO-" + System.Guid.NewGuid().ToString();

            SQLCommand = new SqlCommand("DCO_LoginUser", SQLConnection);
            SQLCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SQLCommand.Parameters.Add(new SqlParameter("@UserName", username));
            SQLCommand.Parameters.Add(new SqlParameter("@Password", password));
            DataTable dt = new DataTable();

            SQLConnection.Open();
            SqlDataReader SQLReader = SQLCommand.ExecuteReader();
            dt.Load(SQLReader);
            SQLConnection.Close();

            DCOUser du = new DCOUser();
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                du.UserID = Convert.ToInt32(dr["UserID"].ToString());
                du.UserName = dr["UserName"].ToString();
                du.FirstName = dr["FirstName"].ToString();
                du.LastName = dr["LastName"].ToString();
                du.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
                du.IsHidden = Convert.ToBoolean(dr["IsHidden"].ToString());
                du.ChangePassword = Convert.ToBoolean(dr["ChangePassword"].ToString());
                du.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
                du.LastUpdateDate = DateTime.Parse(dr["LastUpdateDate"].ToString());
                dcs.SessionData.Add("user", du);
            }

            
            bf.Serialize(memstream, dcs.SessionData);

            if (du.UserID > 0)
            {
                CreateSession(dcs.SessionID, memstream);
            }
            return dcs;
        }

        public void UpdateSessionData(string SessionID, object SessionData)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream memstream = new MemoryStream();

            bf.Serialize(memstream, SessionData);

            SQLCommand = new SqlCommand("DCO_UpdateSessionData", SQLConnection);
            SQLCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SQLCommand.Parameters.Add(new SqlParameter("@SessionID", SessionID));
            SQLCommand.Parameters.Add(new SqlParameter("@SessionData", SqlDbType.VarBinary, Int32.MaxValue));

            SQLCommand.Parameters["@SessionData"].Value = memstream.ToArray();

            SQLConnection.Open();
            SQLCommand.ExecuteNonQuery();
            SQLConnection.Close();
        }

        public bool ValidateSession(string SessionID)
        {
            bool valid = false;
            DataTable dt = new DataTable();

            SQLCommand = new SqlCommand("DCO_ValidateSession", SQLConnection);

            SQLCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SQLCommand.Parameters.Add(new SqlParameter("@SessionID", SessionID));

            SQLConnection.Open();
            SqlDataReader SQLReader = SQLCommand.ExecuteReader();
            dt.Load(SQLReader);
            SQLConnection.Close();

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["SessionID"].ToString() != "")
                {
                    valid = true;
                }
            }

            return valid;
        }


        //Helper
        private object ByteArrayToObject(byte[] _ByteArray)
        {
            try
            {
                // convert byte array to memory stream
                System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream(_ByteArray);

                // create new BinaryFormatter
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _BinaryFormatter
                            = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                // set memory stream position to starting point
                _MemoryStream.Position = 0;


                // Deserializes a stream into an object graph and return as a object.

                return _BinaryFormatter.Deserialize(_MemoryStream);

            }
            catch 
            { }
            return null;
        }
    }
}