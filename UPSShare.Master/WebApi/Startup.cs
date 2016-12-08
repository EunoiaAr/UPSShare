using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using Microsoft.Owin;
using Owin;

namespace UPSShare.Master.WebApi
{
    // see: http://aspnet.codeplex.com/sourcecontrol/latest#Samples/Katana/WebSocketSample/WebSocketServer/Startup.cs
    using WebSocketAccept = Action<IDictionary<string, object>, Func<IDictionary<string, object>, Task>>;
    using WebSocketCloseAsync = Func<int, string, CancellationToken, Task>;
    using WebSocketSendAsync = Func<ArraySegment<byte>, int, bool, CancellationToken, Task>;

    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host.
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
            appBuilder.Use(UpgradeToWebSockets);
            if (!ThreadPool.QueueUserWorkItem(ConsumeEventsProcess)) {
                _log.Warn("Consumer Events Process could not be started");
            }
            _acceptedClients = new ConcurrentDictionary<string, IDictionary<string, object>>();
        }

        async void ConsumeEventsProcess(object state)
        {
            while (true) {
                try {
                    string message;

                    if (MasterService.UpdatesQueue.TryDequeue(out message)) {
                        await SendMessageToClients(message);
                    }
                } catch (Exception e) {
                    _log.Warn(e);
                }
            }
        }

        async Task SendMessageToClients(string message)
        {
            foreach (var client in _acceptedClients.Keys.ToArray()) {
                IDictionary<string, object> websocketContext;

                // if still exists
                if (_acceptedClients.TryGetValue(client, out websocketContext)) {
                    var closeAsync      = (WebSocketCloseAsync) websocketContext["websocket.CloseAsync"];
                    var callCancelled   = (CancellationToken)   websocketContext["websocket.CallCancelled"];
                    object status;

                    // check if it's not closed
                    if (!websocketContext.TryGetValue("websocket.ClientCloseStatus", out status) || (int) status == 0) {
                        var sendAsync       = (WebSocketSendAsync)  websocketContext["websocket.SendAsync"];
                        var messageBytes    = Encoding.UTF8.GetBytes(message);

                        // send the message
                        await sendAsync(new ArraySegment<byte>(messageBytes, 0, messageBytes.Length), (int) WebSocketMessageType.Text, true, callCancelled);
                    } else {
                        object clientCloseDescription;
                        if (!websocketContext.TryGetValue("websocket.ClientCloseDescription", out clientCloseDescription)) {
                            _log.Warn($"could not get close description for client '{client}' and status '{status}'");
                        } else {
                            _log.Debug($"Client ended connection with status {(WebSocketCloseStatus) status} and description {clientCloseDescription}. disconnecting client");
                        }
                        await closeAsync((int) status, (string) clientCloseDescription, callCancelled);
                        _log.Debug($"client {client} disconnected");

                        // remove it if closed
                        _acceptedClients.TryRemove(client, out websocketContext);
                    }
                }
            }
        }

        Task UpgradeToWebSockets(IOwinContext context, Func<Task> next)
        {
            var accept = context.Get<WebSocketAccept>("websocket.Accept");
            if (accept == null || context.Request.Path.Value != "/ws") {
                // Not a websocket request
                return next();
            }

            accept(null, WebSocketPushUPSShare);

            return Task.FromResult<object>(null);
        }

        async Task WebSocketPushUPSShare(IDictionary<string, object> websocketContext)
        {
            var clientKey           = string.Empty;
            await Task.Run(() => {
                try {
                    _log.Debug("client connected");
                    var webSocketsContext   = (HttpListenerWebSocketContext)    websocketContext["System.Net.WebSockets.WebSocketContext"];
                    clientKey = webSocketsContext.SecWebSocketKey;

                    _log.Debug($"Client SecWebSocketKey = {clientKey}");
                    if (!_acceptedClients.TryAdd(clientKey, websocketContext)) {
                        _log.Warn($"client '{clientKey}' could not be added to the accepted client dictionary");
                    }

                } catch (Exception e) {
                    _log.Warn($"client key = {clientKey} --> {e}");
                }
            });
        }

        readonly    ILog                                                        _log    = LogManager.GetLogger(typeof(Startup));
                    ConcurrentDictionary<string, IDictionary<string, object>>   _acceptedClients;
    }
}
