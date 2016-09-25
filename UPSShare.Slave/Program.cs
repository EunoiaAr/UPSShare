using System.ServiceProcess;

namespace UPSShare.Slave
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
                new SlaveService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
