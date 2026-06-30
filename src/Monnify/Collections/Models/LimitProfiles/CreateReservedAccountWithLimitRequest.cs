using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class CreateReservedAccountWithLimitRequest
{
    [JsonPropertyName("contractCode")]
    public string ContractCode { get; set; } = string.Empty;

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = "NGN";

    [JsonPropertyName("accountReference")]
    public string AccountReference { get; set; } = string.Empty;

    [JsonPropertyName("customerEmail")]
    public string CustomerEmail { get; set; } = string.Empty;

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>When <c>false</c>, <see cref="PreferredBanks"/> must be supplied.</summary>
    [JsonPropertyName("getAllAvailableBanks")]
    public bool GetAllAvailableBanks { get; set; } = true;

    [JsonPropertyName("preferredBanks")]
    public IReadOnlyList<string>? PreferredBanks { get; set; }

    /// <summary>The limit profile to enforce on this reserved account.</summary>
    [JsonPropertyName("limitProfileCode")]
    public string LimitProfileCode { get; set; } = string.Empty;
}
