using System;
using System.Configuration;
using System.ServiceProcess;
using log4net;
using Microsoft.Owin.Hosting;

namespace UPSShare.Master
{
    public partial class MasterService : ServiceBase
    {
        public MasterService()
        {
            InitializeComponent();
            BaseAddress = ConfigurationManager.AppSettings["baseAddress"] ?? "http://*:5000";
        }

        public string BaseAddress { get; private set; }

        protected override void OnStart(string[] args)
        {
            _log.Debug("Starting UPS Master ");
            Start(args);
        }

        protected override void OnStop()
        {
            _webApi.Dispose();
            _log.Debug("Stopping UPS Master ");
        }

        internal void Start(string[] args)
        {
            // see: http://www.asp.net/web-api/overview/hosting-aspnet-web-api/use-owin-to-self-host-web-api
            _webApi = WebApp.Start<WebApi.Startup>(url: BaseAddress);
            _log.Debug($"Starting WebAPi on '{BaseAddress}'");

        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            _log.Debug($"OnPowerStatus '{powerStatus}'");
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
        protected override void OnShutdown()
        {
            _log.Debug($"OnShutdown");
            base.OnShutdown();
        }
        IDisposable _webApi;
        ILog        _log    = LogManager.GetLogger(typeof(MasterService));
    }
}
