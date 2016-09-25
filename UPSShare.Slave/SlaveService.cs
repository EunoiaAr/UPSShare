using System.ServiceProcess;

namespace UPSShare.Slave
{
    public partial class SlaveService : ServiceBase
    {
        public SlaveService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
