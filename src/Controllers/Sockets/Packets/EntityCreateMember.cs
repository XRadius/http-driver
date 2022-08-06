using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record EntityCreateMember
    {
        #region Constructors

        public static EntityCreateMember Create(BinaryReader stream)
        {
            var offset = stream.ReadVariableLength();
            var interval = stream.ReadVariableLength();
            var size = stream.ReadVariableLength();
            return new EntityCreateMember { Interval = interval, Offset = offset, Size = size };
        }

        #endregion

        #region Properties

        [JsonPropertyName("interval")]
        public uint Interval { get; init; }

        [JsonPropertyName("offset")]
        public uint Offset { get; init; }

        [JsonPropertyName("size")]
        public uint Size { get; init; }

        #endregion
    }
}