using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DefaultMVC4.Models.Helpers
{
    public class ViewHelper
    {
        public string RenderPartialView(Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public string BoolAsString(bool Bool)
        {
            if (Bool == true) { return "Yes"; } else { return "No"; }
        }
                
        
    }
}