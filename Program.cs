using System;
using System.ServiceProcess;

namespace DatabaseBackUpService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (Environment.UserInteractive)
            {
                DatabaseBackUpService service = new DatabaseBackUpService();
                service.StartInConsole();
            }
            else
            {

                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new DatabaseBackUpService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
