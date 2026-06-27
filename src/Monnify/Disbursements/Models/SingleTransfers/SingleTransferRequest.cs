using System.Text.Json.Serialization;

namespace Monnify.Disbursements;

/// <summary>
/// Initiates a transfer to a bank account. Requires Monnify to have enabled the Transfer feature
/// for the merchant; contact sales@monnify.com to request access.
/// </summary>
public sealed class SingleTransferRequest
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

    /// <summary>Your Monnify wallet account number.</summary>
    [JsonPropertyName("sourceAccountNumber")]
    public string SourceAccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("senderInfo")]
    public SenderInfo? SenderInfo { get; set; }

    /// <summary>Whether the transfer should be processed asynchronously.</summary>
    [JsonPropertyName("async")]
    public bool? Async { get; set; }
}
