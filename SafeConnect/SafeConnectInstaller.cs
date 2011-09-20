using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace SafeConnect
{
    [RunInstaller(true)]
    public partial class SafeConnectInstaller : System.Configuration.Install.Installer
    {
        private const string SERVICE_NAME = "SafeConnect";

        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller processInstaller;

        public SafeConnectInstaller()
        {
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            // Service will run under system account
            processInstaller.Account = ServiceAccount.User;

            // Service will have Start Type of Manual
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            serviceInstaller.ServiceName = SERVICE_NAME;

            processInstaller.AfterInstall += new InstallEventHandler(processInstaller_AfterInstall);

            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }

        private void processInstaller_AfterInstall(object sender, InstallEventArgs args)
        {
            ServiceController service = new ServiceController(SERVICE_NAME);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(10000);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                // ...
            }
        }
    }
}
