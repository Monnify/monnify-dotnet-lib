using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

public sealed class PaymentRejectionInformation
{
    [JsonPropertyName("bankCode")]
    public string BankCode { get; set; } = string.Empty;

    [JsonPropertyName("destinationAccountNumber")]
    public string DestinationAccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("bankName")]
    public string BankName { get; set; } = string.Empty;

    /// <summary>E.g. <c>UNDER_PAYMENT</c>.</summary>
    [JsonPropertyName("rejectionReason")]
    public string RejectionReason { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("expectedAmount")]
    public decimal ExpectedAmount { get; set; }
}
