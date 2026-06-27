using System.Text.Json.Serialization;

namespace Monnify.Http;

/// <summary>
/// A page of results from a Monnify list endpoint. Monnify uses (at least) two slightly different
/// pagination shapes across its APIs: <see cref="TotalPages"/>/<see cref="IsFirst"/>/<see cref="IsLast"/>
/// are populated by the Disbursements/Collections-style paging; <see cref="IsEmpty"/>/<see cref="NextPage"/>
/// are populated by the Bills payment-style paging instead. Only the fields your specific endpoint
/// actually returns will be set - the rest stay at their defaults.
/// </summary>
public sealed class MonnifyPagedResult<T>
{
    [JsonPropertyName("content")]
    public IReadOnlyList<T> Content { get; set; } = Array.Empty<T>();

    [JsonPropertyName("totalElements")]
    public long TotalElements { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("number")]
    public int PageNumber { get; set; }

    [JsonPropertyName("size")]
    public int PageSize { get; set; }

    [JsonPropertyName("first")]
    public bool IsFirst { get; set; }

    [JsonPropertyName("last")]
    public bool IsLast { get; set; }

    [JsonPropertyName("empty")]
    public bool? IsEmpty { get; set; }

    /// <summary>The next page number to request, or <see langword="null"/> when there isn't one.</summary>
    [JsonPropertyName("nextPage")]
    public int? NextPage { get; set; }
}
