using System.Text.Json.Serialization;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record EntityDelete
    {
        #region Constructors

        public static EntityDelete Create(BinaryReader stream)
        {
            var address = stream.ReadUInt64();
            return new EntityDelete { Address = address };
        }

        #endregion

        #region Properties

        [JsonPropertyName("address")]
        public ulong Address { get; init; }

        #endregion
    }
}