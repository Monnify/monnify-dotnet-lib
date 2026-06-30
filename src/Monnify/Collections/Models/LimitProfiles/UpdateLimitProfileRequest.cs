using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class UpdateLimitProfileRequest
{
    [JsonPropertyName("limitProfileName")]
    public string LimitProfileName { get; set; } = string.Empty;

    [JsonPropertyName("singleTransactionValue")]
    public decimal SingleTransactionValue { get; set; }

    [JsonPropertyName("dailyTransactionValue")]
    public decimal DailyTransactionValue { get; set; }

    [JsonPropertyName("dailyTransactionVolume")]
    public int DailyTransactionVolume { get; set; }
}
