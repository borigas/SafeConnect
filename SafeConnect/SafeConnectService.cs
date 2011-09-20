using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace SafeConnect
{
    class SafeConnectService : ServiceBase
    {
        public static readonly string LOG_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SafeConnect", "SafeConnectLog.txt");

        public SafeConnectService()
        {
            this.CanHandlePowerEvent = true;
            this.ServiceName = "SafeConnect";
        }

        protected override void OnStart(string[] args)
        {
            StartService();
        }

        public void StartService()
        {
            SafeConnectService.Log("SafeConnectService: Starting...");
            new Thread(RunMessagePump).Start();

            SafeConnectRefresher.MakeWebRequest();
            SafeConnectRefresher.SetupTimer();
        }

        protected override void OnStop()
        {
            StopService();
        }

        public void StopService()
        {
            SafeConnectService.Log("SafeConnectService: Stopping...");
            SafeConnectRefresher.StopTimer();
            Application.Exit();
        }

        void RunMessagePump()
        {
            SafeConnectService.Log("SafeConnectService: Starting message pump");
            Application.Run(new HiddenForm());
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            SafeConnectService.Log("SafeConnectService: " + powerStatus + DateTime.Now.ToShortTimeString());
            return base.OnPowerEvent(powerStatus);
        }

        public static void Log(string message)
        {
            using (StreamWriter logfile = new StreamWriter(LOG_PATH))
            {
                logfile.WriteLine(message);
            }
        }
    }
}
