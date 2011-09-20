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
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller processInstaller;

        public SafeConnectInstaller()
        {
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            // Service will run under system account
            processInstaller.Account = ServiceAccount.LocalSystem;

            // Service will have Start Type of Manual
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            serviceInstaller.ServiceName = "SafeConnect";

            processInstaller.AfterInstall += new InstallEventHandler(processInstaller_AfterInstall);

            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }

        private void processInstaller_AfterInstall(object sender, InstallEventArgs args)
        {

        }
    }
}
