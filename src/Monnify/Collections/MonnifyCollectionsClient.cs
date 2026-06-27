using System.Globalization;
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

    public Task<BankTransferPaymentDetails> InitiateBankTransferAsync(
        InitiateBankTransferRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Collections.InitiateBankTransfer)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<BankTransferPaymentDetails>(httpRequest, cancellationToken);
    }

    public Task<MonnifyPagedResult<TransactionSummary>> SearchTransactionsAsync(
        SearchTransactionsRequest? filter = null, int page = 0, int size = 10, CancellationToken cancellationToken = default)
    {
        var query = new List<string> { $"page={page}", $"size={size}" };
        if (filter is not null)
        {
            AppendIfPresent(query, "paymentReference", filter.PaymentReference);
            AppendIfPresent(query, "transactionReference", filter.TransactionReference);
            AppendIfPresent(query, "fromAmount", filter.FromAmount?.ToString(CultureInfo.InvariantCulture));
            AppendIfPresent(query, "toAmount", filter.ToAmount?.ToString(CultureInfo.InvariantCulture));
            AppendIfPresent(query, "amount", filter.Amount?.ToString(CultureInfo.InvariantCulture));
            AppendIfPresent(query, "customerName", filter.CustomerName);
            AppendIfPresent(query, "customerEmail", filter.CustomerEmail);
            AppendIfPresent(query, "paymentStatus", filter.PaymentStatus);
            AppendIfPresent(query, "from", filter.From?.ToString(CultureInfo.InvariantCulture));
            AppendIfPresent(query, "to", filter.To?.ToString(CultureInfo.InvariantCulture));
        }

        var path = $"{MonnifyApiPaths.Collections.SearchTransactions}?{string.Join("&", query)}";
        return SendAsync<MonnifyPagedResult<TransactionSummary>>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<Transaction> GetTransactionAsync(string transactionReference, CancellationToken cancellationToken = default)
    {
        RequireValue(transactionReference, nameof(transactionReference));
        var path = $"{MonnifyApiPaths.Collections.TransactionByReference}/{Uri.EscapeDataString(transactionReference)}";
        return SendAsync<Transaction>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<Transaction> QueryTransactionAsync(
        string? transactionReference = null, string? paymentReference = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(transactionReference) && string.IsNullOrWhiteSpace(paymentReference))
        {
            throw new ArgumentException("Either transactionReference or paymentReference must be provided.", nameof(transactionReference));
        }

        var query = new List<string>();
        AppendIfPresent(query, "transactionReference", transactionReference);
        AppendIfPresent(query, "paymentReference", paymentReference);
        var path = $"{MonnifyApiPaths.Collections.QueryTransaction}?{string.Join("&", query)}";
        return SendAsync<Transaction>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    private static void AppendIfPresent(List<string> query, string key, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            query.Add($"{key}={Uri.EscapeDataString(value)}");
        }
    }

    private static void RequireValue(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value must be provided.", paramName);
        }
    }
}
