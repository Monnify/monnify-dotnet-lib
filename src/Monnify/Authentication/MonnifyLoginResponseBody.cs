using System.Text.Json.Serialization;

namespace Monnify.Authentication;

internal sealed class MonnifyLoginResponseBody
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Token lifetime in seconds. Field name is inferred from convention, not independently
    /// confirmed against a live response this session (see docs/COMPATIBILITY.md) — the
    /// 3600-second expiry itself is confirmed, so callers fall back to it if this is absent or zero.
    /// </summary>
    [JsonPropertyName("expiresIn")]
    public int ExpiresIn { get; set; }
}
