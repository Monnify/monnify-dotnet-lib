using System.Net;
using Monnify.Collections;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Collections.Paycodes;

public class MonnifyCollectionsClientPaycodeTests
{
    private static MonnifyCollectionsClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    private const string PaycodeJson = """
        {
          "paycode": "11467409",
          "transactionReference": "MFY-39A78F78E6C341759ACA344297A8CF70",
          "paycodeReference": "ghehdekdkefkefjekjfejj",
          "beneficiaryName": "Marvelous Benji",
          "amount": 50,
          "fee": 100,
          "transactionStatus": "PENDING",
          "expiryDate": "2023-02-19 11:00:26",
          "createdOn": "2023-02-16T12:32:01.591+0000",
          "createdBy": "MK_PROD_WTZLS10MX6",
          "modifiedBy": "MK_PROD_WTZLS10MX6"
        }
        """;

    private static string EnvelopeOf(string body) =>
        $$$"""{ "requestSuccessful": true, "responseMessage": "success", "responseCode": "0", "responseBody": {{{body}}} }""";

    // ── CreatePaycodeAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task CreatePaycodeAsync_SendsPostWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, EnvelopeOf(PaycodeJson)));
        var client = CreateClient(handler);

        var result = await client.CreatePaycodeAsync(new CreatePaycodeRequest
        {
            BeneficiaryName = "Marvelous Benji",
            Amount = 50,
            PaycodeReference = "ghehdekdkefkefjekjfejj",
            ExpiryDate = "2023-10-18 19:00:26",
            ClientId = "MK_PROD_GFVLE0PZTQ",
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/paycode", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"paycodeReference\":\"ghehdekdkefkefjekjfejj\"", handler.RequestBodies[0]);
        Assert.Contains("\"clientId\":\"MK_PROD_GFVLE0PZTQ\"", handler.RequestBodies[0]);
        Assert.Equal("11467409", result.PaycodeValue);
        Assert.Equal(50m, result.Amount);
        Assert.Equal("PENDING", result.TransactionStatus);
    }

    [Fact]
    public async Task CreatePaycodeAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.CreatePaycodeAsync(null!));
    }

    // ── GetPaycodesAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetPaycodesAsync_NoFilter_SendsGetWithNoQueryString()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, $$"""
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [ {{PaycodeJson}} ],
                "last": true, "totalElements": 1, "totalPages": 1,
                "first": true, "numberOfElements": 1, "size": 10, "number": 0, "empty": false
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetPaycodesAsync();

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/paycode", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("", handler.Requests[0].RequestUri!.Query);
        Assert.Single(result.Content);
        Assert.Equal("ghehdekdkefkefjekjfejj", result.Content[0].PaycodeReference);
    }

    [Fact]
    public async Task GetPaycodesAsync_WithAllFilters_IncludesAllQueryParams()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, $$"""
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [], "last": true, "totalElements": 0, "totalPages": 0,
                "first": true, "numberOfElements": 0, "size": 10, "number": 0, "empty": true
              } }
            """));
        var client = CreateClient(handler);

        await client.GetPaycodesAsync(new PaycodeSearchFilter
        {
            TransactionReference = "MFY-ABC",
            BeneficiaryName = "John",
            TransactionStatus = "PENDING",
            From = 1676541600,
            To = 1676628000,
        });

        var query = handler.Requests[0].RequestUri!.Query;
        Assert.Contains("transactionReference=MFY-ABC", query);
        Assert.Contains("beneficiaryName=John", query);
        Assert.Contains("transactionStatus=PENDING", query);
        Assert.Contains("from=1676541600", query);
        Assert.Contains("to=1676628000", query);
    }

    [Fact]
    public async Task GetPaycodesAsync_WithPartialFilter_OmitsNullParams()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, $$"""
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [], "last": true, "totalElements": 0, "totalPages": 0,
                "first": true, "numberOfElements": 0, "size": 10, "number": 0, "empty": true
              } }
            """));
        var client = CreateClient(handler);

        await client.GetPaycodesAsync(new PaycodeSearchFilter { TransactionStatus = "PENDING" });

        var query = handler.Requests[0].RequestUri!.Query;
        Assert.Contains("transactionStatus=PENDING", query);
        Assert.DoesNotContain("transactionReference", query);
        Assert.DoesNotContain("beneficiaryName", query);
        Assert.DoesNotContain("from=", query);
        Assert.DoesNotContain("to=", query);
    }

    // ── GetPaycodeAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task GetPaycodeAsync_SendsGetToCorrectPath()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, EnvelopeOf(PaycodeJson)));
        var client = CreateClient(handler);

        var result = await client.GetPaycodeAsync("ghehdekdkefkefjekjfejj");

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/paycode/ghehdekdkefkefjekjfejj", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("ghehdekdkefkefjekjfejj", result.PaycodeReference);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetPaycodeAsync_MissingReference_Throws(string? reference)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetPaycodeAsync(reference!));
    }

    // ── CancelPaycodeAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task CancelPaycodeAsync_SendsDeleteToCorrectPath_AndReturnsCancelledPaycode()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, EnvelopeOf(PaycodeJson)));
        var client = CreateClient(handler);

        var result = await client.CancelPaycodeAsync("ghehdekdkefkefjekjfejj");

        Assert.Equal(HttpMethod.Delete, handler.Requests[0].Method);
        Assert.Equal("/api/v1/paycode/ghehdekdkefkefjekjfejj", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("11467409", result.PaycodeValue);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CancelPaycodeAsync_MissingReference_Throws(string? reference)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.CancelPaycodeAsync(reference!));
    }

    // ── GetUnmaskedPaycodeAsync ───────────────────────────────────────────────

    [Fact]
    public async Task GetUnmaskedPaycodeAsync_SendsGetToAuthorizePath()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, EnvelopeOf(PaycodeJson)));
        var client = CreateClient(handler);

        var result = await client.GetUnmaskedPaycodeAsync("ghehdekdkefkefjekjfejj");

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/paycode/ghehdekdkefkefjekjfejj/authorize", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("11467409", result.PaycodeValue);
        Assert.Equal(100m, result.Fee);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetUnmaskedPaycodeAsync_MissingReference_Throws(string? reference)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetUnmaskedPaycodeAsync(reference!));
    }
}
