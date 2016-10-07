using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

namespace UPSShare.Master.WebApi
{
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
            if (accept == null)
            {
                // Not a websocket request
                return next();
            }

            accept(null, WebSocketEcho);

            return Task.FromResult<object>(null);

        }

        async Task WebSocketEcho(IDictionary<string, object> websocketContext)
        {
            var sendAsync       = (WebSocketSendAsync)   websocketContext["websocket.SendAsync"];
            var receiveAsync    = (WebSocketReceiveAsync)websocketContext["websocket.ReceiveAsync"];
            var closeAsync      = (WebSocketCloseAsync)  websocketContext["websocket.CloseAsync"];
            var callCancelled   = (CancellationToken)    websocketContext["websocket.CallCancelled"];
            var buffer          = new byte[1024];
            var received        = await receiveAsync(new ArraySegment<byte>(buffer), callCancelled);
            object status;

            while (!websocketContext.TryGetValue("websocket.ClientCloseStatus", out status) || (int) status == 0) {
                // Echo anything we receive
                await sendAsync(new ArraySegment<byte>(buffer, 0, received.Item3), received.Item1, received.Item2, callCancelled);

                received = await receiveAsync(new ArraySegment<byte>(buffer), callCancelled);
            }

            await closeAsync((int) websocketContext["websocket.ClientCloseStatus"], (string) websocketContext["websocket.ClientCloseDescription"], callCancelled);
        }
    }
}
