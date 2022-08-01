using System.Net.WebSockets;

namespace HttpDriver.Controllers.Sockets.Extensions
{
    public static class WebSocketExtensions
    {
        #region Statics

        public static async Task<int> ReceiveMessageAsync(this WebSocket socket, byte[] buffer, CancellationToken token)
        {
            var count = buffer.Length;
            var offset = 0;

            while (true)
            {
                token.ThrowIfCancellationRequested();
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer, offset, count), token);
                offset += result.Count;
                count -= result.Count;
                if (result.EndOfMessage) return offset;
            }
        }

        #endregion
    }
}