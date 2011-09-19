using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Timers;
using System.Configuration;

namespace SafeConnect
{
    class Program
    {
        private const string URL_KEY = "URL";
        private const string USER_AGENT_KEY = "UserAgent";
        private const string SLEEP_TIME_KEY = "SleepMillis";

        static void Main(string[] args)
        {
            MakeWebRequest();
            SetupTimer();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void MakeWebRequest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings[URL_KEY]);
            request.UserAgent = ConfigurationManager.AppSettings[USER_AGENT_KEY];
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine("Status: " + response.StatusCode);
            response.Close();
        }

        private static void SetupTimer()
        {
            Timer timer = new Timer();
            timer.Interval = double.Parse(ConfigurationManager.AppSettings[SLEEP_TIME_KEY]);
            timer.Elapsed += (source, args) =>
            {
                Console.WriteLine("Timer elapsed at " + args.SignalTime);
                MakeWebRequest();
            };
            timer.Start();
        }
    }
}