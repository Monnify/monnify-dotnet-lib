using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

/// <summary>One bank-transfer leg of a collection payment.</summary>
public sealed class PaymentSourceInfo
{
    [JsonPropertyName("bankCode")]
    public string? BankCode { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amountPaid")]
    public decimal AmountPaid { get; set; }

    [JsonPropertyName("accountName")]
    public string? AccountName { get; set; }

    [JsonPropertyName("sessionId")]
    public string? SessionId { get; set; }

    [JsonPropertyName("accountNumber")]
    public string? AccountNumber { get; set; }
}
