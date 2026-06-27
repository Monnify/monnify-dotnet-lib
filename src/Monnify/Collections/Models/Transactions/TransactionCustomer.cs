using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class TransactionCustomer
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
