using System.Diagnostics;
using System.Net.WebSockets;
using HttpDriver.Controllers.Sockets.Abstracts.Enums;
using HttpDriver.Controllers.Sockets.Models;
using HttpDriver.Controllers.Sockets.Packets;
using HttpDriver.Utilities;

namespace HttpDriver.Controllers.Sockets
{
    public class WebSocketChannel
    {
        private readonly WebSocketSettings _settings;
        private readonly WebSocket _socket;
        private readonly Tracker _tracker;
        private DateTime _inactiveTime;

        #region Constructors

        private WebSocketChannel(IMemoryService service, WebSocketSettings settings, WebSocket socket)
        {
            _settings = settings;
            _tracker = new Tracker(service);
            _socket = socket;
            SetInactiveTime();
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

            while (WaitHandle.WaitAny(events) == 0)
            {
                try
                {
                    if (_inactiveTime < DateTime.Now)
                    {
                        cts.Cancel();
                        break;
                    }

                    while (reader.TryDequeue(out var result))
                    {
                        if (result.Array == null) continue;
                        using var ms = new MemoryStream(result.Array, result.Offset, result.Count);
                        using var stream = new BinaryReader(ms);
                        Receive(stream);
                    }

                    var update = _tracker.Update();
                    if (update != null)
                    {
                        using var ms = new MemoryStream();
                        using var stream = new BinaryWriter(ms);
                        stream.Write((byte)PacketType.EntityUpdate);
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

        private void Receive(BinaryReader stream)
        {
            while (stream.BaseStream.Position < stream.BaseStream.Length)
            {
                switch ((PacketType)stream.ReadByte())
                {
                    case PacketType.Activity:
                        SetInactiveTime();
                        break;
                    case PacketType.EntityChange:
                        _tracker.Receive(EntityChange.Create(stream));
                        break;
                    case PacketType.EntityCreate:
                        _tracker.Receive(EntityCreate.Create(stream));
                        break;
                    case PacketType.EntityDelete:
                        _tracker.Receive(EntityDelete.Create(stream));
                        break;
                }
            }
        }

        private void SetInactiveTime()
        {
            _inactiveTime = DateTime.Now.AddSeconds(30);
        }

        #endregion
    }
}