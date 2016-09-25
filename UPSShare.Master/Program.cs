using System.ServiceProcess;

namespace UPSShare.Master
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MasterService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
