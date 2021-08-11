using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class UserState {
        private WebSocket webSocket;
        public UserState() {

        }

        public bool isActive() {
            if (webSocket == null)
                return false;
            return webSocket.State == WebSocketState.Open;
        }

        public async Task closeConnectionAsync(string closeDesc) {
            if (webSocket != null)
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, closeDesc, System.Threading.CancellationToken.None);
        }

        public void addSocket (WebSocket webSocket) {
            this.webSocket = webSocket;
        }

        public async Task sendData<T> (T data) {
            byte[] msg = JsonSerializer.SerializeToUtf8Bytes(data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await webSocket.SendAsync(msg, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
