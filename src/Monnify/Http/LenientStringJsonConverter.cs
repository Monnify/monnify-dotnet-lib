using System.Text.Json;
using System.Text.Json.Serialization;

namespace Monnify.Http;

/// <summary>
/// Some Monnify fields documented as a string come back as an empty object (<c>{}</c>) instead,
/// depending on endpoint/state (e.g. the direct debit mandate debit-status endpoint's
/// <c>responseMessage</c>, which is a string from the debit call itself but <c>{}</c> from the
/// status-check call). Reads a JSON string normally; anything else (object, array, number, bool)
/// deserializes to <see langword="null"/> rather than throwing.
/// </summary>
internal sealed class LenientStringJsonConverter : JsonConverter<string?>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }

        // Properly advances past a {} or [] rather than leaving the reader mid-token, which would
        // otherwise corrupt the rest of the deserialization.
        reader.Skip();
        return null;
    }

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value);
        }
    }
}
