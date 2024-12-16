using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Market.Constants;
using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Market.Controllers
{
    [ApiController]
    public class WebSocketController(IUserService userService, IChatService chatService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IChatService _chatService = chatService;
        private static ConcurrentDictionary<string, WebSocket> _activeConnections = new();
        
        [SwaggerIgnore]
        private async void PrintRequestDetails()
        {
            var request = HttpContext.Request;

            Console.WriteLine("Method: " + request.Method);
            Console.WriteLine("Scheme: " + request.Scheme);
            Console.WriteLine("Host: " + request.Host);
            Console.WriteLine("Path: " + request.Path);
            Console.WriteLine("QueryString: " + request.QueryString);
            Console.WriteLine("Headers:");
            foreach (var header in request.Headers)
            {
                Console.WriteLine($"{header.Key}: {header.Value}");
            }
            Console.WriteLine("Body:");
            using (var reader = new StreamReader(request.Body))
            {
                var body = await reader.ReadToEndAsync();
                Console.WriteLine(body);
            }

            Console.WriteLine("Plain:\n");
            Console.WriteLine(request.ToString());
        }
        [SwaggerIgnore]
        [Route("/ws")]
        public async Task UserOnline()
        {
            // PrintRequestDetails();
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                // if (HttpContext.WebSockets.IsWebSocketRequest)
                // {
                //     if (string.IsNullOrEmpty(token))
                //     {
                //         HttpContext.Response.StatusCode = 400;
                //         return;
                //     }
                //     var sourceUser = _userService.ValidateWebsocketToken(token);
                //     if (sourceUser == null)
                //     {
                //         HttpContext.Response.StatusCode = 401;
                //         return;
                //     }
                //     var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                //     _activeConnections.TryAdd(sourceUser, socket);
                //     await MainEventLoop(socket, sourceUser);
                // }
                var sourceUser = _userService.GetCurrentUser();
                if (sourceUser == null)
                {
                    HttpContext.Response.StatusCode = 401;
                    return;
                }
                var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _activeConnections.TryAdd(sourceUser.Id, socket);
                await MainEventLoop(socket, sourceUser.Id);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        [SwaggerIgnore]
        private async Task MainEventLoop(WebSocket socket, string sourceUserId)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult receiveResult = null!;
            try
            {
                do
                {
                    receiveResult = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (receiveResult.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                        Console.WriteLine("Received: " + message);
                        var chatMessage = JsonSerializer.Deserialize<ChatMessageWsObj>(message, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        Console.WriteLine("ChatMessage: " + chatMessage);
                        var chat = _chatService.GetChatList(chatMessage!.ChatListId);
                        var targetUserId = chat.FromUserId == sourceUserId ? chat.ToUserId : chat.FromUserId;
                        if (chatMessage != null)
                        {
                            if (!chatMessage.FromUserId.Equals(sourceUserId) || !chatMessage.ToUserId.Equals(targetUserId))
                            {
                                await socket.SendAsync(
                                    new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(Result.Fail(ResultCode.ValidateError)))),
                                    WebSocketMessageType.Text,
                                    true,
                                    CancellationToken.None
                                );
                                return;
                            }
                            _chatService.SendMessageSimple(chatMessage);
                            await BroadcastMessage(chatMessage);
                        }
                        else
                        {
                            await socket.SendAsync(
                                new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(Result.Fail(ResultCode.Fail)))),
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None
                            );
                        }
                    }
                    else if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the WebSocket client", CancellationToken.None);
                    }
                } while (!receiveResult.CloseStatus.HasValue);
            }
            finally
            {
                _activeConnections.TryRemove(sourceUserId, out _);
                await socket.CloseAsync(receiveResult.CloseStatus ?? WebSocketCloseStatus.NormalClosure, receiveResult.CloseStatusDescription, CancellationToken.None);
            }
        }

        [SwaggerIgnore]
        private static async Task BroadcastMessage(ChatMessageWsObj chatMessage)
        {
            _activeConnections.TryGetValue(chatMessage.ToUserId, out var socket);
            if (socket != null && socket.State == WebSocketState.Open)
            {
                var message = JsonSerializer.Serialize(Result<ChatMessageWsObj>.Ok(chatMessage));
                var buffer = Encoding.UTF8.GetBytes(message);
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            } else if (socket != null)
            {
                _activeConnections.TryRemove(chatMessage.ToUserId, out _);
            }

        }
    }
    
}