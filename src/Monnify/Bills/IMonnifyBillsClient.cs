using Monnify.Http;

namespace Monnify.Bills;

/// <summary>
/// Bills payment (airtime, data, cable TV, electricity, etc.) via Monnify's biller network.
/// <b>Not active by default</b> for all merchants - email integration-support@monnify.com to
/// have it activated on your account before calling any of these methods.
/// </summary>
/// <remarks>
/// <para>
/// This client is registered with automatic HTTP retry disabled, for the same reason as
/// <see cref="Disbursements.IMonnifyDisbursementsClient"/>: <see cref="VendAsync"/> moves money
/// and isn't safe to blindly resend. If a vend fails ambiguously (timeout, network error, 5xx),
/// call <see cref="RequeryAsync"/> with the <em>same</em> reference to find out what actually
/// happened. Only retry with a <em>new</em> reference if that confirms the vend never went
/// through.
/// </para>
/// <para>
/// Unlike every other paginated list in this SDK (which is 0-based), Monnify's own documented
/// default for <see cref="GetBillerCategoriesAsync"/>, <see cref="GetBillersAsync"/>, and
/// <see cref="GetBillerProductsAsync"/> is <c>page=1</c> (1-based) - that's Monnify's own Bills
/// docs, not a typo carried over from the other clients.
/// </para>
/// </remarks>
public interface IMonnifyBillsClient
{
    /// <summary>Lists biller categories (e.g. cable TV, data, electricity).</summary>
    Task<MonnifyPagedResult<BillerCategory>> GetBillerCategoriesAsync(
        int page = 1, int size = 10, CancellationToken cancellationToken = default);

    /// <summary>Lists billers, optionally filtered to one category.</summary>
    Task<MonnifyPagedResult<Biller>> GetBillersAsync(
        string? categoryCode = null, int page = 1, int size = 20, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists the products a specific biller offers (e.g. each DSTV bouquet, or each MTN data plan).
    /// <paramref name="billerCode"/> (e.g. <c>biller-dstv</c>) isn't explicitly marked required in
    /// Monnify's docs, but the endpoint's own description centers entirely on "products for a
    /// specific biller" - treated as required here pending sandbox verification.
    /// </summary>
    Task<MonnifyPagedResult<BillerProduct>> GetBillerProductsAsync(
        string billerCode, int page = 1, int size = 20, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates a customer identifier (meter number, phone number, etc.) for a product before
    /// vending. Check the result's <see cref="ValidateBillCustomerResult.VendInstruction"/> -
    /// if <see cref="VendInstruction.RequireValidationRef"/> is true, pass
    /// <see cref="ValidateBillCustomerResult.ValidationReference"/> into the following
    /// <see cref="VendAsync"/> call; if false, omit it.
    /// </summary>
    Task<ValidateBillCustomerResult> ValidateCustomerAsync(
        ValidateBillCustomerRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Pays for/vends a product (airtime top-up, bill payment, etc.). See the remarks on
    /// <see cref="IMonnifyBillsClient"/> for how to safely retry after an ambiguous failure.
    /// </summary>
    Task<VendBillResult> VendAsync(VendBillRequest request, CancellationToken cancellationToken = default);

    /// <summary>Checks the final status of a previously initiated vend by its reference.</summary>
    Task<VendBillResult> RequeryAsync(string reference, CancellationToken cancellationToken = default);
}
