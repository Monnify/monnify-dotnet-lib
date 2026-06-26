using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class CreateReservedAccountRequest
{
    [JsonPropertyName("accountReference")]
    public string AccountReference { get; set; } = string.Empty;

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = "NGN";

    [JsonPropertyName("contractCode")]
    public string ContractCode { get; set; } = string.Empty;

    [JsonPropertyName("customerEmail")]
    public string CustomerEmail { get; set; } = string.Empty;

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// When <c>true</c>, Monnify allocates accounts across all banks it supports for reserved
    /// accounts. When <c>false</c>, you must supply <see cref="PreferredBanks"/>. Required.
    /// </summary>
    [JsonPropertyName("getAllAvailableBanks")]
    public bool GetAllAvailableBanks { get; set; } = true;

    /// <summary>Bank codes to restrict the reservation to. Required when <see cref="GetAllAvailableBanks"/> is <c>false</c>.</summary>
    [JsonPropertyName("preferredBanks")]
    public IReadOnlyList<string>? PreferredBanks { get; set; }
}
