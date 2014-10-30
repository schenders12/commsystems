using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace systems.Helpers
{
    public static class SSI
    {
        public static IHtmlString ServerSideInclude(this HtmlHelper helper, string serverPath)
        {
            var filePath = HttpContext.Current.Server.MapPath(serverPath);
            var markup = File.ReadAllText(filePath);
            return new HtmlString(markup);
        }
        // HTML Helper method to add 'active' class to Bootstrap nav bar elements
        public static string IsSelected(this HtmlHelper html, string pageId = null, string controller = null, string action = null)
        {
            string cssClass = "active";
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];
            string currentPageId = (string)html.ViewContext.RouteData.Values["pageid"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction && pageId == currentPageId ?
                cssClass : String.Empty;
        }

    }

}