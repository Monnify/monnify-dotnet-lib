using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class Paycode
{
    /// <summary>The generated numeric code the beneficiary uses to collect cash.</summary>
    [JsonPropertyName("paycode")]
    public string PaycodeValue { get; set; } = string.Empty;

    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("paycodeReference")]
    public string PaycodeReference { get; set; } = string.Empty;

    [JsonPropertyName("beneficiaryName")]
    public string BeneficiaryName { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("fee")]
    public decimal Fee { get; set; }

    /// <summary>E.g. <c>PENDING</c>, <c>COMPLETED</c>, <c>CANCELLED</c>.</summary>
    [JsonPropertyName("transactionStatus")]
    public string TransactionStatus { get; set; } = string.Empty;

    /// <summary>Format: <c>YYYY-MM-DD HH:MM:SS</c>.</summary>
    [JsonPropertyName("expiryDate")]
    public string ExpiryDate { get; set; } = string.Empty;

    [JsonPropertyName("createdOn")]
    public string CreatedOn { get; set; } = string.Empty;

    [JsonPropertyName("createdBy")]
    public string CreatedBy { get; set; } = string.Empty;

    [JsonPropertyName("modifiedBy")]
    public string ModifiedBy { get; set; } = string.Empty;

    /// <summary>Populated once the paycode has been cancelled; absent otherwise.</summary>
    [JsonPropertyName("cancelDate")]
    public string? CancelDate { get; set; }
}
