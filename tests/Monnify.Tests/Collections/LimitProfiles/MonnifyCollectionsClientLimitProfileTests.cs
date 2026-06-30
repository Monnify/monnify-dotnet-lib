using System.Net;
using Monnify.Collections;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Collections.LimitProfiles;

public class MonnifyCollectionsClientLimitProfileTests
{
    private static MonnifyCollectionsClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    private const string LimitProfileJson = """
        {
          "limitProfileCode": "FSYVVWU8UPBD",
          "limitProfileName": "Profile0001",
          "singleTransactionValue": 2000,
          "dailyTransactionVolume": 500,
          "dailyTransactionValue": 150000,
          "dateCreated": "02/08/2022 02:27:43 AM",
          "lastModified": "02/08/2022 02:27:43 AM"
        }
        """;

    [Fact]
    public async Task CreateLimitProfileAsync_SendsPostWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK,
            $$"""{ "requestSuccessful": true, "responseMessage": "success", "responseCode": "0", "responseBody": {{LimitProfileJson}} }"""));
        var client = CreateClient(handler);

        var result = await client.CreateLimitProfileAsync(new CreateLimitProfileRequest
        {
            LimitProfileName = "Profile0001",
            SingleTransactionValue = 2000m,
            DailyTransactionValue = 150000m,
            DailyTransactionVolume = 500,
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/limit-profile/", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"limitProfileName\":\"Profile0001\"", handler.RequestBodies[0]);
        Assert.Equal("FSYVVWU8UPBD", result.LimitProfileCode);
        Assert.Equal(2000m, result.SingleTransactionValue);
        Assert.Equal(500, result.DailyTransactionVolume);
    }

    [Fact]
    public async Task CreateLimitProfileAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.CreateLimitProfileAsync(null!));
    }

    [Fact]
    public async Task GetLimitProfilesAsync_SendsGetAndDeserializesPagedResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, $$"""
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [ {{LimitProfileJson}} ],
                "pageable": null, "last": true, "totalElements": 3, "totalPages": 1,
                "sort": null, "first": true, "numberOfElements": 3, "size": 10, "number": 0, "empty": false
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetLimitProfilesAsync(page: 0, size: 10);

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/limit-profile/", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("page=0&size=10", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Single(result.Content);
        Assert.Equal(3, result.TotalElements);
        Assert.Equal("FSYVVWU8UPBD", result.Content[0].LimitProfileCode);
    }

    [Fact]
    public async Task UpdateLimitProfileAsync_SendsPutWithCodeInPathAndBodyWithoutCode()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK,
            $$"""{ "requestSuccessful": true, "responseMessage": "success", "responseCode": "0", "responseBody": {{LimitProfileJson}} }"""));
        var client = CreateClient(handler);

        var result = await client.UpdateLimitProfileAsync("FSYVVWU8UPBD", new UpdateLimitProfileRequest
        {
            LimitProfileName = "prof991",
            SingleTransactionValue = 70000m,
            DailyTransactionValue = 100000000m,
            DailyTransactionVolume = 4000,
        });

        Assert.Equal(HttpMethod.Put, handler.Requests[0].Method);
        Assert.Equal("/api/v1/limit-profile/FSYVVWU8UPBD", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"limitProfileName\":\"prof991\"", handler.RequestBodies[0]);
        Assert.DoesNotContain("limitProfileCode", handler.RequestBodies[0]);
        Assert.Equal("FSYVVWU8UPBD", result.LimitProfileCode);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task UpdateLimitProfileAsync_MissingCode_Throws(string? code)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(
            () => client.UpdateLimitProfileAsync(code!, new UpdateLimitProfileRequest()));
    }

    [Fact]
    public async Task UpdateLimitProfileAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => client.UpdateLimitProfileAsync("FSYVVWU8UPBD", null!));
    }

    [Fact]
    public async Task CreateReservedAccountWithLimitAsync_SendsPostToLimitEndpointWithLimitProfileCode()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "contractCode": "7059707855",
                "accountReference": "ref-00--7",
                "accountName": "Kan",
                "currencyCode": "NGN",
                "customerEmail": "KanYo@monnify.com",
                "customerName": "Kan Yo",
                "accountNumber": "5000578928",
                "bankName": "Moniepoint Microfinance bank",
                "bankCode": "50515",
                "collectionChannel": "RESERVED_ACCOUNT",
                "reservationReference": "0B70FP4CNC61U334XFG1",
                "reservedAccountType": "GENERAL",
                "status": "ACTIVE",
                "createdOn": "2022-08-02T02:53:25.617Z",
                "incomeSplitConfig": [],
                "restrictPaymentSource": false
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.CreateReservedAccountWithLimitAsync(new CreateReservedAccountWithLimitRequest
        {
            ContractCode = "7059707855",
            AccountName = "Kan Yo' Reserved with Limit",
            CurrencyCode = "NGN",
            AccountReference = "ref-00--7",
            CustomerEmail = "KanYo@monnify.com",
            CustomerName = "Kan Yo",
            GetAllAvailableBanks = false,
            PreferredBanks = ["50515"],
            LimitProfileCode = "FSYVVWU8UPBD",
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/bank-transfer/reserved-accounts/limit", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"limitProfileCode\":\"FSYVVWU8UPBD\"", handler.RequestBodies[0]);
        Assert.Equal("ref-00--7", result.AccountReference);
        Assert.Equal("ACTIVE", result.Status);
    }

    [Fact]
    public async Task CreateReservedAccountWithLimitAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.CreateReservedAccountWithLimitAsync(null!));
    }

    [Fact]
    public async Task UpdateReservedAccountLimitAsync_SendsPutWithReferenceAndCode()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "contractCode": "7059707855",
                "accountReference": "ref-00--7",
                "accountName": "Kan",
                "currencyCode": "NGN",
                "customerEmail": "KanYo@monnify.com",
                "customerName": "Kan Yo",
                "accountNumber": "5000578928",
                "bankName": "Moniepoint Microfinance bank",
                "bankCode": "50515",
                "collectionChannel": "RESERVED_ACCOUNT",
                "reservationReference": "0B70FP4CNC61U334XFG1",
                "reservedAccountType": "GENERAL",
                "status": "ACTIVE",
                "createdOn": "2022-08-02T02:53:25.617Z",
                "incomeSplitConfig": [],
                "restrictPaymentSource": false
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.UpdateReservedAccountLimitAsync(new UpdateReservedAccountLimitRequest
        {
            AccountReference = "ref-00--7",
            LimitProfileCode = "FSYVVWU8UPBD",
        });

        Assert.Equal(HttpMethod.Put, handler.Requests[0].Method);
        Assert.Equal("/api/v1/bank-transfer/reserved-accounts/limit", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"accountReference\":\"ref-00--7\"", handler.RequestBodies[0]);
        Assert.Contains("\"limitProfileCode\":\"FSYVVWU8UPBD\"", handler.RequestBodies[0]);
        Assert.Equal("ref-00--7", result.AccountReference);
    }

    [Fact]
    public async Task UpdateReservedAccountLimitAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.UpdateReservedAccountLimitAsync(null!));
    }
}
