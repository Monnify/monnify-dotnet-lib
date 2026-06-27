using System.Text.Json.Serialization;

namespace Monnify.Disbursements;

/// <summary>Identifies the sender on a single transfer, shown to the beneficiary's bank.</summary>
public sealed class SenderInfo
{
    [JsonPropertyName("sourceAccountNumber")]
    public string SourceAccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("sourceAccountName")]
    public string SourceAccountName { get; set; } = string.Empty;

    [JsonPropertyName("sourceAccountBvn")]
    public string? SourceAccountBvn { get; set; }

    [JsonPropertyName("senderBankCode")]
    public string SenderBankCode { get; set; } = string.Empty;
}
