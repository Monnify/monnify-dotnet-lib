using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class Mandate
{
    [JsonPropertyName("mandateCode")]
    public string MandateCode { get; set; } = string.Empty;

    /// <summary>
    /// The merchant-generated reference supplied as <c>mandateReference</c> at creation time. Our
    /// docs' sample for this endpoint shows the field as <c>externalMandateReference</c>; the real
    /// sandbox returns <c>mandateReference</c> instead.
    /// </summary>
    [JsonPropertyName("mandateReference")]
    public string MandateReference { get; set; } = string.Empty;

    /// <summary>Format: <c>YYYY-MM-DDTHH:MM:SS</c> (with milliseconds and a timezone offset on responses).</summary>
    [JsonPropertyName("startDate")]
    public string StartDate { get; set; } = string.Empty;

    /// <summary>Format: <c>YYYY-MM-DDTHH:MM:SS</c> (with milliseconds and a timezone offset on responses).</summary>
    [JsonPropertyName("endDate")]
    public string EndDate { get; set; } = string.Empty;

    /// <summary>
    /// E.g. <c>ACTIVE</c>, <c>FAILED</c>, <c>CANCELLED</c>, <c>EXPIRED</c>. Sandbox-observed values
    /// include <c>PENDING_AUTHORIZATION</c> as well, which isn't in our docs' status list
    /// (<c>PENDING</c>, <c>ACTIVE</c>, <c>FAILED</c>, <c>CANCELLED</c>, <c>EXPIRED</c>) - treat that
    /// list as non-exhaustive.
    /// </summary>
    [JsonPropertyName("mandateStatus")]
    public string MandateStatus { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("mandateAmount")]
    public decimal MandateAmount { get; set; }

    [JsonPropertyName("contractCode")]
    public string ContractCode { get; set; } = string.Empty;

    [JsonPropertyName("autoRenew")]
    public bool AutoRenew { get; set; }

    [JsonPropertyName("customerPhoneNumber")]
    public string CustomerPhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("customerEmailAddress")]
    public string CustomerEmailAddress { get; set; } = string.Empty;

    [JsonPropertyName("customerAddress")]
    public string CustomerAddress { get; set; } = string.Empty;

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>The account name Monnify resolved for <see cref="CustomerAccountNumber"/>, distinct from <see cref="CustomerName"/>.</summary>
    [JsonPropertyName("customerAccountName")]
    public string? CustomerAccountName { get; set; }

    [JsonPropertyName("customerAccountNumber")]
    public string CustomerAccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("customerAccountBankCode")]
    public string CustomerAccountBankCode { get; set; } = string.Empty;

    [JsonPropertyName("mandateDescription")]
    public string MandateDescription { get; set; } = string.Empty;

    /// <summary>The amount to be debited per periodic debit, if different from <see cref="MandateAmount"/> (the lifetime total).</summary>
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("debitAmount")]
    public decimal? DebitAmount { get; set; }

    /// <summary>Human-readable instructions for the customer to authorize the mandate (a token transfer).</summary>
    [JsonPropertyName("authorizationMessage")]
    public string? AuthorizationMessage { get; set; }

    /// <summary>A hosted page where the customer can complete authorization.</summary>
    [JsonPropertyName("authorizationLink")]
    public string? AuthorizationLink { get; set; }

    /// <summary>
    /// A human-readable status message (e.g. <c>"Active Mandate"</c>). 
    /// </summary>
    [JsonPropertyName("responseMessage")]
    public string? ResponseMessage { get; set; }

    /// <summary>The direct debit scheme, e.g. <c>NDD</c>.</summary>
    [JsonPropertyName("schemeCode")]
    public string? SchemeCode { get; set; }
}
