using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace ClientManagementWebforms
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var logDirectorySetting = ConfigurationManager.AppSettings["LogDirectory"];
            if (string.IsNullOrWhiteSpace(logDirectorySetting))
            {
                logDirectorySetting = "~/logs";
            }

            var physicalPath = Server.MapPath(logDirectorySetting);
            if (!Directory.Exists(physicalPath))
            {
                Directory.CreateDirectory(physicalPath);
            }
        }
    }
}
