using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

public sealed class SettlementAccountPayment
{
    [JsonPropertyName("bankCode")]
    public string? BankCode { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amountPaid")]
    public decimal AmountPaid { get; set; }

    [JsonPropertyName("accountName")]
    public string? AccountName { get; set; }

    [JsonPropertyName("accountNumber")]
    public string? AccountNumber { get; set; }
}
