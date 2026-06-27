using System.Text.Json.Serialization;

namespace Monnify.Disbursements;

public sealed class BulkTransferItem
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;

    [JsonPropertyName("narration")]
    public string Narration { get; set; } = string.Empty;

    [JsonPropertyName("destinationBankCode")]
    public string DestinationBankCode { get; set; } = string.Empty;

    [JsonPropertyName("destinationAccountNumber")]
    public string DestinationAccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("destinationAccountName")]
    public string DestinationAccountName { get; set; } = string.Empty;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "NGN";
}
