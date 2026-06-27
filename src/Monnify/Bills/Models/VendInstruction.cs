using System.Text.Json.Serialization;

namespace Monnify.Bills;

public sealed class VendInstruction
{
    /// <summary>
    /// If <see langword="true"/>, <see cref="ValidationReference"/> must be passed as
    /// <see cref="VendBillRequest.ValidationReference"/> on the following <c>VendAsync</c> call.
    /// If <see langword="false"/>, omit it.
    /// </summary>
    [JsonPropertyName("requireValidationRef")]
    public bool RequireValidationRef { get; set; }

    /// <summary>
    /// Only present when <see cref="RequireValidationRef"/> is <see langword="true"/>. Confirmed
    /// against a real sandbox response (an electricity prepaid validation) - our docs sample only
    /// showed the "no reference required" case, which omits this field entirely.
    /// </summary>
    [JsonPropertyName("validationReference")]
    public string? ValidationReference { get; set; }
}
