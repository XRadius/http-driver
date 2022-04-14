using System.Text.Json.Serialization;

public record ProcessEntry {
  [JsonPropertyName("pid")]
  public int Pid { get; init; }

  [JsonPropertyName("command")]
  public string Command { get; init; } = string.Empty;

  [JsonPropertyName("args")]
  public IEnumerable<string> Args { get; init; } = Enumerable.Empty<string>();
}
