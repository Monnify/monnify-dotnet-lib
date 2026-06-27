using Monnify.Http;

namespace Monnify.Bills;

/// <summary>
/// Bills payment (airtime, data, cable TV, electricity, etc.) via our biller network. We document
/// this as <b>not active by default</b> for all merchants - reach out to
/// integration-support@monnify.com to have it activated - though the sandbox account this client
/// was verified against had no trouble calling any of these methods without a separate activation
/// step, so that requirement may not be universal. If you hit a 401/403, that's the first thing to
/// check.
/// </summary>
/// <remarks>
/// This client is registered with automatic HTTP retry disabled, for the same reason as
/// <see cref="Disbursements.IMonnifyDisbursementsClient"/>: <see cref="VendAsync"/> moves money
/// and isn't safe to blindly resend. If a vend fails ambiguously (timeout, network error, 5xx),
/// call <see cref="RequeryAsync"/> with the <em>same</em> reference to find out what actually
/// happened. Only retry with a <em>new</em> reference if that confirms the vend never went
/// through.
/// </remarks>
public interface IMonnifyBillsClient
{
    /// <summary>
    /// Lists biller categories (e.g. cable TV, data, electricity). Paging is 0-based - confirmed
    /// against the real sandbox, which contradicts our own docs (we currently document a 1-based
    /// default of <c>page=1</c>; passing that against the real API silently skips the first page).
    /// </summary>
    Task<MonnifyPagedResult<BillerCategory>> GetBillerCategoriesAsync(
        int page = 0, int size = 20, CancellationToken cancellationToken = default);

    /// <summary>Lists billers, optionally filtered to one category. See the 0-based paging note on <see cref="GetBillerCategoriesAsync"/>.</summary>
    Task<MonnifyPagedResult<Biller>> GetBillersAsync(
        string? categoryCode = null, int page = 0, int size = 20, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists the products a specific biller offers (e.g. each DSTV bouquet, or each MTN data plan).
    /// See the 0-based paging note on <see cref="GetBillerCategoriesAsync"/>.
    /// </summary>
    /// <param name="billerCode">
    /// The biller to list products for (e.g. <c>biller-dstv</c>, or a plain code like <c>AIRTEL</c>
    /// for some billers - confirmed inconsistent by direct sandbox calls). We don't explicitly mark
    /// this query parameter required in our docs, but the endpoint's own description centers
    /// entirely on "products for a specific biller" - treated as required here.
    /// </param>
    /// <param name="page">0-based page index.</param>
    /// <param name="size">Page size.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    Task<MonnifyPagedResult<BillerProduct>> GetBillerProductsAsync(
        string billerCode, int page = 0, int size = 20, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates a customer identifier (meter number, phone number, etc.) for a product before
    /// vending. Check the result's <see cref="ValidateBillCustomerResult.VendInstruction"/> -
    /// if <see cref="VendInstruction.RequireValidationRef"/> is true, pass
    /// <see cref="VendInstruction.ValidationReference"/> into the following
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
