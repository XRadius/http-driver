using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record EntityDelete
    {
        #region Constructors

        public static EntityDelete Create(BinaryReader stream)
        {
            var id = stream.ReadVariableLength();
            return new EntityDelete { Id = id };
        }

        #endregion

        #region Properties

        [JsonPropertyName("Id")]
        public uint Id { get; init; }

        #endregion
    }
}