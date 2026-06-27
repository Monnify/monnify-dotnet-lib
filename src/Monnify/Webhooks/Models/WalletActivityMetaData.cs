using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

/// <summary>
/// The envelope-level <c>metaData</c> Monnify sends alongside <see cref="WalletActivityEventData"/>.
/// Parse it with <c>MonnifyWebhookParser.ParseMetaData&lt;WalletActivityMetaData&gt;(envelope)</c>.
/// </summary>
public sealed class WalletActivityMetaData
{
    [JsonPropertyName("senderAccount")]
    public string? SenderAccount { get; set; }

    [JsonPropertyName("sourceAccountName")]
    public string? SourceAccountName { get; set; }

    [JsonPropertyName("sourceAccountNumber")]
    public string? SourceAccountNumber { get; set; }

    [JsonPropertyName("sourceBankCode")]
    public string? SourceBankCode { get; set; }

    [JsonPropertyName("sourceBankName")]
    public string? SourceBankName { get; set; }
}
