using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record ChangeEntity
    {
        #region Constructors

        public static ChangeEntity Create(BinaryReader stream)
        {
            var address = stream.ReadUInt64();
            var changes = stream.ReadEntityArray(ChangeEntityMember.Create);
            return new ChangeEntity { Address = address, Changes = changes };
        }

        #endregion

        #region Properties

        [JsonPropertyName("address")]
        public ulong Address { get; init; }

        [JsonPropertyName("changes")]
        public ICollection<ChangeEntityMember> Changes { get; init; } = Array.Empty<ChangeEntityMember>();

        #endregion
    }
}