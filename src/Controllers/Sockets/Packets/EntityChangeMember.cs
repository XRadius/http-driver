using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record EntityChangeMember
    {
        #region Constructors

        public static EntityChangeMember Create(BinaryReader stream)
        {
            var offset = stream.ReadVariableLength();
            var bufferSize = (int)stream.ReadVariableLength();
            var buffer = stream.ReadBytes(bufferSize);
            return new EntityChangeMember { Offset = offset, Buffer = buffer };
        }

        #endregion

        #region Properties

        [JsonPropertyName("buffer")]
        public byte[] Buffer { get; init; } = Array.Empty<byte>();

        [JsonPropertyName("offset")]
        public uint Offset { get; init; }

        #endregion
    }
}