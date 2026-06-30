using System.Net;
using Monnify.Collections;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Collections.SubAccounts;

public class MonnifyCollectionsClientSubAccountTests
{
    private static MonnifyCollectionsClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    [Fact]
    public async Task CreateSubAccountsAsync_SendsPostWithArrayBody_AndDeserializesArray()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": [
                {
                  "subAccountCode": "MFY_SUB_811397375865",
                  "accountNumber": "0211319282",
                  "accountName": "ALEMOH DANIEL MOSES",
                  "currencyCode": "NGN",
                  "email": "tamira1@gmail.com",
                  "bankCode": "058",
                  "bankName": "GTBank",
                  "defaultSplitPercentage": 20.87,
                  "settlementProfileCode": "8717495899",
                  "settlementReportEmails": []
                }
              ] }
            """));
        var client = CreateClient(handler);

        var result = await client.CreateSubAccountsAsync(new[]
        {
            new CreateSubAccountRequest
            {
                CurrencyCode = "NGN",
                AccountNumber = "0211319282",
                BankCode = "058",
                Email = "tamira1@gmail.com",
                DefaultSplitPercentage = 20.87m,
            },
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/sub-accounts", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"accountNumber\":\"0211319282\"", handler.RequestBodies[0]);
        Assert.Single(result);
        Assert.Equal("MFY_SUB_811397375865", result[0].SubAccountCode);
        Assert.Equal("ALEMOH DANIEL MOSES", result[0].AccountName);
        Assert.Equal(20.87m, result[0].DefaultSplitPercentage);
        Assert.Equal("8717495899", result[0].SettlementProfileCode);
    }

    [Fact]
    public async Task CreateSubAccountsAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.CreateSubAccountsAsync(null!));
    }

    [Fact]
    public async Task GetSubAccountsAsync_SendsGetAndDeserializesArray()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": [
                {
                  "subAccountCode": "MFY_SUB_811397375865",
                  "accountNumber": "0211319282",
                  "accountName": "ALEMOH DANIEL MOSES",
                  "currencyCode": "NGN",
                  "email": "tamira1@gmail.com",
                  "bankCode": "058",
                  "bankName": "GTBank",
                  "defaultSplitPercentage": 20.87
                }
              ] }
            """));
        var client = CreateClient(handler);

        var result = await client.GetSubAccountsAsync();

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/sub-accounts", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Single(result);
        Assert.Equal("MFY_SUB_811397375865", result[0].SubAccountCode);
        Assert.Null(result[0].SettlementProfileCode);
    }

    [Fact]
    public async Task UpdateSubAccountAsync_SendsPutWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "subAccountCode": "MFY_SUB_811397375865",
                "accountNumber": "0211319282",
                "accountName": "ALEMOH DANIEL MOSES",
                "currencyCode": "NGN",
                "email": "kali@gmail.com",
                "bankCode": "058",
                "bankName": "GTBank",
                "defaultSplitPercentage": 25,
                "settlementProfileCode": "8717495899",
                "settlementReportEmails": []
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.UpdateSubAccountAsync(new UpdateSubAccountRequest
        {
            SubAccountCode = "MFY_SUB_811397375865",
            CurrencyCode = "NGN",
            AccountNumber = "0211319282",
            BankCode = "058",
            Email = "kali@gmail.com",
            DefaultSplitPercentage = 25m,
        });

        Assert.Equal(HttpMethod.Put, handler.Requests[0].Method);
        Assert.Equal("/api/v1/sub-accounts", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"subAccountCode\":\"MFY_SUB_811397375865\"", handler.RequestBodies[0]);
        Assert.Equal("kali@gmail.com", result.Email);
        Assert.Equal(25m, result.DefaultSplitPercentage);
    }

    [Fact]
    public async Task UpdateSubAccountAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.UpdateSubAccountAsync(null!));
    }

    [Fact]
    public async Task DeleteSubAccountAsync_SendsDeleteWithCodeInPath_AndSucceedsWithNoResponseBody()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0" }
            """));
        var client = CreateClient(handler);

        await client.DeleteSubAccountAsync("MFY_SUB_811397375865");

        Assert.Equal(HttpMethod.Delete, handler.Requests[0].Method);
        Assert.Equal("/api/v1/sub-accounts/MFY_SUB_811397375865", handler.Requests[0].RequestUri!.AbsolutePath);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task DeleteSubAccountAsync_MissingSubAccountCode_Throws(string? subAccountCode)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.DeleteSubAccountAsync(subAccountCode!));
    }
}
