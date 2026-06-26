using Monnify.Http;

namespace Monnify.Collections;

internal sealed class MonnifyCollectionsClient : MonnifyHttpClientBase, IMonnifyCollectionsClient
{
    public MonnifyCollectionsClient(HttpClient httpClient) : base(httpClient) { }

    public Task<InitializeTransactionResult> InitializeTransactionAsync(
        InitializeTransactionRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Collections.InitializeTransaction)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<InitializeTransactionResult>(httpRequest, cancellationToken);
    }

    public Task<ReservedAccount> CreateReservedAccountAsync(
        CreateReservedAccountRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Collections.ReservedAccountsBase)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<ReservedAccount>(httpRequest, cancellationToken);
    }

    public Task<ReservedAccount> GetReservedAccountAsync(string accountReference, CancellationToken cancellationToken = default)
    {
        RequireValue(accountReference, nameof(accountReference));
        var path = $"{MonnifyApiPaths.Collections.ReservedAccountsBase}/{Uri.EscapeDataString(accountReference)}";
        return SendAsync<ReservedAccount>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<MonnifyPagedResult<ReservedAccountTransaction>> GetReservedAccountTransactionsAsync(
        string accountReference, int page = 0, int size = 10, CancellationToken cancellationToken = default)
    {
        RequireValue(accountReference, nameof(accountReference));
        var path = $"{MonnifyApiPaths.Collections.ReservedAccountsBaseV1}/transactions" +
                   $"?accountReference={Uri.EscapeDataString(accountReference)}&page={page}&size={size}";
        return SendAsync<MonnifyPagedResult<ReservedAccountTransaction>>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<ReservedAccount> DeleteReservedAccountAsync(string accountReference, CancellationToken cancellationToken = default)
    {
        RequireValue(accountReference, nameof(accountReference));
        var path = $"{MonnifyApiPaths.Collections.ReservedAccountsBaseV1}/reference/{Uri.EscapeDataString(accountReference)}";
        return SendAsync<ReservedAccount>(new HttpRequestMessage(HttpMethod.Delete, path), cancellationToken);
    }

    public Task<Invoice> CreateInvoiceAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Collections.CreateInvoice)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<Invoice>(httpRequest, cancellationToken);
    }

    public Task<Invoice> GetInvoiceAsync(string invoiceReference, CancellationToken cancellationToken = default)
    {
        RequireValue(invoiceReference, nameof(invoiceReference));
        var path = $"{MonnifyApiPaths.Collections.InvoiceBase}/{Uri.EscapeDataString(invoiceReference)}/details";
        return SendAsync<Invoice>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<MonnifyPagedResult<Invoice>> GetInvoicesAsync(int page = 0, int size = 10, CancellationToken cancellationToken = default)
    {
        var path = $"{MonnifyApiPaths.Collections.InvoiceBase}/all?page={page}&size={size}";
        return SendAsync<MonnifyPagedResult<Invoice>>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<Invoice> CancelInvoiceAsync(string invoiceReference, CancellationToken cancellationToken = default)
    {
        RequireValue(invoiceReference, nameof(invoiceReference));
        var path = $"{MonnifyApiPaths.Collections.InvoiceBase}/{Uri.EscapeDataString(invoiceReference)}/cancel";
        return SendAsync<Invoice>(new HttpRequestMessage(HttpMethod.Delete, path), cancellationToken);
    }

    private static void RequireValue(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value must be provided.", paramName);
        }
    }
}
