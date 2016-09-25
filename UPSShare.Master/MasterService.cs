using System.ServiceProcess;

namespace UPSShare.Master
{
    public partial class MasterService : ServiceBase
    {
        public MasterService()
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
