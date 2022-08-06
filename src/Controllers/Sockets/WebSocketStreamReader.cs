using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets
{
    public class WebSocketStreamReader : IDisposable
    {
        private readonly ConcurrentQueue<byte[]> _queue;
        private readonly WebSocket _socket;
        private readonly CancellationToken _token;
        private byte[] _buffer;
        private int _offset;

        #region Constructors

        private WebSocketStreamReader(WebSocket socket, CancellationToken token)
        {
            _buffer = new byte[1024];
            _queue = new ConcurrentQueue<byte[]>();
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

        public bool TryDequeue(out byte[]? result)
        {
            return _queue.TryDequeue(out result);
        }

        private ArraySegment<byte> Prepare(int count)
        {
            while (_offset + count > _buffer.Length)
            {
                Array.Resize(ref _buffer, _buffer.Length * 2);
            }

            return new ArraySegment<byte>(_buffer, _offset, count);
        }

        private async Task RunAsync()
        {
            while (!_token.IsCancellationRequested)
            {
                try
                {
                    var buffer = Prepare(1024);
                    var result = await _socket.ReceiveAsync(buffer, _token);
                    _offset += result.Count;

                    if (result.EndOfMessage)
                    {
                        _queue.Enqueue(_buffer.Copy(0, _offset));
                        _offset = 0;
                    }
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