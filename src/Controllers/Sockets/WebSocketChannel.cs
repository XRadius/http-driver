using System.Diagnostics;
using System.Net.WebSockets;
using HttpDriver.Controllers.Sockets.Abstracts.Enums;
using HttpDriver.Controllers.Sockets.Models;
using HttpDriver.Utilities;

namespace HttpDriver.Controllers.Sockets
{
    public class WebSocketChannel
    {
        private readonly IMemoryService _service;
        private readonly WebSocketSettings _settings;
        private readonly WebSocket _socket;

        #region Constructors

        private WebSocketChannel(IMemoryService service, WebSocketSettings settings, WebSocket socket)
        {
            _service = service;
            _settings = settings;
            _socket = socket;
        }

        public static WebSocketChannel Create(IMemoryService service, WebSocketSettings settings, WebSocket socket)
        {
            return new WebSocketChannel(service, settings, socket);
        }

        #endregion

        #region Methods

        public void Run()
        {
            using var cts = new CancellationTokenSource();
            using var reader = WebSocketStreamReader.Create(_socket, cts.Token);
            using var writer = WebSocketStreamWriter.Create(_socket, cts.Token);
            using var tlt = TightLoopTimer.Create(TimeSpan.FromMilliseconds(1000f / _settings.FramesPerSecond));

            var events = new[] { tlt, cts.Token.WaitHandle };
            var state = new State(_service);

            while (WaitHandle.WaitAny(events) == 0)
            {
                try
                {
                    if (!state.IsActive)
                    {
                        cts.Cancel();
                        break;
                    }

                    while (reader.TryDequeue(out var result))
                    {
                        if (result.Array == null) continue;
                        using var ms = new MemoryStream(result.Array, result.Offset, result.Count);
                        using var stream = new BinaryReader(ms);
                        state.Receive(stream);
                    }

                    var update = state.Update();
                    if (update != null)
                    {
                        using var ms = new MemoryStream();
                        using var stream = new BinaryWriter(ms);
                        stream.Write((byte)PacketType.Update);
                        update.Write(stream);
                        writer.Enqueue(ms.ToArray());
                    }
                }
                catch (OperationCanceledException)
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
    }
}