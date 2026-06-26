using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class Invoice
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("invoiceReference")]
    public string InvoiceReference { get; set; } = string.Empty;

    [JsonPropertyName("invoiceStatus")]
    public string InvoiceStatus { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("contractCode")]
    public string ContractCode { get; set; } = string.Empty;

    [JsonPropertyName("customerEmail")]
    public string CustomerEmail { get; set; } = string.Empty;

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("expiryDate")]
    public string ExpiryDate { get; set; } = string.Empty;

    [JsonPropertyName("createdBy")]
    public string CreatedBy { get; set; } = string.Empty;

    [JsonPropertyName("createdOn")]
    public string CreatedOn { get; set; } = string.Empty;

    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("accountName")]
    public string? AccountName { get; set; }

    [JsonPropertyName("bankName")]
    public string BankName { get; set; } = string.Empty;

    [JsonPropertyName("bankCode")]
    public string BankCode { get; set; } = string.Empty;

    /// <summary>Only present on create/details responses, not in the list returned by <c>GetInvoicesAsync</c>.</summary>
    [JsonPropertyName("checkoutUrl")]
    public string? CheckoutUrl { get; set; }

    [JsonPropertyName("redirectUrl")]
    public string? RedirectUrl { get; set; }

    [JsonPropertyName("transactionReference")]
    public string? TransactionReference { get; set; }

    [JsonPropertyName("accountReference")]
    public string? AccountReference { get; set; }

    [JsonPropertyName("incomeSplitConfig")]
    public IReadOnlyList<IncomeSplitConfig>? IncomeSplitConfig { get; set; }
}
