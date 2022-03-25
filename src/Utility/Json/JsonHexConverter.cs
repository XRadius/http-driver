using System.Text.Json;
using System.Text.Json.Serialization;

public class JsonHexConverter : JsonConverter<ulong> {
  public override ulong Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options) {
    throw new NotImplementedException();
  }

  public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options) {
    writer.WriteStringValue(value.ToString("X"));
  }
}
