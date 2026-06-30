namespace Monnify.Collections;

/// <summary>Optional filters for <see cref="IMonnifyCollectionsClient.ListMandatesAsync"/>. <see cref="StartDate"/>/<see cref="EndDate"/> are required by the endpoint.</summary>
public sealed class ListMandatesFilter
{
    /// <summary>Required. Format: <c>YYYY-MM-DDTHH:MM:SS</c>. The range with <see cref="EndDate"/> must not exceed 90 days.</summary>
    public string StartDate { get; set; } = string.Empty;

    /// <summary>Required. Format: <c>YYYY-MM-DDTHH:MM:SS</c>. The range with <see cref="StartDate"/> must not exceed 90 days.</summary>
    public string EndDate { get; set; } = string.Empty;

    public string? CustomerEmail { get; set; }

    /// <summary>Direct debit scheme code, e.g. <c>ADD</c>.</summary>
    public string? SchemeCode { get; set; }

    /// <summary>One of <c>PENDING</c>, <c>ACTIVE</c>, <c>FAILED</c>, <c>CANCELLED</c>, <c>EXPIRED</c>.</summary>
    public string? MandateStatus { get; set; }
}
