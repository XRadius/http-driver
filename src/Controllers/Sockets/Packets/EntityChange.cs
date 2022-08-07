using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record EntityChange
    {
        #region Constructors

        public static EntityChange Create(BinaryReader stream)
        {
            var id = stream.ReadVariableLength();
            var changesSize = (int)stream.ReadVariableLength();
            var changes = Enumerable.Range(0, changesSize).Select(_ => EntityChangeMember.Create(stream)).ToList();
            return new EntityChange { Id = id, Changes = changes };
        }

        #endregion

        #region Properties

        [JsonPropertyName("id")]
        public uint Id { get; init; }
        
        [JsonPropertyName("changes")]
        public IReadOnlyCollection<EntityChangeMember> Changes { get; init; } = Array.Empty<EntityChangeMember>();

        #endregion
    }
}