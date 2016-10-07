using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

namespace UPSShare.Master.WebApi
{
    using System.Net.WebSockets;
    // see: http://aspnet.codeplex.com/sourcecontrol/latest#Samples/Katana/WebSocketSample/WebSocketServer/Startup.cs
    using WebSocketAccept = Action<IDictionary<string, object>, Func<IDictionary<string, object>, Task>>;
    using WebSocketCloseAsync = Func<int, string, CancellationToken, Task>;
    using WebSocketReceiveAsync = Func<ArraySegment<byte>, CancellationToken, Task<Tuple<int, bool, int>>>;
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
        }

        Task UpgradeToWebSockets(IOwinContext context, Func<Task> next)
        {
            WebSocketAccept accept = context.Get<WebSocketAccept>("websocket.Accept");
            if (accept == null || context.Request.Path.Value != "/ws")
            {
                // Not a websocket request
                return next();
            }

            accept(null, WebSocketPushUPSShare);

            return Task.FromResult<object>(null);
        }

        async Task WebSocketPushUPSShare(IDictionary<string, object> websocketContext)
        {
            Console.WriteLine("client connected");
            string clientKey = String.Empty;
            try {
                var webSocketsContext   = (HttpListenerWebSocketContext)    websocketContext["System.Net.WebSockets.WebSocketContext"];
                clientKey               = webSocketsContext.SecWebSocketKey;

                Console.WriteLine($"Client SecWebSocketKey = {clientKey}");

                var sendAsync           = (WebSocketSendAsync)              websocketContext["websocket.SendAsync"];
                var receiveAsync        = (WebSocketReceiveAsync)           websocketContext["websocket.ReceiveAsync"];
                var closeAsync          = (WebSocketCloseAsync)             websocketContext["websocket.CloseAsync"];
                var callCancelled       = (CancellationToken)               websocketContext["websocket.CallCancelled"];
                var buffer              = new byte[1024];
                var received            = await receiveAsync(new ArraySegment<byte>(buffer), callCancelled);
                object status;

                while (!websocketContext.TryGetValue("websocket.ClientCloseStatus", out status) || (int) status == 0) {
                    // Echo anything we receive
                    var type            = received.Item1;
                    var endOfMessage    = received.Item2;
                    var count           = received.Item3;

                    await sendAsync(new ArraySegment<byte>(buffer, 0, count), type, endOfMessage, callCancelled);

                    received = await receiveAsync(new ArraySegment<byte>(buffer), callCancelled);
                }
                object clientCloseDescription;
                websocketContext.TryGetValue("websocket.ClientCloseDescription", out clientCloseDescription);
                Console.WriteLine($"Client ended connection with status {(WebSocketCloseStatus) status} and description {clientCloseDescription}. disconnecting client");

                await closeAsync((int) status, (string) clientCloseDescription, callCancelled);
            } finally {
                Console.WriteLine($"client {clientKey} disconnected");
            }
        }
    }
}
