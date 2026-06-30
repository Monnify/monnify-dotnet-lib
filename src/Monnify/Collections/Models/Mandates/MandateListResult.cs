using System.Text.Json.Serialization;

namespace Monnify.Collections;

/// <summary>
/// The paged result from <c>ListMandatesAsync</c>. Uses its own field names
/// (<c>pageNumber</c>/<c>pageSize</c>/<c>numberOfElements</c>) rather than the
/// <c>number</c>/<c>size</c> shapes <see cref="Http.MonnifyPagedResult{T}"/> covers elsewhere in
/// this SDK, so it isn't reused here.
/// </summary>
public sealed class MandateListResult
{
    [JsonPropertyName("content")]
    public IReadOnlyList<Mandate> Content { get; set; } = Array.Empty<Mandate>();

    [JsonPropertyName("totalElements")]
    public long TotalElements { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("first")]
    public bool? IsFirst { get; set; }

    [JsonPropertyName("last")]
    public bool IsLast { get; set; }

    [JsonPropertyName("numberOfElements")]
    public int? NumberOfElements { get; set; }

    [JsonPropertyName("empty")]
    public bool? IsEmpty { get; set; }
}
