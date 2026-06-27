using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

public sealed class WebhookCustomerInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
