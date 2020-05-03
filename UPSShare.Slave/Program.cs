using System;
using System.ServiceProcess;

namespace UPSShare.Slave
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var options = new Options();
            var service = new SlaveService();
            var isValid = CommandLine.Parser.Default.ParseArgumentsStrict(args, options);
            if (!isValid) {
                return;
            }

            if (!options.AsCommand) {
                var servicesToRun = new ServiceBase[] {
                    service
                };
                ServiceBase.Run(servicesToRun);
            } else {
                service.Start(args);
                service.RunSlave().ConfigureAwait(false);
                Console.WriteLine("Press <ENTER> to finish");
                Console.ReadLine();
                service.Stop();
            }
        }
    }
}
