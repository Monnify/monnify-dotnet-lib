using System.Text.Json.Serialization;

namespace Monnify.Cards;

/// <summary>
/// Raw card details for a direct charge. All fields are modeled as strings (rather than the
/// numeric types our docs show for some of these on the 3DS endpoint specifically) to avoid
/// silently dropping a leading zero in <see cref="ExpiryMonth"/>, <see cref="Cvv"/>, or
/// <see cref="Pin"/>.
/// </summary>
public sealed class Card
{
    [JsonPropertyName("number")]
    public string Number { get; set; } = string.Empty;

    /// <summary>Two digits, e.g. <c>"01"</c>-<c>"12"</c>.</summary>
    [JsonPropertyName("expiryMonth")]
    public string ExpiryMonth { get; set; } = string.Empty;

    /// <summary>Four digits, e.g. <c>"2025"</c>.</summary>
    [JsonPropertyName("expiryYear")]
    public string ExpiryYear { get; set; } = string.Empty;

    [JsonPropertyName("pin")]
    public string Pin { get; set; } = string.Empty;

    [JsonPropertyName("cvv")]
    public string Cvv { get; set; } = string.Empty;
}
