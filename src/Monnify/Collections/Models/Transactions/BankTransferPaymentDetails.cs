using System.Text.Json.Serialization;

namespace Monnify.Collections;

/// <summary>A dynamically generated bank account for a one-time bank transfer payment.</summary>
public sealed class BankTransferPaymentDetails
{
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;

    [JsonPropertyName("bankName")]
    public string BankName { get; set; } = string.Empty;

    [JsonPropertyName("bankCode")]
    public string BankCode { get; set; } = string.Empty;

    [JsonPropertyName("accountDurationSeconds")]
    public int AccountDurationSeconds { get; set; }

    [JsonPropertyName("ussdPayment")]
    public string? UssdPayment { get; set; }

    [JsonPropertyName("requestTime")]
    public string RequestTime { get; set; } = string.Empty;

    [JsonPropertyName("expiresOn")]
    public string ExpiresOn { get; set; } = string.Empty;

    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("paymentReference")]
    public string PaymentReference { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("fee")]
    public decimal Fee { get; set; }

    [JsonPropertyName("totalPayable")]
    public decimal TotalPayable { get; set; }

    [JsonPropertyName("collectionChannel")]
    public string CollectionChannel { get; set; } = string.Empty;
}
