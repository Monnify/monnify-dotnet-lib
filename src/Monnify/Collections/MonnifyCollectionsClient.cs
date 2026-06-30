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

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Collections.Transactions.Initialize)
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

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Collections.ReservedAccounts.Base)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<ReservedAccount>(httpRequest, cancellationToken);
    }

    public Task<ReservedAccount> GetReservedAccountAsync(string accountReference, CancellationToken cancellationToken = default)
    {
        RequireValue(accountReference, nameof(accountReference));
        var path = $"{MonnifyApiPaths.Collections.ReservedAccounts.Base}/{Uri.EscapeDataString(accountReference)}";
        return SendAsync<ReservedAccount>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<MonnifyPagedResult<ReservedAccountTransaction>> GetReservedAccountTransactionsAsync(
        string accountReference, int page = 0, int size = 10, CancellationToken cancellationToken = default)
    {
        RequireValue(accountReference, nameof(accountReference));
        var path = $"{MonnifyApiPaths.Collections.ReservedAccounts.BaseV1}/transactions" +
                   $"?accountReference={Uri.EscapeDataString(accountReference)}&page={page}&size={size}";
        return SendAsync<MonnifyPagedResult<ReservedAccountTransaction>>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<ReservedAccount> DeleteReservedAccountAsync(string accountReference, CancellationToken cancellationToken = default)
    {
        RequireValue(accountReference, nameof(accountReference));
        var path = $"{MonnifyApiPaths.Collections.ReservedAccounts.BaseV1}/reference/{Uri.EscapeDataString(accountReference)}";
        return SendAsync<ReservedAccount>(new HttpRequestMessage(HttpMethod.Delete, path), cancellationToken);
    }

    public Task<Invoice> CreateInvoiceAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Collections.Invoices.Create)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<Invoice>(httpRequest, cancellationToken);
    }

    public Task<Invoice> GetInvoiceAsync(string invoiceReference, CancellationToken cancellationToken = default)
    {
        RequireValue(invoiceReference, nameof(invoiceReference));
        var path = $"{MonnifyApiPaths.Collections.Invoices.Base}/{Uri.EscapeDataString(invoiceReference)}/details";
        return SendAsync<Invoice>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<MonnifyPagedResult<Invoice>> GetInvoicesAsync(int page = 0, int size = 10, CancellationToken cancellationToken = default)
    {
        var path = $"{MonnifyApiPaths.Collections.Invoices.Base}/all?page={page}&size={size}";
        return SendAsync<MonnifyPagedResult<Invoice>>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<Invoice> CancelInvoiceAsync(string invoiceReference, CancellationToken cancellationToken = default)
    {
        RequireValue(invoiceReference, nameof(invoiceReference));
        var path = $"{MonnifyApiPaths.Collections.Invoices.Base}/{Uri.EscapeDataString(invoiceReference)}/cancel";
        return SendAsync<Invoice>(new HttpRequestMessage(HttpMethod.Delete, path), cancellationToken);
    }

    public Task<BankTransferPaymentDetails> InitiateBankTransferAsync(
        InitiateBankTransferRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Collections.Transactions.InitiateBankTransfer)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<BankTransferPaymentDetails>(httpRequest, cancellationToken);
    }

    public Task<ChargeCardResult> ChargeAsync(ChargeCardRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Collections.Cards.Charge)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<ChargeCardResult>(httpRequest, cancellationToken);
    }

    public Task<AuthorizeCardOtpResult> AuthorizeOtpAsync(AuthorizeCardOtpRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Collections.Cards.AuthorizeOtp)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<AuthorizeCardOtpResult>(httpRequest, cancellationToken);
    }

    public Task<AuthorizeCardOtpResult> Authorize3dsAsync(Authorize3dsCardRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Collections.Cards.Authorize3ds)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<AuthorizeCardOtpResult>(httpRequest, cancellationToken);
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

        var path = $"{MonnifyApiPaths.Collections.Transactions.Search}?{string.Join("&", query)}";
        return SendAsync<MonnifyPagedResult<TransactionSummary>>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<Transaction> GetTransactionAsync(string transactionReference, CancellationToken cancellationToken = default)
    {
        RequireValue(transactionReference, nameof(transactionReference));
        var path = $"{MonnifyApiPaths.Collections.Transactions.ByReference}/{Uri.EscapeDataString(transactionReference)}";
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
        var path = $"{MonnifyApiPaths.Collections.Transactions.Query}?{string.Join("&", query)}";
        return SendAsync<Transaction>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<MandateActionResult> CreateMandateAsync(CreateMandateRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Collections.Mandates.Create)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<MandateActionResult>(httpRequest, cancellationToken);
    }

    public Task<IReadOnlyList<Mandate>> GetMandatesAsync(string mandateReferences, CancellationToken cancellationToken = default)
    {
        RequireValue(mandateReferences, nameof(mandateReferences));
        var path = $"{MonnifyApiPaths.Collections.Mandates.Base}?mandateReferences={Uri.EscapeDataString(mandateReferences)}";
        return SendAsync<IReadOnlyList<Mandate>>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<MandateDebitResult> DebitMandateAsync(DebitMandateRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Collections.Mandates.Debit)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<MandateDebitResult>(httpRequest, cancellationToken);
    }

    public Task<MandateDebitResult> GetMandateDebitStatusAsync(string paymentReference, CancellationToken cancellationToken = default)
    {
        RequireValue(paymentReference, nameof(paymentReference));
        var path = $"{MonnifyApiPaths.Collections.Mandates.DebitStatus}?paymentReference={Uri.EscapeDataString(paymentReference)}";
        return SendAsync<MandateDebitResult>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<MandateActionResult> CancelMandateAsync(string mandateCode, CancellationToken cancellationToken = default)
    {
        RequireValue(mandateCode, nameof(mandateCode));
        var path = $"{MonnifyApiPaths.Collections.Mandates.Cancel}/{Uri.EscapeDataString(mandateCode)}";
        // HttpMethod.Patch isn't available on netstandard2.0; construct it explicitly instead.
        return SendAsync<MandateActionResult>(new HttpRequestMessage(new HttpMethod("PATCH"), path), cancellationToken);
    }

    public Task<MandateListResult> ListMandatesAsync(
        ListMandatesFilter filter, int page = 0, int limit = 20, CancellationToken cancellationToken = default)
    {
        if (filter is null)
        {
            throw new ArgumentNullException(nameof(filter));
        }

        RequireValue(filter.StartDate, nameof(filter.StartDate));
        RequireValue(filter.EndDate, nameof(filter.EndDate));

        var query = new List<string>
        {
            $"startDate={Uri.EscapeDataString(filter.StartDate)}",
            $"endDate={Uri.EscapeDataString(filter.EndDate)}",
            $"page={page}",
            $"limit={limit}",
        };
        AppendIfPresent(query, "customerEmail", filter.CustomerEmail);
        AppendIfPresent(query, "schemeCode", filter.SchemeCode);
        AppendIfPresent(query, "mandateStatus", filter.MandateStatus);

        var path = $"{MonnifyApiPaths.Collections.Mandates.List}?{string.Join("&", query)}";
        return SendAsync<MandateListResult>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
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
