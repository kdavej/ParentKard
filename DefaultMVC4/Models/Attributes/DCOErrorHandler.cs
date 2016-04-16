using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DefaultMVC4.Controllers;
using ParentKardData.DataAccess;
using ParentKardData.DataObjects.Session;
using DefaultMVC4.Models.Helpers;
using DefaultMVC4.Models.Services.Email;
using Newtonsoft.Json;

namespace DefaultMVC4.Models.Attributes
{
    public class DCOErrorHandler : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
                return;

            var statusCode = (int)HttpStatusCode.InternalServerError;
            if (filterContext.Exception is HttpException)
            {
                statusCode = (filterContext.Exception as HttpException).GetHttpCode();
            }
            else if (filterContext.Exception is UnauthorizedAccessException)
            {
                //to prevent login prompt in IIS
                // which will appear when returning 401.
                statusCode = (int)HttpStatusCode.Forbidden;
            }

            Exchange email = new Exchange();
            email.UserName = ConfigurationManager.AppSettings["EXCHANGEUSER"].ToString();
            email.Password = ConfigurationManager.AppSettings["EXCHANGEPASSWORD"].ToString();
            email.ServiceURL = ConfigurationManager.AppSettings["EXCHANGESERVER"].ToString();

            string CookieName = ConfigurationManager.AppSettings["COOKIENAME"].ToString();

            SessionAccess sa = new SessionAccess();
            DCOUser du = new DCOUser();
            if (HttpContext.Current.Request.Cookies[CookieName] != null)
            {
                DCOSession ds = sa.GetSessionByID(HttpContext.Current.Request.Cookies[CookieName].Value);
                du = (DCOUser)ds.SessionData["user"];
            }

            ViewHelper h = new ViewHelper();
            ErrorController ec = new ErrorController();
            ec.ViewData["error"] = filterContext.Exception.ToString();

            ec.ViewData["stacktrace"] = filterContext.Exception.ToString();
            ec.ViewData["UserDetails"] = JsonConvert.SerializeObject(du);

            Dictionary<string, object> FORM = new Dictionary<string, object>();
            if (HttpContext.Current.Request.Form != null)
            {

                foreach (string s in HttpContext.Current.Request.Form)
                {
                    FORM.Add(s, HttpContext.Current.Request.Form[s]);
                }
            }
            ec.ViewData["RequestScope"] = JsonConvert.SerializeObject(FORM);
            ec.ViewData["URL"] = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            ec.ControllerContext = filterContext.Controller.ControllerContext;
            var result = CreateActionResult(filterContext, statusCode);
            email.SendMessage(h.RenderPartialView(ec, "/views/shared/email/errormessage.aspx", ec.ViewData), "care-support@unitedcarcare.com", "care-support@unitedcarcare.com", "CARE UNITED Error: " + filterContext.Exception.Message);
            filterContext.Result = result;

            // Prepare the response code.
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = statusCode;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        protected virtual ActionResult CreateActionResult(ExceptionContext filterContext, int statusCode)
        {
            var ctx = new ControllerContext(filterContext.RequestContext, filterContext.Controller);
            var statusCodeName = ((HttpStatusCode)statusCode).ToString();

            var viewName = "/views/shared/error.aspx";

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            var result = new ViewResult
            {
                ViewName = viewName,
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
            };
            result.ViewBag.StatusCode = statusCode;
            return result;
        }
    }
}