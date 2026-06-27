using System.Text.Json.Serialization;

namespace Monnify.Bills;

public sealed class VendInstruction
{
    /// <summary>
    /// If <see langword="true"/>, the <see cref="ValidateBillCustomerResult.ValidationReference"/>
    /// from this validation must be passed as <see cref="VendBillRequest.ValidationReference"/> on
    /// the following <c>VendAsync</c> call. If <see langword="false"/>, omit it.
    /// </summary>
    [JsonPropertyName("requireValidationRef")]
    public bool RequireValidationRef { get; set; }
}
