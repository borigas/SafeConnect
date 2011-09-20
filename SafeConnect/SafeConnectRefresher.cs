﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Timers;
using System.Configuration;
using System.Drawing;
using Microsoft.Win32;

namespace SafeConnect
{
    class SafeConnectRefresher
    {
        private const string URL_KEY = "URL";
        private const string USER_AGENT_KEY = "UserAgent";
        private const string SLEEP_TIME_KEY = "SleepMillis";

        private static Timer timer;

        public static void MakeWebRequest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings[URL_KEY]);
            request.UserAgent = ConfigurationManager.AppSettings[USER_AGENT_KEY];
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            SafeConnectService.Log("SafeConnectRefresher: Status: " + response.StatusCode);
            response.Close();
        }

        public static void SetupTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = double.Parse(ConfigurationManager.AppSettings[SLEEP_TIME_KEY]);
            timer.Elapsed += (source, args) =>
            {
                SafeConnectService.Log("SafeConnectRefresher: Timer elapsed at " + args.SignalTime);
                MakeWebRequest();
            };
            timer.Start();
        }

        public static void StopTimer()
        {
            timer.Stop();
        }

        public static void HandlePowerEvent(PowerModeChangedEventArgs args)
        {
            if (args.Mode == PowerModes.Resume)
            {
                MakeWebRequest();
            }
        }
    }
}