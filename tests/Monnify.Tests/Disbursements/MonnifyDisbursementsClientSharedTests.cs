using System.Net;
using Monnify.Disbursements;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Disbursements;

public class MonnifyDisbursementsClientSharedTests
{
    private static MonnifyDisbursementsClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    [Fact]
    public async Task SearchTransactionsAsync_SendsGetWithSourceAccountAndFilters()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": { "content": [], "totalElements": 0, "totalPages": 0, "number": 0, "size": 3, "first": true, "last": true } }
            """));
        var client = CreateClient(handler);

        await client.SearchTransactionsAsync(
            "9988776655",
            new DisbursementTransactionSearchFilter { AmountFrom = 10, AmountTo = 100 },
            pageNo: 0, pageSize: 3);

        Assert.Equal("/api/v2/disbursements/search-transactions", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("sourceAccountNumber=9988776655&pageNo=0&pageSize=3&amountFrom=10&amountTo=100",
            handler.Requests[0].RequestUri!.Query.TrimStart('?'));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task SearchTransactionsAsync_MissingSourceAccountNumber_Throws(string? sourceAccountNumber)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.SearchTransactionsAsync(sourceAccountNumber!));
    }

    [Fact]
    public async Task GetWalletBalanceAsync_SendsGetWithAccountNumberQueryParam_AndAcceptsNumericBalance()
    {
        var handler = new FakeHttpMessageHandler();
        // Sample payload from a real sandbox call - note availableBalance/ledgerBalance come back
        // as plain JSON numbers here, even though Monnify's own docs show them quoted as strings.
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": { "availableBalance": 3284590355.36, "ledgerBalance": 3284590355.36 } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetWalletBalanceAsync("9988776655");

        Assert.Equal("/api/v2/disbursements/wallet-balance", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("accountNumber=9988776655", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Equal(3284590355.36m, result.AvailableBalance);
    }

    [Fact]
    public async Task GetWalletBalanceAsync_AcceptsQuotedStringBalance()
    {
        // Per Monnify's own documented sample for this endpoint.
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": { "availableBalance": "4919798101.36", "ledgerBalance": "4919798101.36" } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetWalletBalanceAsync("9988776655");

        Assert.Equal(4919798101.36m, result.AvailableBalance);
        Assert.Equal(4919798101.36m, result.LedgerBalance);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetWalletBalanceAsync_MissingAccountNumber_Throws(string? accountNumber)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetWalletBalanceAsync(accountNumber!));
    }
}
