using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Abstracts.Interfaces;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record EntityUpdateEntityMember : IPacketWriter
    {
        #region Properties

        [JsonPropertyName("buffer")]
        public byte[] Buffer { get; init; } = Array.Empty<byte>();

        [JsonPropertyName("offset")]
        public uint Offset { get; init; }

        #endregion

        #region Implementation of IPacketWriter

        public void Write(BinaryWriter stream)
        {
            stream.WriteVariableLength(Offset);
            stream.WriteVariableLength((uint)Buffer.Length);
            stream.Write(Buffer);
        }

        #endregion
    }
}