using System;
using System.Configuration;
using System.ServiceProcess;
using Microsoft.Owin.Hosting;

namespace UPSShare.Master
{
    public partial class MasterService : ServiceBase
    {
        public MasterService()
        {
            InitializeComponent();
            BaseAddress = ConfigurationManager.AppSettings["baseAddress"] ?? "http://localhost:8080";
        }

        public string BaseAddress { get; private set; }

        protected override void OnStart(string[] args)
        {
            Start(args);
        }

        protected override void OnStop()
        {
            _webApi.Dispose();
        }

        internal void Start(string[] args)
        {
            // see: http://www.asp.net/web-api/overview/hosting-aspnet-web-api/use-owin-to-self-host-web-api
            _webApi = WebApp.Start<WebApi.Startup>(url: BaseAddress);
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            switch (powerStatus) {
                case PowerBroadcastStatus.BatteryLow:
                    break;
                case PowerBroadcastStatus.OemEvent:
                    break;
                case PowerBroadcastStatus.PowerStatusChange:
                    break;
                case PowerBroadcastStatus.QuerySuspend:
                    break;
                case PowerBroadcastStatus.QuerySuspendFailed:
                    break;
                case PowerBroadcastStatus.ResumeAutomatic:
                    break;
                case PowerBroadcastStatus.ResumeCritical:
                    break;
                case PowerBroadcastStatus.ResumeSuspend:
                    break;
                case PowerBroadcastStatus.Suspend:
                    break;
                default:
                    break;
            }
            return base.OnPowerEvent(powerStatus);
        }

        IDisposable _webApi;
    }
}
