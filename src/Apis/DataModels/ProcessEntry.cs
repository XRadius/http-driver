using System.Text.Json.Serialization;

namespace HttpDriver.Apis.DataModels
{
    public record ProcessEntry
    {
        #region Properties

        [JsonPropertyName("args")]
        public IEnumerable<string> Args { get; init; } = Enumerable.Empty<string>();

        [JsonPropertyName("command")]
        public string Command { get; init; } = string.Empty;

        [JsonPropertyName("pid")]
        public int Pid { get; init; }

        #endregion
    }
}