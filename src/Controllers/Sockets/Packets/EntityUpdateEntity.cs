using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Abstracts.Interfaces;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record EntityUpdateEntity : IPacketWriter
    {
        #region Properties

        [JsonPropertyName("address")]
        public ulong Address { get; init; }

        [JsonPropertyName("members")]
        public IReadOnlyCollection<EntityUpdateEntityMember> Members { get; init; } = Array.Empty<EntityUpdateEntityMember>();

        #endregion

        #region Implementation of IPacketWriter

        public void Write(BinaryWriter stream)
        {
            stream.Write(Address);
            stream.WriteKnownEntityArray(Members);
        }

        #endregion
    }
}