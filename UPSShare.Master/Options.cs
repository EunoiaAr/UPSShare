using CommandLine;

namespace UPSShare.Master
{
    public class Options
    {
        [Option('c', "Command", DefaultValue = false)]
        public bool AsCommand { get; set; }
    }
}
