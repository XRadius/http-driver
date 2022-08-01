using System.Text.Json.Serialization;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record CreateEntityMember
    {
        #region Constructors

        public static CreateEntityMember Create(BinaryReader stream)
        {
            var offset = stream.ReadUInt16();
            var interval = stream.ReadUInt16();
            var size = stream.ReadUInt16();
            return new CreateEntityMember { Interval = interval, Offset = offset, Size = size };
        }

        #endregion

        #region Properties

        [JsonPropertyName("interval")]
        public ushort Interval { get; init; }

        [JsonPropertyName("offset")]
        public ushort Offset { get; init; }

        [JsonPropertyName("size")]
        public ushort Size { get; init; }

        #endregion
    }
}