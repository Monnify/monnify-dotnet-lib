using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class LimitProfile
{
    [JsonPropertyName("limitProfileCode")]
    public string LimitProfileCode { get; set; } = string.Empty;

    [JsonPropertyName("limitProfileName")]
    public string LimitProfileName { get; set; } = string.Empty;

    [JsonPropertyName("singleTransactionValue")]
    public decimal SingleTransactionValue { get; set; }

    [JsonPropertyName("dailyTransactionVolume")]
    public int DailyTransactionVolume { get; set; }

    [JsonPropertyName("dailyTransactionValue")]
    public decimal DailyTransactionValue { get; set; }

    [JsonPropertyName("dateCreated")]
    public string DateCreated { get; set; } = string.Empty;

    [JsonPropertyName("lastModified")]
    public string LastModified { get; set; } = string.Empty;
}
