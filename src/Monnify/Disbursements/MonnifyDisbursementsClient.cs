using Monnify.Http;

namespace Monnify.Disbursements;

internal sealed class MonnifyDisbursementsClient : MonnifyHttpClientBase, IMonnifyDisbursementsClient
{
    public MonnifyDisbursementsClient(HttpClient httpClient) : base(httpClient) { }

    public Task<SingleTransferResult> InitiateSingleTransferAsync(
        SingleTransferRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Disbursements.Single.Initiate)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<SingleTransferResult>(httpRequest, cancellationToken);
    }

    public Task<SingleTransferResult> AuthorizeSingleTransferAsync(
        string reference, string authorizationCode, CancellationToken cancellationToken = default)
    {
        RequireValue(reference, nameof(reference));
        RequireValue(authorizationCode, nameof(authorizationCode));

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Disbursements.Single.ValidateOtp)
        {
            Content = CreateJsonContent(new { reference, authorizationCode }),
        };
        return SendAsync<SingleTransferResult>(httpRequest, cancellationToken);
    }

    public Task<DisbursementOtpResendResult> ResendSingleTransferOtpAsync(string reference, CancellationToken cancellationToken = default)
    {
        RequireValue(reference, nameof(reference));

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Disbursements.Single.ResendOtp)
        {
            Content = CreateJsonContent(new { reference }),
        };
        return SendAsync<DisbursementOtpResendResult>(httpRequest, cancellationToken);
    }

    public Task<DisbursementTransaction> GetSingleTransferAsync(string reference, CancellationToken cancellationToken = default)
    {
        RequireValue(reference, nameof(reference));
        var path = $"{MonnifyApiPaths.Disbursements.Single.Summary}?reference={Uri.EscapeDataString(reference)}";
        return SendAsync<DisbursementTransaction>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<MonnifyPagedResult<DisbursementTransaction>> GetSingleTransfersAsync(
        int pageNo = 0, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var path = $"{MonnifyApiPaths.Disbursements.Single.Transactions}?pageNo={pageNo}&pageSize={pageSize}";
        return SendAsync<MonnifyPagedResult<DisbursementTransaction>>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<BulkTransferResult> InitiateBulkTransferAsync(BulkTransferRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Disbursements.Bulk.Initiate)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<BulkTransferResult>(httpRequest, cancellationToken);
    }

    public Task<BulkTransferResult> AuthorizeBulkTransferAsync(
        string reference, string authorizationCode, CancellationToken cancellationToken = default)
    {
        RequireValue(reference, nameof(reference));
        RequireValue(authorizationCode, nameof(authorizationCode));

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Disbursements.Bulk.ValidateOtp)
        {
            Content = CreateJsonContent(new { reference, authorizationCode }),
        };
        return SendAsync<BulkTransferResult>(httpRequest, cancellationToken);
    }

    public Task<DisbursementOtpResendResult> ResendBulkTransferOtpAsync(string batchReference, CancellationToken cancellationToken = default)
    {
        RequireValue(batchReference, nameof(batchReference));

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Disbursements.Bulk.ResendOtp)
        {
            Content = CreateJsonContent(new { batchReference }),
        };
        return SendAsync<DisbursementOtpResendResult>(httpRequest, cancellationToken);
    }

    public Task<BatchSummary> GetBulkTransferSummaryAsync(string batchReference, CancellationToken cancellationToken = default)
    {
        RequireValue(batchReference, nameof(batchReference));
        var path = $"{MonnifyApiPaths.Disbursements.Bulk.Summary}?reference={Uri.EscapeDataString(batchReference)}";
        return SendAsync<BatchSummary>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<MonnifyPagedResult<DisbursementTransaction>> GetBulkTransferTransactionsAsync(
        string batchReference, CancellationToken cancellationToken = default)
    {
        RequireValue(batchReference, nameof(batchReference));
        var path = $"{MonnifyApiPaths.Disbursements.Bulk.TransactionsBase}/{Uri.EscapeDataString(batchReference)}/transactions";
        return SendAsync<MonnifyPagedResult<DisbursementTransaction>>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<MonnifyPagedResult<DisbursementTransaction>> SearchTransactionsAsync(
        string sourceAccountNumber, DisbursementTransactionSearchFilter? filter = null,
        int pageNo = 0, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        RequireValue(sourceAccountNumber, nameof(sourceAccountNumber));

        var query = new List<string>
        {
            $"sourceAccountNumber={Uri.EscapeDataString(sourceAccountNumber)}",
            $"pageNo={pageNo}",
            $"pageSize={pageSize}",
        };
        if (filter is not null)
        {
            AppendIfPresent(query, "startDate", filter.StartDate?.ToString(System.Globalization.CultureInfo.InvariantCulture));
            AppendIfPresent(query, "endDate", filter.EndDate?.ToString(System.Globalization.CultureInfo.InvariantCulture));
            AppendIfPresent(query, "amountFrom", filter.AmountFrom?.ToString(System.Globalization.CultureInfo.InvariantCulture));
            AppendIfPresent(query, "amountTo", filter.AmountTo?.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        var path = $"{MonnifyApiPaths.Disbursements.SearchTransactions}?{string.Join("&", query)}";
        return SendAsync<MonnifyPagedResult<DisbursementTransaction>>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<WalletBalance> GetWalletBalanceAsync(string accountNumber, CancellationToken cancellationToken = default)
    {
        RequireValue(accountNumber, nameof(accountNumber));
        var path = $"{MonnifyApiPaths.Disbursements.WalletBalance}?accountNumber={Uri.EscapeDataString(accountNumber)}";
        return SendAsync<WalletBalance>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
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
