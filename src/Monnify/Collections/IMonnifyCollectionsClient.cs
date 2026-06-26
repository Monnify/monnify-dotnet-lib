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
}
