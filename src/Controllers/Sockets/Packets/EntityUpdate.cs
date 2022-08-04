using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Abstracts.Enums;
using HttpDriver.Controllers.Sockets.Abstracts.Interfaces;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record EntityUpdate : IPacketWriter
    {
        #region Properties

        [JsonPropertyName("entities")]
        public IReadOnlyCollection<EntityUpdateEntity> Entities { get; init; } = Array.Empty<EntityUpdateEntity>();

        #endregion

        #region Implementation of IPacketWriter

        public void Write(BinaryWriter stream)
        {
            stream.Write((byte)PacketType.EntityUpdate);
            stream.WriteKnownEntityArray(Entities);
        }

        #endregion
    }
}