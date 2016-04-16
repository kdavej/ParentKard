using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ParentKardData.DataAccess;
using ParentKardData.DataObjects.Session;

namespace DefaultMVC4.Models.Attributes
{
    public class DCOAuthorize : AuthorizeAttribute
    {
        string CookieName = ConfigurationManager.AppSettings["COOKIENAME"].ToString();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isAuthorized = false;

            if (httpContext.Request.Cookies != null && httpContext.Request.Cookies[CookieName] != null)
            {
                isAuthorized = Authorize(httpContext.Request.Cookies[CookieName].Value);
            }

            return isAuthorized;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.Request.Cookies[CookieName] != null)
                {
                    SessionAccess sa = new SessionAccess();
                    DCOSession ds = sa.GetSessionByID(HttpContext.Current.Request.Cookies[CookieName].Value);
                    DCOUser du = (DCOUser)ds.SessionData["user"];

                    filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "login", action = "index" }));

                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "login", action = "index" }));
                }
            }
        }

        public bool Authorize(string SessionID)
        {
            SessionAccess sa = new SessionAccess();
            bool found = sa.ValidateSession(SessionID);

            if (found)
            {
                DCOSession ds = sa.GetSessionByID(SessionID);
                if (ds.SessionData != null && ds.SessionData.ContainsKey("user"))
                {
                    DCOUser cu = (DCOUser)ds.SessionData["user"];
                    if (cu.UserID > 0)
                    {
                        found = true;
                    }
                    else
                    {
                        found = false;
                    }

                }
            }
            return found;
        }

        
    }
}