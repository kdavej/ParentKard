using DefaultMVC4.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DefaultMVC4.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View("/views/pages/Home.aspx");
        }

        public ActionResult About(string id)
        {
            return View("/views/pages/About.aspx");
        }

        public ActionResult Contact()
        {
            return View("/views/pages/Contact.asxp");
        }
    }
}
