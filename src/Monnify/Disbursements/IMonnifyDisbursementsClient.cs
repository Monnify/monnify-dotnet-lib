using Monnify.Http;

namespace Monnify.Disbursements;

/// <summary>
/// Moves money out of your Monnify wallet. The Transfer feature requires Monnify to have
/// activated it for your merchant account (contact sales@monnify.com); until then, calls to the
/// initiating methods fail with a 401/403 from Monnify.
/// </summary>
public interface IMonnifyDisbursementsClient
{
    /// <summary>Initiates a transfer to a bank account.</summary>
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

    /// <summary>Initiates a batch of transfers.</summary>
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
