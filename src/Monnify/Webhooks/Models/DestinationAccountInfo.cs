using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

public sealed class DestinationAccountInfo
{
    [JsonPropertyName("bankCode")]
    public string? BankCode { get; set; }

    [JsonPropertyName("bankName")]
    public string? BankName { get; set; }

    [JsonPropertyName("accountNumber")]
    public string? AccountNumber { get; set; }
}
