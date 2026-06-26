using System.Net;
using Monnify.Collections;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Collections.ReservedAccounts;

public class MonnifyCollectionsClientReservedAccountsTests
{
    private static MonnifyCollectionsClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    private const string SampleReservedAccount = """
        { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
          "responseBody": {
            "contractCode": "1234567890",
            "accountReference": "acct-ref-1",
            "accountName": "Ada Lovelace Wallet",
            "currencyCode": "NGN",
            "customerEmail": "ada@example.com",
            "customerName": "Ada Lovelace",
            "accounts": [
              { "bankCode": "035", "bankName": "Wema bank", "accountNumber": "1003823063", "accountName": "Ada Lovelace Wallet" },
              { "bankCode": "232", "bankName": "Sterling bank", "accountNumber": "2207879758", "accountName": "Ada Lovelace Wallet" }
            ],
            "collectionChannel": "RESERVED_ACCOUNT",
            "reservationReference": "17ERLN8RZCZ203H00044",
            "reservedAccountType": "GENERAL",
            "status": "ACTIVE",
            "createdOn": "2026-06-26 23:19:13.511864338",
            "incomeSplitConfig": [],
            "restrictPaymentSource": false
          } }
        """;

    [Fact]
    public async Task CreateReservedAccountAsync_SendsPostToV2Endpoint_AndDeserializesMultiBankAccounts()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, SampleReservedAccount));
        var client = CreateClient(handler);

        var result = await client.CreateReservedAccountAsync(new CreateReservedAccountRequest
        {
            AccountReference = "acct-ref-1",
            AccountName = "Ada Lovelace Wallet",
            ContractCode = "1234567890",
            CustomerEmail = "ada@example.com",
            CustomerName = "Ada Lovelace",
            GetAllAvailableBanks = true,
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v2/bank-transfer/reserved-accounts", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"getAllAvailableBanks\":true", handler.RequestBodies[0]);
        Assert.Equal(2, result.Accounts.Count);
        Assert.Equal("Wema bank", result.Accounts[0].BankName);
    }

    [Fact]
    public async Task CreateReservedAccountAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.CreateReservedAccountAsync(null!));
    }

    [Fact]
    public async Task GetReservedAccountAsync_SendsGetToV2Endpoint_WithReferenceInPath()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, SampleReservedAccount));
        var client = CreateClient(handler);

        var result = await client.GetReservedAccountAsync("acct-ref-1");

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v2/bank-transfer/reserved-accounts/acct-ref-1", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("acct-ref-1", result.AccountReference);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetReservedAccountAsync_MissingReference_Throws(string? reference)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetReservedAccountAsync(reference!));
    }

    [Fact]
    public async Task GetReservedAccountTransactionsAsync_SendsGetToV1Endpoint_WithQueryParams()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": { "content": [], "totalElements": 0, "totalPages": 0, "number": 0, "size": 10, "first": true, "last": true } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetReservedAccountTransactionsAsync("acct-ref-1", page: 2, size: 5);

        Assert.Equal("/api/v1/bank-transfer/reserved-accounts/transactions", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("accountReference=acct-ref-1&page=2&size=5", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Empty(result.Content);
    }

    [Fact]
    public async Task DeleteReservedAccountAsync_SendsDeleteToV1ReferencePath()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, SampleReservedAccount));
        var client = CreateClient(handler);

        await client.DeleteReservedAccountAsync("acct-ref-1");

        Assert.Equal(HttpMethod.Delete, handler.Requests[0].Method);
        Assert.Equal("/api/v1/bank-transfer/reserved-accounts/reference/acct-ref-1", handler.Requests[0].RequestUri!.AbsolutePath);
    }
}
