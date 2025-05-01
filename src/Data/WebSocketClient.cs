using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MelonLoader;
using AmongUsHacks.Main;

namespace AmongUsHacks.Data
{
    public static class WebSocketClient
    {
        private static readonly System.Uri RPCWebSocketURI = new System.Uri("wss://amongusvr.sleepie.dev:443/rpc");
        private static ClientWebSocket? webSocket;
        private static CancellationTokenSource? cancellationTokenSource;
        private static Task? receiveTask;

        public static async void ConnectWebSocket()
        {
            try
            {
                webSocket = new ClientWebSocket();
                cancellationTokenSource = new CancellationTokenSource();

                webSocket.Options.SetRequestHeader("User-Agent", Config.UserAgent);

                await webSocket.ConnectAsync(RPCWebSocketURI, CancellationToken.None);
                MelonLogger.Msg("Connected to RPC WebSocket!");

                receiveTask = ReceiveMessages();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"RPC WebSocket connection error: {ex.Message}");
            }
        }

        private static async Task ReceiveMessages()
        {
            byte[] buffer = new byte[1024];
            while (webSocket != null && cancellationTokenSource != null && webSocket.State == WebSocketState.Open) // i put null checks for the websocket and cancellation token source here bcuz my ide was yelling at me
            {
                try
                {
                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new System.ArraySegment<byte>(buffer), cancellationTokenSource.Token);
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    MelonLogger.Msg($"Received: {message}");
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"RPC WebSocket receive error: {ex.Message}");
                    break;
                }
            }
            Reconnect();
        }

        private static async void Reconnect()
        {
            MelonLogger.Warning("Reconnecting to RPC WebSocket...");
            await Task.Delay(5000);
            ConnectWebSocket();
        }
    }
}
