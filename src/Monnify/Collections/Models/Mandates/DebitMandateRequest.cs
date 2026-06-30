using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class DebitMandateRequest
{
    /// <summary>Your own reference to identify this single debit.</summary>
    [JsonPropertyName("paymentReference")]
    public string PaymentReference { get; set; } = string.Empty;

    [JsonPropertyName("mandateCode")]
    public string MandateCode { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("debitAmount")]
    public decimal DebitAmount { get; set; }

    [JsonPropertyName("narration")]
    public string Narration { get; set; } = string.Empty;

    [JsonPropertyName("customerEmail")]
    public string CustomerEmail { get; set; } = string.Empty;

    /// <summary>How to split this payment among sub-accounts, if at all.</summary>
    [JsonPropertyName("incomeSplitConfig")]
    public IReadOnlyList<IncomeSplitConfig>? IncomeSplitConfig { get; set; }
}
