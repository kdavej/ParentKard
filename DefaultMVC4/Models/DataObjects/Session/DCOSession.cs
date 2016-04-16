using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultMVC4.Models.DataObjects.Session
{
    public class DCOSession
    {
        public string SessionID { get; set; }
        public SessionInformation SessionData { get; set; }
    }
}