using System;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using SafeConnectCore;

namespace SafeConnect
{
    class SafeConnectService : ServiceBase
    {
        public SafeConnectService()
        {
            //this.CanHandlePowerEvent = true;
            this.ServiceName = "SafeConnect";
        }

        protected override void OnStart(string[] args)
        {
            StartService();
        }

        public void StartService()
        {
            Logger.Log("SafeConnectService: Starting...");
            new Thread(RunMessagePump).Start();

            SafeConnectUpdater.MakeWebRequest();
            SafeConnectUpdater.SetupTimer();
        }

        protected override void OnStop()
        {
            StopService();
        }

        public void StopService()
        {
            Logger.Log("SafeConnectService: Stopping...");
            SafeConnectUpdater.StopTimer();
            Application.Exit();
        }

        void RunMessagePump()
        {
            Logger.Log("SafeConnectService: Starting message pump");
            Application.Run(new HiddenForm());
        }

        //protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        //{
        //    Logger.Log("SafeConnectService: " + powerStatus + DateTime.Now.ToString("g"));
        //    return base.OnPowerEvent(powerStatus);
        //}
    }
}
