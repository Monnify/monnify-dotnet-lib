using System.Text.Json.Serialization;

namespace Monnify.Bills;

/// <summary>Returned by both <c>VendAsync</c> and <c>RequeryAsync</c> - they share the same shape.</summary>
public sealed class VendBillResult
{
    [JsonPropertyName("vendReference")]
    public string VendReference { get; set; } = string.Empty;

    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    /// <summary>E.g. <c>SUCCESS</c>.</summary>
    [JsonPropertyName("vendStatus")]
    public string VendStatus { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("vendAmount")]
    public decimal VendAmount { get; set; }

    /// <summary>The amount actually charged, including <see cref="Commission"/>.</summary>
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("payableAmount")]
    public decimal PayableAmount { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("commission")]
    public decimal Commission { get; set; }

    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;

    [JsonPropertyName("productCode")]
    public string ProductCode { get; set; } = string.Empty;

    [JsonPropertyName("productName")]
    public string ProductName { get; set; } = string.Empty;

    [JsonPropertyName("billerCode")]
    public string BillerCode { get; set; } = string.Empty;

    [JsonPropertyName("billerName")]
    public string BillerName { get; set; } = string.Empty;
}
