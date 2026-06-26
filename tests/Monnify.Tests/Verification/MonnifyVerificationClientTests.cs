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
}
