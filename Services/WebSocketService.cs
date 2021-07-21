using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace cumin_api.Services {
    public interface IWebSocketService {
        public Task Connect();
        public Task Send(Object data);
    }
    public class WebSocketService: IWebSocketService {
        public ClientWebSocket socket { get; set; }

        public WebSocketService() {
            socket = new ClientWebSocket();
        }

        public async Task Connect() {
            await socket.ConnectAsync(new Uri("ws://localhost:21941/server?token=fake"), CancellationToken.None);
        }

        public async Task Send(Object data) {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            Console.WriteLine($"this is is the object {json}");
            try {
                await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(json)), WebSocketMessageType.Text, true, CancellationToken.None);
            } catch { }
        }
    }
}
