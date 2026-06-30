using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class CreateMandateRequest
{
    [JsonPropertyName("contractCode")]
    public string ContractCode { get; set; } = string.Empty;

    /// <summary>Your own reference to identify this mandate - returned back as <see cref="Mandate.MandateReference"/>.</summary>
    [JsonPropertyName("mandateReference")]
    public string MandateReference { get; set; } = string.Empty;

    /// <summary>Total lifetime amount debitable on the mandate.</summary>
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("mandateAmount")]
    public decimal MandateAmount { get; set; }

    /// <summary>Whether the mandate renews automatically once it reaches <see cref="MandateEndDate"/>.</summary>
    [JsonPropertyName("autoRenew")]
    public bool AutoRenew { get; set; }

    /// <summary>Whether the customer is allowed to cancel this mandate themselves.</summary>
    [JsonPropertyName("customerCancellation")]
    public bool CustomerCancellation { get; set; }

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("customerPhoneNumber")]
    public string CustomerPhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("customerEmailAddress")]
    public string CustomerEmailAddress { get; set; } = string.Empty;

    [JsonPropertyName("customerAddress")]
    public string CustomerAddress { get; set; } = string.Empty;

    [JsonPropertyName("customerAccountNumber")]
    public string CustomerAccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("customerAccountBankCode")]
    public string CustomerAccountBankCode { get; set; } = string.Empty;

    [JsonPropertyName("mandateDescription")]
    public string MandateDescription { get; set; } = string.Empty;

    /// <summary>Format: <c>YYYY-MM-DDTHH:MM:SS</c>.</summary>
    [JsonPropertyName("mandateStartDate")]
    public string MandateStartDate { get; set; } = string.Empty;

    /// <summary>Format: <c>YYYY-MM-DDTHH:MM:SS</c>.</summary>
    [JsonPropertyName("mandateEndDate")]
    public string MandateEndDate { get; set; } = string.Empty;

    /// <summary>Where to redirect the customer to after they complete authorization.</summary>
    [JsonPropertyName("redirectUrl")]
    public string? RedirectUrl { get; set; }

    /// <summary>The amount to be debited per periodic debit, if different from the lifetime <see cref="MandateAmount"/>.</summary>
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("debitAmount")]
    public decimal? DebitAmount { get; set; }
}
