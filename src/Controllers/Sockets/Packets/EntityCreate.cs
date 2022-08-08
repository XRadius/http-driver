using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record EntityCreate
    {
        #region Constructors

        public static EntityCreate Create(BinaryReader stream)
        {
            var id = stream.ReadVariableLength();
            var address = stream.ReadUInt64();
            var membersSize = (int)stream.ReadVariableLength();
            var members = Enumerable.Range(0, membersSize).Select(_ => EntityCreateMember.Create(stream)).ToList();
            var requestBatch = stream.ReadBoolean();
            return new EntityCreate { Id = id, Address = address, Members = members, RequestBatch = requestBatch };
        }

        #endregion

        #region Properties

        [JsonPropertyName("address")]
        public ulong Address { get; init; }

        [JsonPropertyName("id")]
        public uint Id { get; init; }

        [JsonPropertyName("members")]
        public IReadOnlyCollection<EntityCreateMember> Members { get; init; } = Array.Empty<EntityCreateMember>();

        [JsonPropertyName("requestBatch")]
        public bool RequestBatch { get; init; }

        #endregion
    }
}