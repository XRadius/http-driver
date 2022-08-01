using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Abstracts.Interfaces;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record UpdateEntity : IPacketWriter
    {
        #region Properties

        [JsonPropertyName("address")]
        public ulong Address { get; init; }

        [JsonPropertyName("members")]
        public ICollection<UpdateEntityMember> Members { get; init; } = Array.Empty<UpdateEntityMember>();

        #endregion

        #region Implementation of IPacketWriter

        public void Write(BinaryWriter stream)
        {
            stream.Write(Address);
            stream.WriteEntityArray(Members);
        }

        #endregion
    }
}