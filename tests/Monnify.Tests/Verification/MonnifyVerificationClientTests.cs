using System.Net;
using Monnify.Tests.TestUtilities;
using Monnify.Verification;

namespace Monnify.Tests.Verification;

public class MonnifyVerificationClientTests
{
    private static MonnifyVerificationClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    [Fact]
    public async Task ValidateAccountNumberAsync_SendsGetWithQueryParams_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": { "accountNumber": "0123456789", "accountName": "Ada Lovelace", "bankCode": "044", "currencyCode": "NGN" } }
            """));
        var client = CreateClient(handler);

        var result = await client.ValidateAccountNumberAsync("0123456789", "044");

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/disbursements/account/validate", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("accountNumber=0123456789&bankCode=044", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Equal("Ada Lovelace", result.AccountName);
        Assert.Equal("NGN", result.CurrencyCode);
    }

    [Theory]
    [InlineData(null, "044")]
    [InlineData("", "044")]
    [InlineData("0123456789", null)]
    [InlineData("0123456789", "")]
    public async Task ValidateAccountNumberAsync_MissingArguments_ThrowsArgumentException(string? accountNumber, string? bankCode)
    {
        var client = CreateClient(new FakeHttpMessageHandler());

        await Assert.ThrowsAsync<ArgumentException>(() => client.ValidateAccountNumberAsync(accountNumber!, bankCode!));
    }

    [Fact]
    public async Task ValidateAccountNumberAsync_EscapesQueryValues()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": { "accountNumber": "0123456789", "accountName": "Ada", "bankCode": "0 44", "currencyCode": "NGN" } }
            """));
        var client = CreateClient(handler);

        await client.ValidateAccountNumberAsync("0123456789", "0 44");

        Assert.Contains("bankCode=0%2044", handler.Requests[0].RequestUri!.Query);
    }

    [Fact]
    public async Task MatchBvnDetailsAsync_SendsPostWithJsonBody_AndDeserializesMixedShapeResponse()
    {
        var handler = new FakeHttpMessageHandler();
        // Sample payload from our official API reference (POST /api/v1/vas/bvn-details-match).
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "bvn": "22228945899",
                "name": { "matchStatus": "FULL_MATCH", "matchPercentage": 100 },
                "dateOfBirth": "NO_MATCH",
                "mobileNo": "FULL_MATCH"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.MatchBvnDetailsAsync(new BvnDetailsMatchRequest
        {
            Bvn = "22222222226",
            Name = "OLATUNDE JOSIAH OGUNBOYEJO",
            DateOfBirth = "27-Apr-1993",
            MobileNo = "08142223149",
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/vas/bvn-details-match", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"bvn\":\"22222222226\"", handler.RequestBodies[0]);
        Assert.Equal("FULL_MATCH", result.Name!.MatchStatus);
        Assert.Equal(100, result.Name.MatchPercentage);
        Assert.Equal("NO_MATCH", result.DateOfBirth);
        Assert.Equal("FULL_MATCH", result.MobileNo);
    }

    [Fact]
    public async Task MatchBvnDetailsAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.MatchBvnDetailsAsync(null!));
    }

    [Fact]
    public async Task MatchBvnToAccountAsync_SendsPostWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        // Sample payload from our official API reference (POST /api/v1/vas/bvn-account-match).
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "bvn": "22222222226",
                "accountNumber": "0103284175",
                "accountName": "OLATUNDE JOSIAH OGUNBOYEJO",
                "matchStatus": "FULL_MATCH",
                "matchPercentage": 100
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.MatchBvnToAccountAsync(new BvnAccountMatchRequest
        {
            BankCode = "057",
            AccountNumber = "2191802645",
            Bvn = "22222222226",
        });

        Assert.Equal("/api/v1/vas/bvn-account-match", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("OLATUNDE JOSIAH OGUNBOYEJO", result.AccountName);
        Assert.Equal("FULL_MATCH", result.MatchStatus);
        Assert.Equal(100, result.MatchPercentage);
    }

    [Fact]
    public async Task MatchBvnToAccountAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.MatchBvnToAccountAsync(null!));
    }

    [Fact]
    public async Task VerifyNinAsync_SendsPostWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        // Sample payload from our official API reference (POST /api/v1/vas/nin-details).
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "nin": "91919191913",
                "lastName": "WILES",
                "firstName": "BENJAMIN",
                "middleName": "CHUKS",
                "dateOfBirth": "1996-10-08",
                "gender": "OTHER",
                "mobileNumber": "2348107248890"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.VerifyNinAsync("94646622685");

        Assert.Equal("/api/v1/vas/nin-details", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"nin\":\"94646622685\"", handler.RequestBodies[0]);
        Assert.Equal("BENJAMIN", result.FirstName);
        Assert.Equal("WILES", result.LastName);
        Assert.Equal("CHUKS", result.MiddleName);
        Assert.Equal("2348107248890", result.MobileNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task VerifyNinAsync_MissingNin_ThrowsArgumentException(string? nin)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.VerifyNinAsync(nin!));
    }
}
