using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultMVC4.Models.DataObjects.Session
{
    public class DCOUser
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public bool IsHidden { get; set; }
        public bool ChangePassword { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public string FullName { get {
            return FirstName + " " + LastName;
        } }
    }

}