using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Extensions;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public record EntityChangeMember
    {
        #region Constructors

        public static EntityChangeMember Create(BinaryReader stream)
        {
            var offset = stream.ReadVariableLength();
            var deltaSize = (int)stream.ReadVariableLength();
            return deltaSize != 0 ? CreateDelta(stream, offset, deltaSize) : CreateBuffer(stream, offset);
        }

        private static EntityChangeMember CreateBuffer(BinaryReader stream, uint offset)
        {
            var bufferSize = (int)stream.ReadVariableLength();
            var buffer = stream.ReadBytes(bufferSize);
            return new EntityChangeMember { Offset = offset, Buffer = buffer };
        }

        private static EntityChangeMember CreateDelta(BinaryReader stream, uint offset, int deltaSize)
        {
            var deltas = Enumerable.Range(0, deltaSize).Select(_ => EntityChangeMemberDelta.Create(stream)).ToList();
            return new EntityChangeMember { Offset = offset, Deltas = deltas };
        }

        #endregion

        #region Properties

        [JsonPropertyName("buffer")]
        public byte[] Buffer { get; init; } = Array.Empty<byte>();

        [JsonPropertyName("deltas")]
        public IReadOnlyCollection<EntityChangeMemberDelta> Deltas { get; init; } = Array.Empty<EntityChangeMemberDelta>();

        [JsonPropertyName("offset")]
        public uint Offset { get; init; }

        #endregion
    }
}