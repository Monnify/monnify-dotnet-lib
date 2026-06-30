using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class UpdateReservedAccountLimitRequest
{
    [JsonPropertyName("accountReference")]
    public string AccountReference { get; set; } = string.Empty;

    [JsonPropertyName("limitProfileCode")]
    public string LimitProfileCode { get; set; } = string.Empty;
}
