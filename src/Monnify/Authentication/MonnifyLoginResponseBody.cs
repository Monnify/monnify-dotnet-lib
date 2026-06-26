using System.Text.Json.Serialization;

namespace Monnify.Authentication;

internal sealed class MonnifyLoginResponseBody
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>Token lifetime in seconds (typically ~3600). Callers fall back to 3600 if absent or zero.</summary>
    [JsonPropertyName("expiresIn")]
    public int ExpiresIn { get; set; }
}
