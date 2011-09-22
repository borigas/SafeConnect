using System.Net;
using System.Timers;
using System.Configuration;
using Microsoft.Win32;

namespace SafeConnectCore
{
    public class SafeConnectUpdater
    {
        private const string URL_KEY = "URL";
        private const string USER_AGENT_KEY = "UserAgent";
        private const string SLEEP_TIME_KEY = "SleepMillis";

        private static Timer timer;

        public static bool MakeWebRequest()
        {
            bool success = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings[URL_KEY]);
                request.UserAgent = ConfigurationManager.AppSettings[USER_AGENT_KEY];
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                success = response.StatusCode == HttpStatusCode.OK;
                response.Close();
            }
            catch { }
            Logger.Log("Connection " + (success ? "succeeded" : "failed"));
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