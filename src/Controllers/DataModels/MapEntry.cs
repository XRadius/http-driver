using System.Text.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Annotations;

public record MapEntry {
  [JsonConverter(typeof(JsonHexConverter))]
  [JsonPropertyName("start")]
  [JsonSchema(JsonObjectType.String)]
  public ulong Start { get; init; }

  [JsonConverter(typeof(JsonHexConverter))]
  [JsonPropertyName("end")]
  [JsonSchema(JsonObjectType.String)]
  public ulong End { get; init; }

  [JsonPropertyName("perms")]
  public MapEntryPermissions Perms { get; init; }

  [JsonConverter(typeof(JsonHexConverter))]
  [JsonPropertyName("offset")]
  [JsonSchema(JsonObjectType.String)]
  public ulong Offset { get; init; }

  [JsonPropertyName("devMajor")]
  public ushort DevMajor { get; init; }

  [JsonPropertyName("devMinor")]
  public ushort DevMinor { get; init; }

  [JsonConverter(typeof(JsonHexConverter))]
  [JsonPropertyName("inode")]
  [JsonSchema(JsonObjectType.String)]
  public ulong Inode { get; init; }

  [JsonPropertyName("pathname")]
  public string Pathname { get; init; } = default!;
}
