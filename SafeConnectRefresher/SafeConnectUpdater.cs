using System.Net;
using System.Timers;
using System.Configuration;
using Microsoft.Win32;
using System.IO;
using System;

namespace SafeConnectCore
{
    public class SafeConnectUpdater
    {
        private const string URL_KEY = "URL";
        private const string USER_AGENT_KEY = "UserAgent";
        private const string SLEEP_TIME_KEY = "SleepMillis";
        private const string MAX_REPEAT_COUNT_KEY = "MaxRepeatCount";

        private static Timer timer;

        public static bool MakeWebRequest()
        {
            int i = 0;
            int maxRepeats = Int32.Parse(ConfigurationManager.AppSettings[MAX_REPEAT_COUNT_KEY]);
            bool success = false;
            while (!success && i < maxRepeats)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings[URL_KEY]);
                    request.UserAgent = ConfigurationManager.AppSettings[USER_AGENT_KEY];
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        Logger.Log("Server: " + response.Server + "; Status: " + response.StatusCode + "; Uri: " +
                                   response.ResponseUri.AbsoluteUri + "; Host: " + response.ResponseUri.Host + 
                                   "; Desc: " + response.StatusDescription);
                        success = response.StatusCode == HttpStatusCode.OK && 
                            response.ResponseUri.AbsoluteUri == ConfigurationManager.AppSettings[URL_KEY];
                        response.Close();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log("Exception: " + ex.Message + Environment.NewLine + ex.StackTrace);
                }
                Logger.Log("Connection " + (success ? "succeeded" : "failed") + " on attempt " + (i + 1));
                i++;
            }
            return success;
        }

        public static void SetupTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = double.Parse(ConfigurationManager.AppSettings[SLEEP_TIME_KEY]);
            timer.Elapsed += (source, args) =>
            {
                Logger.Log("Timer elapsed");
                MakeWebRequest();
            };
            timer.Start();
        }

        public static void StopTimer()
        {
            timer.Stop();
        }

        public static void ResetTimer()
        {
            timer.Stop();
            timer.Start();
        }

        public static void HandlePowerEvent(PowerModeChangedEventArgs args)
        {
            if (args.Mode == PowerModes.Resume)
            {
                Logger.Log("Refreshing on system resume");
                MakeWebRequest();
            }
        }
    }
}