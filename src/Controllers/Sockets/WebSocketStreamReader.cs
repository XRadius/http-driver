using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets
{
    // TODO: Gracefully handle messages exceeding buffer size.
    public class WebSocketStreamReader : IDisposable
    {
        private readonly ConcurrentQueue<ArraySegment<byte>> _queue;
        private readonly WebSocket _socket;
        private readonly CancellationToken _token;

        #region Constructors

        private WebSocketStreamReader(WebSocket socket, CancellationToken token)
        {
            _queue = new ConcurrentQueue<ArraySegment<byte>>();
            _socket = socket;
            _token = token;
        }

        public static WebSocketStreamReader Create(WebSocket socket, CancellationToken token)
        {
            var reader = new WebSocketStreamReader(socket, token);
            Task.Factory.StartNew(reader.RunAsync, TaskCreationOptions.LongRunning);
            return reader;
        }

        #endregion

        #region Methods

        public bool TryDequeue(out ArraySegment<byte> result)
        {
            return _queue.TryDequeue(out result);
        }

        private async Task RunAsync()
        {
            while (!_token.IsCancellationRequested)
            {
                try
                {
                    var buffer = new byte[8192];
                    var count = await _socket.ReceiveMessageAsync(buffer, _token);
                    if (count != 0) _queue.Enqueue(new ArraySegment<byte>(buffer, 0, count));
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (WebSocketException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    if (Debugger.IsAttached) Console.WriteLine(ex);
                }
            }
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}