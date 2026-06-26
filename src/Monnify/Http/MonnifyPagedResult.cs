using System.Text.Json.Serialization;

namespace Monnify.Http;

/// <summary>A page of results from a Monnify list endpoint.</summary>
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
}
