using System;
using System.Net.WebSockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;

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
            _stopEvent = new ManualResetEvent(false);
            RunSlave().ConfigureAwait(false);
        }

        public void Start(string[] args)
        {
            OnStart(args);
        }
        protected override void OnStop()
        {
            _stopEvent.Set();
        }

        public async Task RunSlave()
        {
            var url         = "ws://localhost:5000/ws";
            var disconnect  = false;

            while (!_stopEvent.WaitOne(1)) {
                var websocket   = new ClientWebSocket();
                try {
                    _log.Debug($"connecting to {url}");
                    await websocket.ConnectAsync(new Uri(url), CancellationToken.None);

                    while (!_stopEvent.WaitOne(1) && !disconnect) {
                        var message = await ReceiveMessage(websocket);

                        switch (message) {
                            case "master shutdown":
                                disconnect = true;
                                break;
                            case "power off":
                                HibernateOrShutdown();
                                break;
                            default:
                                break;
                        }
                    }
                } catch (Exception e) {
                    _log.Warn(e);
                } finally {
                    if (websocket != null && websocket.State == WebSocketState.Open)
                        await websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye!", CancellationToken.None);
                }
            }
        }

        private void HibernateOrShutdown()
        {
            // hibernate if enabled otherwise shutdown immediately!!!
        }

        async Task<string> ReceiveMessage(ClientWebSocket socket)
        {
            byte[] incomingData = new byte[1024];
            WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(incomingData), CancellationToken.None);

            var receivedMessage = string.Empty;
            if (result.CloseStatus.HasValue) {
                Console.WriteLine("Closed; Status: " + result.CloseStatus + ", " + result.CloseStatusDescription);
                return "close";
            } else {
                receivedMessage = Encoding.UTF8.GetString(incomingData, 0, result.Count);
                Console.WriteLine("Received message: " + receivedMessage);
                return receivedMessage;
            }
        }

        ManualResetEvent    _stopEvent;
        ILog                _log        = LogManager.GetLogger(nameof(SlaveService));
    }
}
