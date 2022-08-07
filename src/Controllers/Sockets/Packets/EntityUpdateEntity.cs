using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Abstracts.Interfaces;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record EntityUpdateEntity : IPacketWriter
    {
        #region Properties

        [JsonPropertyName("id")]
        public uint Id { get; init; }

        [JsonPropertyName("members")]
        public IReadOnlyCollection<EntityUpdateEntityMember> Members { get; init; } = Array.Empty<EntityUpdateEntityMember>();

        #endregion

        #region Implementation of IPacketWriter

        public void Write(BinaryWriter stream)
        {
            stream.WriteVariableLength(Id);
            stream.WriteVariableLength((uint)Members.Count);
            foreach (var member in Members) member.Write(stream);
        }

        #endregion
    }
}