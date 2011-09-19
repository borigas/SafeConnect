using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Timers;

namespace SafeConnect
{
    class Program
    {
        static void Main(string[] args)
        {
            MakeWebRequest();
            SetupTimer();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void MakeWebRequest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.google.com");
            request.UserAgent = "USER_AGENT=Opera/9.00 (Nintendo Wii; U;;1038-58;Wii Shop Channel/1.0;en)";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine("Status: " + response.StatusCode);
            response.Close();
        }

        private static void SetupTimer()
        {
            Timer timer = new Timer();
            timer.Interval = 5000;
            timer.Elapsed += (source, args) =>
            {
                Console.WriteLine("Timer elapsed at " + args.SignalTime);
                MakeWebRequest();
            };
            timer.Start();
        }
    }
}