using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Abstracts.Enums;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record EntityChangeMemberDelta
    {
        #region Constructors

        public static EntityChangeMemberDelta Create(BinaryReader stream)
        {
            var offset = stream.ReadVariableLength();
            var type = (DeltaType)stream.ReadByte();
            var buffer = stream.ReadBytes(type.Size());
            return new EntityChangeMemberDelta { Offset = offset, Type = type, Buffer = buffer };
        }

        #endregion

        #region Properties

        [JsonPropertyName("buffer")]
        public byte[] Buffer { get; init; } = Array.Empty<byte>();

        [JsonPropertyName("offset")]
        public uint Offset { get; init; }

        [JsonPropertyName("type")]
        public DeltaType Type { get; init; }

        #endregion
    }
}