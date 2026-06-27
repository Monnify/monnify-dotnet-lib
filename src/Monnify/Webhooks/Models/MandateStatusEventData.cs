using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

/// <summary>Event data for <see cref="MonnifyWebhookEventTypes.MandateUpdate"/>.</summary>
public sealed class MandateStatusEventData
{
    [JsonPropertyName("customerAddress")]
    public string? CustomerAddress { get; set; }

    [JsonPropertyName("endDate")]
    public string? EndDate { get; set; }

    [JsonPropertyName("customerEmailAddress")]
    public string CustomerEmailAddress { get; set; } = string.Empty;

    [JsonPropertyName("customerAccountName")]
    public string CustomerAccountName { get; set; } = string.Empty;

    [JsonPropertyName("customerAccountNumber")]
    public string CustomerAccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("customerAccountBankCode")]
    public string CustomerAccountBankCode { get; set; } = string.Empty;

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("mandateDescription")]
    public string? MandateDescription { get; set; }

    [JsonPropertyName("externalMandateReference")]
    public string ExternalMandateReference { get; set; } = string.Empty;

    /// <summary>E.g. <c>PENDING</c>, <c>ACTIVATED</c>, <c>CANCELLED</c>, <c>FAILED</c>.</summary>
    [JsonPropertyName("mandateStatus")]
    public string MandateStatus { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("mandateAmount")]
    public decimal MandateAmount { get; set; }

    [JsonPropertyName("autoRenew")]
    public bool AutoRenew { get; set; }

    [JsonPropertyName("mandateCode")]
    public string MandateCode { get; set; } = string.Empty;

    [JsonPropertyName("contractCode")]
    public string ContractCode { get; set; } = string.Empty;

    [JsonPropertyName("customerPhoneNumber")]
    public string? CustomerPhoneNumber { get; set; }

    [JsonPropertyName("startDate")]
    public string? StartDate { get; set; }
}
