using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace ClientManagementWebforms.Infrastructure
{
    public static class Logger
    {
        public static void Info(string message)
        {
            WriteLog("INFO", message, null);
        }

        public static void Error(string message, Exception exception)
        {
            WriteLog("ERROR", message, exception);
        }

        private static void WriteLog(string level, string message, Exception exception)
        {
            try
            {
                var directorySetting = ConfigurationManager.AppSettings["LogDirectory"];
                if (string.IsNullOrWhiteSpace(directorySetting))
                {
                    directorySetting = "~/logs";
                }

                string basePath = null;

                if (HttpContext.Current != null)
                {
                    basePath = HttpContext.Current.Server.MapPath(directorySetting);
                }
                else
                {
                    basePath = HostingEnvironment.MapPath(directorySetting);
                }

                if (string.IsNullOrEmpty(basePath))
                {
                    return;
                }

                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                var fileName = DateTime.UtcNow.ToString("yyyy-MM-dd") + ".log";
                var filePath = Path.Combine(basePath, fileName);

                var builder = new StringBuilder();
                builder.AppendFormat("[{0:u}] [{1}] {2}", DateTime.UtcNow, level, message ?? string.Empty);
                builder.AppendLine();

                if (exception != null)
                {
                    builder.AppendLine(exception.ToString());
                }

                File.AppendAllText(filePath, builder.ToString());
            }
            catch
            {
                // Swallow all logging exceptions to avoid impacting the application.
            }
        }
    }
}

