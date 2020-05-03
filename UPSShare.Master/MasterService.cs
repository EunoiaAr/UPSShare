using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Net;
using System.Reflection;
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
            BaseAddress = ConfigurationManager.AppSettings["baseAddress"] ?? "http://*:5000/";
        }

        public string BaseAddress { get; private set; }

        public static ConcurrentQueue<string> UpdatesQueue => _updatesQueue;

        internal void Start(string[] args)
        {
            // see: http://www.asp.net/web-api/overview/hosting-aspnet-web-api/use-owin-to-self-host-web-api
            try {
                _log.Debug($"Starting server at {BaseAddress}");
                _webApi = WebApp.Start<WebApi.Startup>(url: BaseAddress);
            } catch (TargetInvocationException tiex) when (tiex.InnerException is HttpListenerException &&
                                                           tiex.InnerException.Message == "Access is denied") {
                var message = $@"
Make BaseAccess ({BaseAddress}) available to the impersonating user ({Environment.UserDomainName}\{Environment.UserName})
   see: https://serverfault.com/a/811249/544";
                _log.Error(message);
                throw new ApplicationException(message);
            }
            _log.Debug($"Started WebAPi on '{BaseAddress}'");

        }

        protected override void OnStart(string[] args)
        {
            _log.Debug("Starting UPS Master");
            Start(args);
        }

        protected override void OnStop()
        {
            _updatesQueue.Enqueue("OnStop");
            _log.Debug("Stopping UPS Master");
            _webApi.Dispose();
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            _log.Debug($"OnPowerStatus '{powerStatus}'");
            _updatesQueue.Enqueue(powerStatus.ToString());
            return base.OnPowerEvent(powerStatus);
        }

        protected override void OnShutdown()
        {
            _log.Debug(OnShutdownMessage);
            _updatesQueue.Enqueue(OnShutdownMessage);
            base.OnShutdown();
        }

        const string OnShutdownMessage = "OnShutdown";
        static
        readonly ConcurrentQueue<string> _updatesQueue = new ConcurrentQueue<string>();
        readonly ILog _log = LogManager.GetLogger(typeof(MasterService));
        IDisposable _webApi;
    }
}
