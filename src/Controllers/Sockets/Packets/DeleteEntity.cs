using System.Text.Json.Serialization;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record DeleteEntity
    {
        #region Constructors

        public static DeleteEntity Create(BinaryReader stream)
        {
            var address = stream.ReadUInt64();
            return new DeleteEntity { Address = address };
        }

        #endregion

        #region Properties

        [JsonPropertyName("address")]
        public ulong Address { get; init; }

        #endregion
    }
}