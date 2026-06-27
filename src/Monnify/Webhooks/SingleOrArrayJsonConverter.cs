using System.Text.Json;
using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

/// <summary>
/// Some Monnify webhook fields (e.g. <c>paymentSourceInformation</c>) come back as an array of
/// objects for one event variant and an empty object (<c>{}</c>) for another, even though they
/// share the same <c>eventType</c>. This accepts either shape rather than throwing on whichever
/// one wasn't expected.
/// </summary>
internal sealed class SingleOrArrayJsonConverter<T> : JsonConverter<IReadOnlyList<T>>
{
    public override IReadOnlyList<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return Array.Empty<T>();
        }

        if (reader.TokenType == JsonTokenType.StartArray)
        {
            return JsonSerializer.Deserialize<List<T>>(ref reader, options) ?? new List<T>();
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            if (!document.RootElement.EnumerateObject().Any())
            {
                return Array.Empty<T>();
            }

            var single = document.RootElement.Deserialize<T>(options);
            return single is null ? Array.Empty<T>() : new[] { single };
        }

        throw new JsonException($"Unexpected token {reader.TokenType} when reading {typeToConvert}.");
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<T> value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, value, options);
}
