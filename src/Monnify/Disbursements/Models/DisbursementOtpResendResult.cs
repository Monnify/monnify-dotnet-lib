using System.Text.Json.Serialization;

namespace Monnify.Disbursements;

public sealed class DisbursementOtpResendResult
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
}
