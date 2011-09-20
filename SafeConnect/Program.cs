using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace SafeConnect
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				ServiceBase[] ServicesToRun;
				ServicesToRun = new ServiceBase[] 
			{ 
				new SafeConnectService() 
			};
				ServiceBase.Run(ServicesToRun);
			}
			else
			{
				SafeConnectService scs = new SafeConnectService();
				scs.StartService();

				Console.WriteLine("Press any key to exit");
				Console.ReadKey();

				scs.StopService();
			}
		}
	}
}
