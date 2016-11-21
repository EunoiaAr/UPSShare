using System;
using System.ServiceProcess;
using CommandLine;

namespace UPSShare.Master
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options)) {
                return;
            }
            var service = new MasterService();
            if (!options.AsCommand) {
                ServiceBase[] ServicesToRun = new ServiceBase[] {
                    service
                };
                ServiceBase.Run(ServicesToRun);
            } else {
                service.Start(null);
                Console.WriteLine("Press <ENTER> to finish");
                Console.ReadLine();
                service.Stop();
            }
        }
    }
}
