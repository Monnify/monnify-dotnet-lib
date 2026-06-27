using System.Text.Json.Serialization;

namespace Monnify.Disbursements;

public sealed class SingleTransferResult
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;

    /// <summary>
    /// E.g. <c>SUCCESS</c>, <c>PENDING</c>, <c>PENDING_AUTHORIZATION</c> (awaiting OTP),
    /// <c>FAILED</c>, <c>REVERSED</c>, or <c>OTP_EMAIL_DISPATCH_FAILED</c>.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("dateCreated")]
    public string DateCreated { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("totalFee")]
    public decimal TotalFee { get; set; }

    [JsonPropertyName("destinationAccountName")]
    public string DestinationAccountName { get; set; } = string.Empty;

    [JsonPropertyName("destinationBankName")]
    public string DestinationBankName { get; set; } = string.Empty;

    [JsonPropertyName("destinationAccountNumber")]
    public string DestinationAccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("destinationBankCode")]
    public string DestinationBankCode { get; set; } = string.Empty;

    /// <summary>Present only after OTP authorization (<c>AuthorizeSingleTransferAsync</c>).</summary>
    [JsonPropertyName("senderInfo")]
    public SenderInfo? SenderInfo { get; set; }

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }
}
