using Monnify.Http;

namespace Monnify.Disbursements;

/// <summary>
/// Moves money out of your Monnify wallet. The Transfer feature requires Monnify to have
/// activated it for your merchant account (contact sales@monnify.com); until then, calls to the
/// initiating methods fail with a 401/403 from Monnify.
/// </summary>
/// <remarks>
/// This client is registered with automatic HTTP retry disabled: if a transfer-initiating call
/// fails ambiguously (timeout, network error, 5xx), do not resend the same request. Instead, call
/// <see cref="GetSingleTransferAsync"/> or <see cref="GetBulkTransferSummaryAsync"/> with the
/// <em>same</em> reference to find out what actually happened. Only if that confirms the transfer
/// never went through should you retry - and a retry must use a <em>new</em> reference, since
/// Monnify treats <c>reference</c>/<c>batchReference</c> as an idempotency key: resending the same
/// one either gets rejected as a duplicate or just returns the original (possibly still-failed)
/// result, rather than giving the transfer a fresh attempt.
/// </remarks>
public interface IMonnifyDisbursementsClient
{
    /// <summary>
    /// Initiates a transfer to a bank account. See the remarks on <see cref="IMonnifyDisbursementsClient"/>
    /// for how to safely retry after an ambiguous failure.
    /// </summary>
    Task<SingleTransferResult> InitiateSingleTransferAsync(
        SingleTransferRequest request, CancellationToken cancellationToken = default);

    /// <summary>Authorizes a single transfer that is awaiting OTP authorization (status <c>PENDING_AUTHORIZATION</c>).</summary>
    Task<SingleTransferResult> AuthorizeSingleTransferAsync(
        string reference, string authorizationCode, CancellationToken cancellationToken = default);

    /// <summary>Requests a new OTP for a single transfer awaiting authorization.</summary>
    Task<DisbursementOtpResendResult> ResendSingleTransferOtpAsync(string reference, CancellationToken cancellationToken = default);

    Task<DisbursementTransaction> GetSingleTransferAsync(string reference, CancellationToken cancellationToken = default);

    Task<MonnifyPagedResult<DisbursementTransaction>> GetSingleTransfersAsync(
        int pageNo = 0, int pageSize = 20, CancellationToken cancellationToken = default);

    /// <summary>
    /// Initiates a batch of transfers. See the remarks on <see cref="IMonnifyDisbursementsClient"/>
    /// for how to safely retry after an ambiguous failure.
    /// </summary>
    Task<BulkTransferResult> InitiateBulkTransferAsync(BulkTransferRequest request, CancellationToken cancellationToken = default);

    /// <summary>Authorizes a batch transfer that is awaiting OTP authorization.</summary>
    Task<BulkTransferResult> AuthorizeBulkTransferAsync(
        string reference, string authorizationCode, CancellationToken cancellationToken = default);

    /// <summary>Requests a new OTP for a batch transfer awaiting authorization.</summary>
    Task<DisbursementOtpResendResult> ResendBulkTransferOtpAsync(string batchReference, CancellationToken cancellationToken = default);

    Task<BatchSummary> GetBulkTransferSummaryAsync(string batchReference, CancellationToken cancellationToken = default);

    Task<MonnifyPagedResult<DisbursementTransaction>> GetBulkTransferTransactionsAsync(
        string batchReference, int pageNo = 0, int pageSize = 20, CancellationToken cancellationToken = default);

    /// <summary>Searches disbursement transactions on the given wallet, optionally filtered by date range or amount range.</summary>
    Task<MonnifyPagedResult<DisbursementTransaction>> SearchTransactionsAsync(
        string sourceAccountNumber, DisbursementTransactionSearchFilter? filter = null,
        int pageNo = 0, int pageSize = 10, CancellationToken cancellationToken = default);

    Task<WalletBalance> GetWalletBalanceAsync(string accountNumber, CancellationToken cancellationToken = default);
}
