using Monnify.Http;

namespace Monnify.Collections;

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

    /// <summary>Searches transactions on the integration, optionally filtered by reference, amount range, customer, status, or date range.</summary>
    Task<MonnifyPagedResult<TransactionSummary>> SearchTransactionsAsync(
        SearchTransactionsRequest? filter = null, int page = 0, int size = 10, CancellationToken cancellationToken = default);

    /// <summary>Gets the status of a transaction by its Monnify-generated transaction reference.</summary>
    Task<Transaction> GetTransactionAsync(string transactionReference, CancellationToken cancellationToken = default);

    /// <summary>Gets the status of a transaction by either its Monnify transaction reference or the merchant's payment reference.</summary>
    Task<Transaction> QueryTransactionAsync(
        string? transactionReference = null, string? paymentReference = null, CancellationToken cancellationToken = default);
}
