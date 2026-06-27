namespace Monnify.Collections;

/// <summary>Optional filters for <see cref="IMonnifyCollectionsClient.SearchTransactionsAsync"/>. All fields are optional.</summary>
public sealed class SearchTransactionsRequest
{
    public string? PaymentReference { get; set; }
    public string? TransactionReference { get; set; }
    public decimal? FromAmount { get; set; }
    public decimal? ToAmount { get; set; }
    public decimal? Amount { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? PaymentStatus { get; set; }

    /// <summary>Start of the date range, as a Unix timestamp in milliseconds.</summary>
    public long? From { get; set; }

    /// <summary>End of the date range, as a Unix timestamp in milliseconds.</summary>
    public long? To { get; set; }
}
