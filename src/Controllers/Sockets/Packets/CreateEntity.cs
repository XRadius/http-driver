using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record CreateEntity
    {
        #region Constructors

        public static CreateEntity Create(BinaryReader stream)
        {
            var address = stream.ReadUInt64();
            var members = stream.ReadEntityArray(CreateEntityMember.Create);
            return new CreateEntity { Address = address, Members = members };
        }

        #endregion

        #region Properties

        [JsonPropertyName("address")]
        public ulong Address { get; init; }

        [JsonPropertyName("members")]
        public ICollection<CreateEntityMember> Members { get; init; } = Array.Empty<CreateEntityMember>();

        #endregion
    }
}