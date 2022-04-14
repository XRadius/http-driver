using System.Text.Json.Serialization;

public record MapEntry {
  [JsonConverter(typeof(JsonHexConverter))]
  [JsonPropertyName("start")]
  public ulong Start { get; init; }

  [JsonConverter(typeof(JsonHexConverter))]
  [JsonPropertyName("end")]
  public ulong End { get; init; }

  [JsonPropertyName("perms")]
  public MapEntryPermissions Perms { get; init; }

  [JsonConverter(typeof(JsonHexConverter))]
  [JsonPropertyName("offset")]
  public ulong Offset { get; init; }

  [JsonPropertyName("devMajor")]
  public ushort DevMajor { get; init; }

  [JsonPropertyName("devMinor")]
  public ushort DevMinor { get; init; }

  [JsonConverter(typeof(JsonHexConverter))]
  [JsonPropertyName("inode")]
  public ulong Inode { get; init; }

  [JsonPropertyName("pathname")]
  public string Pathname { get; init; } = default!;
}
