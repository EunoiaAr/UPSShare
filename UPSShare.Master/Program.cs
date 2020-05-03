using System;
using System.ServiceProcess;
using CommandLine;

namespace UPSShare.Master
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options)) {
                return;
            }
            var service = new MasterService();
            if (!options.AsCommand) {
                var ServicesToRun = new ServiceBase[] {
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
