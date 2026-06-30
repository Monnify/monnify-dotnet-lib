using System.Text.Json.Serialization;
using Monnify.Http;

namespace Monnify.Collections;

/// <summary>Returned by both <c>DebitMandateAsync</c> and <c>GetMandateDebitStatusAsync</c> - they share the same shape.</summary>
public sealed class MandateDebitResult
{
    /// <summary>E.g. <c>PENDING</c> from a fresh debit, or <c>PAID</c>/<c>FAILED</c> from a status check.</summary>
    [JsonPropertyName("transactionStatus")]
    public string TransactionStatus { get; set; } = string.Empty;

    /// <summary>
    /// A string from <c>DebitMandateAsync</c>, but our own documented sample for
    /// <c>GetMandateDebitStatusAsync</c> shows this as an empty object (<c>{}</c>) instead -
    /// deserializes to <see langword="null"/> in that case rather than throwing.
    /// </summary>
    [JsonConverter(typeof(LenientStringJsonConverter))]
    [JsonPropertyName("responseMessage")]
    public string? ResponseMessage { get; set; }

    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("paymentReference")]
    public string PaymentReference { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("debitAmount")]
    public decimal DebitAmount { get; set; }

    [JsonPropertyName("narration")]
    public string Narration { get; set; } = string.Empty;

    [JsonPropertyName("mandateCode")]
    public string MandateCode { get; set; } = string.Empty;
}
