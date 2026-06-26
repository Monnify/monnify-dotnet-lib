using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class CreateInvoiceRequest
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = "NGN";

    [JsonPropertyName("invoiceReference")]
    public string InvoiceReference { get; set; } = string.Empty;

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("customerEmail")]
    public string CustomerEmail { get; set; } = string.Empty;

    [JsonPropertyName("contractCode")]
    public string ContractCode { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>Format: <c>YYYY-MM-DD HH:MM:SS</c>. Monnify rejects dates too far in the future.</summary>
    [JsonPropertyName("expiryDate")]
    public string ExpiryDate { get; set; } = string.Empty;

    [JsonPropertyName("paymentMethods")]
    public IReadOnlyList<string>? PaymentMethods { get; set; }

    [JsonPropertyName("incomeSplitConfig")]
    public IReadOnlyList<IncomeSplitConfig>? IncomeSplitConfig { get; set; }

    [JsonPropertyName("redirectUrl")]
    public string? RedirectUrl { get; set; }

    /// <summary>
    /// Attaches the invoice to an existing reserved account (a "Static Invoice"). Omit to create
    /// a "Dynamic Invoice" with its own one-off account instead.
    /// </summary>
    [JsonPropertyName("accountReference")]
    public string? AccountReference { get; set; }
}
