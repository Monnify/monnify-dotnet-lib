using Monnify.Http;

namespace Monnify.Bills;

internal sealed class MonnifyBillsClient : MonnifyHttpClientBase, IMonnifyBillsClient
{
    public MonnifyBillsClient(HttpClient httpClient) : base(httpClient) { }

    public Task<MonnifyPagedResult<BillerCategory>> GetBillerCategoriesAsync(
        int page = 1, int size = 10, CancellationToken cancellationToken = default)
    {
        var path = $"{MonnifyApiPaths.Bills.BillerCategories}?page={page}&size={size}";
        return SendAsync<MonnifyPagedResult<BillerCategory>>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<MonnifyPagedResult<Biller>> GetBillersAsync(
        string? categoryCode = null, int page = 1, int size = 20, CancellationToken cancellationToken = default)
    {
        var query = new List<string> { $"page={page}", $"size={size}" };
        if (!string.IsNullOrWhiteSpace(categoryCode))
        {
            query.Add($"category_code={Uri.EscapeDataString(categoryCode)}");
        }

        var path = $"{MonnifyApiPaths.Bills.Billers}?{string.Join("&", query)}";
        return SendAsync<MonnifyPagedResult<Biller>>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<MonnifyPagedResult<BillerProduct>> GetBillerProductsAsync(
        string billerCode, int page = 1, int size = 20, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(billerCode))
        {
            throw new ArgumentException("Biller code must be provided.", nameof(billerCode));
        }

        var path = $"{MonnifyApiPaths.Bills.BillerProducts}?biller_code={Uri.EscapeDataString(billerCode)}&page={page}&size={size}";
        return SendAsync<MonnifyPagedResult<BillerProduct>>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<ValidateBillCustomerResult> ValidateCustomerAsync(
        ValidateBillCustomerRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Bills.ValidateCustomer)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<ValidateBillCustomerResult>(httpRequest, cancellationToken);
    }

    public Task<VendBillResult> VendAsync(VendBillRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Bills.Vend)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<VendBillResult>(httpRequest, cancellationToken);
    }

    public Task<VendBillResult> RequeryAsync(string reference, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            throw new ArgumentException("Reference must be provided.", nameof(reference));
        }

        var path = $"{MonnifyApiPaths.Bills.Requery}?reference={Uri.EscapeDataString(reference)}";
        return SendAsync<VendBillResult>(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }
}
