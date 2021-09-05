using cumin_api.Models;
using cumin_api.Others;
//using cumin_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using cumin_api.Services.v2;
using System.Security.Claims;

namespace cumin_api.Middlewares {
    public class WebsocketHandlerMiddleware {
        private readonly RequestDelegate next;
        private readonly Services.StateManager stateManager;

        private const uint BODY_BUFFER_LENGTH = 2048;

        public WebsocketHandlerMiddleware(RequestDelegate next, Services.StateManager stateManager) {
            this.next = next;
            this.stateManager = stateManager;
        }

        public async Task InvokeAsync(HttpContext context, TokenHelper tokenHelper, UserService userService) {
            // if not a websocket request, forward it in the request pipeline
            if (context.WebSockets.IsWebSocketRequest == false) {
                await next(context);
                return;
            }

            using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync()) {
                int userId = -1;
                byte[] buffer = new byte[BODY_BUFFER_LENGTH];
                // read the token from the body
                // int bytesRead = await context.Request.Body.ReadAsync(buffer, 0, (int)BODY_BUFFER_LENGTH);
                JsonElement json;
                await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                try {
                    // convert bytes array to jsonElement
                    json = Utf8BytesToPoco<JsonElement>(buffer);
                } catch (JsonException e) {
                    // inalvid json
                    Console.WriteLine(e.Message);
                    return;
                }

                // get the token string from json
                string token;
                try {
                    token = json.GetProperty("token").GetString();
                } catch (KeyNotFoundException e) {
                    Console.WriteLine(e.Message);
                    return;
                }

                string remoteIp = context.Connection.RemoteIpAddress.ToString();
                userId = ValidateToken(tokenHelper, userService, token, remoteIp);

                // this is just a patch could be bad in long run
                var user_ = userService.FindById(userId);
                // now ^ has active project id
                // ==============================================

                if (user_ == null || userId == -1) {
                    return;
                }

                // check if the user is already connected to the server through websocket
                if (stateManager.Users.ContainsKey(userId) == false) {
                    // add the user to dictinary
                    stateManager.Users.Add(userId, new UserState());
                }

                // close the old connection
                if (stateManager.Users[userId].isActive() == true) {
                    await stateManager.Users[userId].closeConnectionAsync("Initiating new connection on your request!");
                }

                // save this socket in the stateManager
                // add new socket to userState
                stateManager.Users[userId].addSocket(webSocket);

                if (user_.ActiveProjectId.HasValue) {
                    stateManager.AddUserToProject(userId, user_.ActiveProjectId.Value);
                }

                await HandleClientRequest(webSocket, userId);
            }
        }
        
        private int ValidateToken(TokenHelper tokenHelper, UserService userService, string token, string remoteIp) {
            int userId = -1;
            IEnumerable<Claim> claims = tokenHelper.ExtractClaimsFromToken(token);
            if (Int32.TryParse(claims.DefaultIfEmpty(null).FirstOrDefault(x => x.Type == "userId").Value, out userId)) {
                // check if the claimed user exists
                if (userService.FindById(userId) == null)
                    userId = -1;

                // check if the websocket req originates from the claim ip address
                //string claimedRemoteIp = claims.DefaultIfEmpty(null).FirstOrDefault(x => x.Type == "remoteIp").Value;
                //if (claimedRemoteIp == null || claimedRemoteIp != remoteIp) {
                //    return -1;
                //}
                //else {
                //    return userId;
                //}
            }

            return userId;
        }

        private async Task HandleClientRequest(WebSocket webSocket, int userId) {
            Console.WriteLine($"User: {userId} connected!");
            var buffer = new byte[1024 * 4];
            try {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                while (!result.CloseStatus.HasValue) {
                    // keep listening
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }

                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                // just some insightful info
                Console.WriteLine($"User: {userId} closed the connection with status: {result.CloseStatus.Value}");
            } catch { }
        }

        private T Utf8BytesToPoco<T>(byte[] buffer) {
            var utf8Reader = new Utf8JsonReader(buffer);
            T data = JsonSerializer.Deserialize<T>(ref utf8Reader);
            return data;
        }
    }
}
