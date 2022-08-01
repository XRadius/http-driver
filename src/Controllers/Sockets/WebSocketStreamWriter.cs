using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;

namespace HttpDriver.Controllers.Sockets
{
    public class WebSocketStreamWriter : IDisposable
    {
        private readonly AutoResetEvent _event;
        private readonly ConcurrentQueue<byte[]> _queue;
        private readonly WebSocket _socket;
        private readonly CancellationToken _token;

        #region Constructors

        private WebSocketStreamWriter(WebSocket socket, CancellationToken token)
        {
            _event = new AutoResetEvent(false);
            _queue = new ConcurrentQueue<byte[]>();
            _socket = socket;
            _token = token;
        }

        public static WebSocketStreamWriter Create(WebSocket socket, CancellationToken token)
        {
            var writer = new WebSocketStreamWriter(socket, token);
            Task.Factory.StartNew(writer.RunAsync, TaskCreationOptions.LongRunning);
            return writer;
        }

        #endregion

        #region Methods

        public void Enqueue(byte[] buffer)
        {
            _queue.Enqueue(buffer);
            _event.Set();
        }

        private async Task RunAsync()
        {
            var events = new[] { _event, _token.WaitHandle };

            while (WaitHandle.WaitAny(events) == 0)
            {
                while (_queue.TryDequeue(out var buffer))
                {
                    try
                    {
                        await _socket.SendAsync(buffer, WebSocketMessageType.Binary, true, _token);
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
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            _event.Set();
            _event.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}