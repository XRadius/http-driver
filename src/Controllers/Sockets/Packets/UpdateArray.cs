using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Abstracts.Interfaces;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record UpdateArray : IPacketWriter
    {
        #region Properties

        [JsonPropertyName("entities")]
        public ICollection<UpdateEntity> Entities { get; init; } = Array.Empty<UpdateEntity>();

        #endregion

        #region Implementation of IPacketWriter

        public void Write(BinaryWriter stream)
        {
            stream.WriteEntityArray(Entities);
        }

        #endregion
    }
}