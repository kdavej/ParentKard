using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DefaultMVC4.Models.DataAccess;
using DefaultMVC4.Models.DataObjects.Session;

namespace DefaultMVC4.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View("/views/pages/Login.aspx");
        }

        [HttpPost]
        public ActionResult Submit()
        {
            if (Request.Form["UserName"] != null && Request.Form["Password"] != null)
            {
                string UserName = Request.Form["UserName"].ToString();
                string Password = Request.Form["Password"].ToString();
                SessionAccess sa = new SessionAccess();
                DCOSession dcs = sa.LoginUser(UserName, Password);
                DCOUser du = (DCOUser)dcs.SessionData["user"];

                if (du.UserID > 0)
                {
                    return Redirect("LOGGEDIN");
                }
                else
                {
                    return Redirect("/login");
                }
            }
            else
            {
                return Redirect("/login");
            }
        }
    }
}
