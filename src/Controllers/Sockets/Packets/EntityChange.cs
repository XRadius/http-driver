using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record EntityChange
    {
        #region Constructors

        public static EntityChange Create(BinaryReader stream)
        {
            var address = stream.ReadUInt64();
            var changes = stream.ReadKnownEntityArray(EntityChangeMember.Create);
            return new EntityChange { Address = address, Changes = changes };
        }

        #endregion

        #region Properties

        [JsonPropertyName("address")]
        public ulong Address { get; init; }

        [JsonPropertyName("changes")]
        public IReadOnlyCollection<EntityChangeMember> Changes { get; init; } = Array.Empty<EntityChangeMember>();

        #endregion
    }
}