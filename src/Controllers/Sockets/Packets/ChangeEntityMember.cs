using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record ChangeEntityMember
    {
        #region Constructors

        public static ChangeEntityMember Create(BinaryReader stream)
        {
            var offset = stream.ReadUInt16();
            var buffer = stream.ReadByteArray();
            return new ChangeEntityMember { Offset = offset, Buffer = buffer };
        }

        #endregion

        #region Properties

        [JsonPropertyName("buffer")]
        public byte[] Buffer { get; init; } = Array.Empty<byte>();

        [JsonPropertyName("offset")]
        public ushort Offset { get; init; }

        #endregion
    }
}