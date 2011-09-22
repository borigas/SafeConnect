using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace SafeConnectCore
{
    public class Logger
    {
        private const string LOG_ENABLED_KEY = "LogEnabled";
        private static readonly string LOG_DIRECTORY = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SafeConnect");
        private static readonly string LOG_FILE_NANE = "SafeConnectLog.txt";
        private static readonly string LOG_PATH = Path.Combine(LOG_DIRECTORY, LOG_FILE_NANE);
        private static readonly long MAX_FILE_SIZE = 1024 * 20;

        public static void Log(string message)
        {
            bool logEnabled = Boolean.Parse(ConfigurationManager.AppSettings[LOG_ENABLED_KEY]);
            if (logEnabled)
            {
                if (!Directory.Exists(LOG_DIRECTORY))
                {
                    Directory.CreateDirectory(LOG_DIRECTORY);
                }

                FileInfo logFileInfo = new FileInfo(LOG_PATH);
                if (logFileInfo.Exists && logFileInfo.Length > MAX_FILE_SIZE)
                {
                    logFileInfo.Delete();
                }

                using (StreamWriter logfile = new StreamWriter(LOG_PATH, true))
                {
                    logfile.WriteLine(System.DateTime.Now.ToString("M/d/yy h:mm:ss") + " - " + message);
                    logfile.Close();
                }
            }
        }
    }
}
