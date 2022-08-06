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
        private readonly Tracker _entities;
        private readonly WebSocketSettings _settings;
        private readonly WebSocket _socket;
        private DateTime _inactiveTime;

        #region Constructors

        private WebSocketChannel(IMemoryService service, WebSocketSettings settings, WebSocket socket)
        {
            _entities = new Tracker(service);
            _settings = settings;
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
                    using var outputBuffer = new MemoryStream();
                    using var output = new BinaryWriter(outputBuffer);

                    if (_inactiveTime < DateTime.Now)
                    {
                        cts.Cancel();
                        break;
                    }

                    while (reader.TryDequeue(out var result))
                    {
                        if (result == null) continue;
                        using var inputBuffer = new MemoryStream(result);
                        using var input = new BinaryReader(inputBuffer);
                        Receive(input, output);
                    }

                    _entities.Update(output);

                    writer.Enqueue(outputBuffer.ToArray());
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

        private void Receive(BinaryReader input, BinaryWriter output)
        {
            while (input.BaseStream.Position < input.BaseStream.Length)
            {
                switch ((PacketType)input.ReadByte())
                {
                    case PacketType.BasicAlive:
                        SetInactiveTime();
                        break;
                    case PacketType.BasicSync:
                        BasicSync.Create(input).Write(output);
                        break;
                    case PacketType.EntityChange:
                        _entities.Receive(EntityChange.Create(input));
                        break;
                    case PacketType.EntityCreate:
                        _entities.Receive(EntityCreate.Create(input));
                        break;
                    case PacketType.EntityDelete:
                        _entities.Receive(EntityDelete.Create(input));
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