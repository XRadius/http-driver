using System.Text.Json.Serialization;
using HttpDriver.Utilities.Json;

namespace HttpDriver.Apis.DataModels
{
    public record MapEntry
    {
        #region Properties

        [JsonPropertyName("devMajor")]
        public ushort DevMajor { get; init; }

        [JsonPropertyName("devMinor")]
        public ushort DevMinor { get; init; }

        [JsonConverter(typeof(JsonHexConverter))]
        [JsonPropertyName("end")]
        public ulong End { get; init; }

        [JsonConverter(typeof(JsonHexConverter))]
        [JsonPropertyName("inode")]
        public ulong Inode { get; init; }

        [JsonConverter(typeof(JsonHexConverter))]
        [JsonPropertyName("offset")]
        public ulong Offset { get; init; }

        [JsonPropertyName("pathname")]
        public string Pathname { get; init; } = default!;

        [JsonPropertyName("perms")]
        public MapEntryPermissions Perms { get; init; }

        [JsonConverter(typeof(JsonHexConverter))]
        [JsonPropertyName("start")]
        public ulong Start { get; init; }

        #endregion
    }
}