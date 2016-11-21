using CommandLine;
using CommandLine.Text;

namespace UPSShare.Slave
{
    public class Options
    {
        [Option('c', "Command", DefaultValue = false)]
        public bool AsCommand { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
