using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace SafeConnect
{
    class SafeConnectService : ServiceBase
    {
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
            Console.WriteLine("Starting...");
            new Thread(RunMessagePump).Start();
        }

        protected override void OnStop()
        {
            StopService();
        }

        public void StopService()
        {
            Console.WriteLine("Stopping...");
            Application.Exit();
        }

        void RunMessagePump()
        {
            Console.WriteLine("Starting message pump");
            Application.Run(new HiddenForm());
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            Console.WriteLine(powerStatus + DateTime.Now.ToShortTimeString());
            return base.OnPowerEvent(powerStatus);
        }
    }
}
