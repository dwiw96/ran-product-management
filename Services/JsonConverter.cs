using System.Text.Json;
using System.Text.Json.Serialization;
using ran_product_management_net.Database.Mongodb.Models;

namespace ran_product_management_net.Services;

public class JsonPolymorphicConverter<TBase> : JsonConverter<TBase>
{
    public override TBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var doc = JsonDocument.ParseValue(ref reader);
        if (!doc.RootElement.TryGetProperty("$type", out var typeProperty))
        {
            throw new JsonException("Missing $type discriminator");
        }

        var typeName = typeProperty.GetString();
        var derivedType = typeName switch
        {
            "Smartphone" => typeof(Smartphone),
            "Fashion" => typeof(Fashion),
            "Electronic" => typeof(Electronic),
            _ => throw new JsonException($"Unknown type discriminator: {typeName}")
        };

        var json = doc.RootElement.GetRawText();
        return (TBase)JsonSerializer.Deserialize(json, derivedType, options)!;
    }

    public override void Write(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        // Convert PascalCase or camelCase to snake_case
        return string.Concat(
            name.Select((ch, i) =>
                i > 0 && char.IsUpper(ch) ? "_" + char.ToLower(ch) : char.ToLower(ch).ToString()
            )
        );
    }
}
