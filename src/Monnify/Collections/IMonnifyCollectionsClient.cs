using Monnify.Http;

namespace Monnify.Collections;

/// <remarks>
/// Registered with automatic HTTP retry disabled: <see cref="ChargeAsync"/> directly attempts to
/// debit a card and <see cref="DebitMandateAsync"/> directly debits a mandate, so an ambiguous
/// failure (timeout, network error, 5xx) on either must not be blindly resent with the same
/// details - that risks a double charge, same reasoning as
/// <see cref="Disbursements.IMonnifyDisbursementsClient"/> and <see cref="Bills.IMonnifyBillsClient"/>.
/// Query <see cref="GetTransactionAsync"/> or <see cref="GetMandateDebitStatusAsync"/> with the
/// same reference instead. Most other methods on this client don't move money directly (they set
/// up a payment instrument the customer still has to complete), so this is a more conservative
/// retry stance than they strictly need, traded for keeping every collection method - including
/// card charges and mandate debits - on one client.
/// </remarks>
public interface IMonnifyCollectionsClient
{
    /// <summary>Starts a checkout, returning a hosted page URL to redirect the customer to.</summary>
    Task<InitializeTransactionResult> InitializeTransactionAsync(
        InitializeTransactionRequest request, CancellationToken cancellationToken = default);

    /// <summary>Creates a dedicated virtual account for receiving repeat payments from a customer.</summary>
    Task<ReservedAccount> CreateReservedAccountAsync(
        CreateReservedAccountRequest request, CancellationToken cancellationToken = default);

    Task<ReservedAccount> GetReservedAccountAsync(string accountReference, CancellationToken cancellationToken = default);

    Task<MonnifyPagedResult<ReservedAccountTransaction>> GetReservedAccountTransactionsAsync(
        string accountReference, int page = 0, int size = 10, CancellationToken cancellationToken = default);

    /// <summary>Permanently deallocates a reserved account; it can no longer receive payments afterward.</summary>
    Task<ReservedAccount> DeleteReservedAccountAsync(string accountReference, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates an invoice. Supplying <see cref="CreateInvoiceRequest.AccountReference"/> attaches
    /// it to an existing reserved account (a "Static Invoice"); omitting it creates a one-off
    /// "Dynamic Invoice" instead.
    /// </summary>
    Task<Invoice> CreateInvoiceAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default);

    Task<Invoice> GetInvoiceAsync(string invoiceReference, CancellationToken cancellationToken = default);

    Task<MonnifyPagedResult<Invoice>> GetInvoicesAsync(int page = 0, int size = 10, CancellationToken cancellationToken = default);

    Task<Invoice> CancelInvoiceAsync(string invoiceReference, CancellationToken cancellationToken = default);

    /// <summary>Generates a dynamic, single-use bank account for a one-time payment against an already-initialized transaction.</summary>
    Task<BankTransferPaymentDetails> InitiateBankTransferAsync(
        InitiateBankTransferRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Charges a card (raw PAN/CVV/PIN) against a transaction reference from
    /// <see cref="InitializeTransactionAsync"/>, as an alternative to the hosted checkout flow.
    /// Depending on the card, the result is either immediately final, or requires a follow-up call
    /// to <see cref="AuthorizeOtpAsync"/> (OTP) or <see cref="Authorize3dsAsync"/> (3DS) to complete.
    /// </summary>
    Task<ChargeCardResult> ChargeAsync(ChargeCardRequest request, CancellationToken cancellationToken = default);

    /// <summary>Completes a charge that required an OTP, using the token ID from <see cref="ChargeAsync"/>'s response.</summary>
    Task<AuthorizeCardOtpResult> AuthorizeOtpAsync(AuthorizeCardOtpRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Completes a charge on a card that uses 3D Secure authentication. This is not active by
    /// default - it's restricted to PCI DSS-certified merchants and requires it to be enabled for
    /// your account first.
    /// </summary>
    Task<AuthorizeCardOtpResult> Authorize3dsAsync(Authorize3dsCardRequest request, CancellationToken cancellationToken = default);

    /// <summary>Searches transactions on the integration, optionally filtered by reference, amount range, customer, status, or date range.</summary>
    Task<MonnifyPagedResult<TransactionSummary>> SearchTransactionsAsync(
        SearchTransactionsRequest? filter = null, int page = 0, int size = 10, CancellationToken cancellationToken = default);

    /// <summary>Gets the status of a transaction by its Monnify-generated transaction reference.</summary>
    Task<Transaction> GetTransactionAsync(string transactionReference, CancellationToken cancellationToken = default);

    /// <summary>Gets the status of a transaction by either its Monnify transaction reference or the merchant's payment reference.</summary>
    Task<Transaction> QueryTransactionAsync(
        string? transactionReference = null, string? paymentReference = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Initiates a direct debit mandate - a recurring authorization to debit a customer's bank
    /// account. The mandate isn't active until the customer completes authorization (a token
    /// transfer) via <see cref="MandateActionResult.RedirectUrl"/> or the
    /// <see cref="Mandate.AuthorizationLink"/> returned by <see cref="GetMandatesAsync"/>.
    /// </summary>
    Task<MandateActionResult> CreateMandateAsync(CreateMandateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Looks up one or more mandates by reference. Despite the singular use case, the endpoint's
    /// query parameter and response are both plural - pass a single reference or several
    /// comma-separated, and always get a list back.
    /// </summary>
    Task<IReadOnlyList<Mandate>> GetMandatesAsync(string mandateReferences, CancellationToken cancellationToken = default);

    /// <summary>
    /// Debits a single payment from an active mandate. See the remarks on
    /// <see cref="IMonnifyCollectionsClient"/> for the safe retry pattern after an ambiguous
    /// failure.
    /// </summary>
    Task<MandateDebitResult> DebitMandateAsync(DebitMandateRequest request, CancellationToken cancellationToken = default);

    /// <summary>Checks the status of a previous <see cref="DebitMandateAsync"/> call by its payment reference.</summary>
    Task<MandateDebitResult> GetMandateDebitStatusAsync(string paymentReference, CancellationToken cancellationToken = default);

    /// <summary>Requests cancellation of a mandate.</summary>
    Task<MandateActionResult> CancelMandateAsync(string mandateCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists mandates created or initiated by the merchant. <paramref name="filter"/>'s
    /// <see cref="ListMandatesFilter.StartDate"/>/<see cref="ListMandatesFilter.EndDate"/> are
    /// required and the range between them must not exceed 90 days.
    /// </summary>
    Task<MandateListResult> ListMandatesAsync(
        ListMandatesFilter filter, int page = 0, int limit = 20, CancellationToken cancellationToken = default);
}
