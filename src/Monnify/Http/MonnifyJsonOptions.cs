using System.Text.Json;

namespace Monnify.Http;

internal static class MonnifyJsonOptions
{
    /// <summary>
    /// Shared serializer options for all Monnify request/response payloads. DTO properties use
    /// explicit <see cref="System.Text.Json.Serialization.JsonPropertyNameAttribute"/> rather than
    /// relying solely on the camelCase naming policy, since not every Monnify field name follows a
    /// simple PascalCase-to-camelCase conversion (e.g. acronyms like BVN).
    /// </summary>
    public static readonly JsonSerializerOptions Default = new()
    {
        PropertyNameCaseInsensitive = true,
    };
}
