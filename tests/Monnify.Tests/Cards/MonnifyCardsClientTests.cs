using System.Net;
using Monnify.Cards;
using Monnify.Exceptions;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Cards;

// Payloads below are from our own documented samples for these endpoints; see
// docs/COMPATIBILITY.md for sandbox-verification status once that's done.
public class MonnifyCardsClientTests
{
    private static MonnifyCardsClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    private static ChargeCardRequest CreateChargeRequest(string cardNumber = "4111111111111111") => new()
    {
        TransactionReference = "MNFY|99|20220725125351|000271",
        Card = new Card
        {
            Number = cardNumber,
            ExpiryMonth = "10",
            ExpiryYear = "2025",
            Pin = "1234",
            Cvv = "123",
        },
        DeviceInformation = new DeviceInformation
        {
            HttpBrowserLanguage = "en-US",
            HttpBrowserJavaEnabled = false,
            HttpBrowserJavaScriptEnabled = true,
            HttpBrowserColorDepth = 24,
            HttpBrowserScreenHeight = 1203,
            HttpBrowserScreenWidth = 2138,
            HttpBrowserTimeDifference = "",
            UserAgentBrowserValue = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)",
        },
    };

    [Fact]
    public async Task ChargeAsync_SendsPostWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "status": "SUCCESS", "message": "Transaction Successful",
                "transactionReference": "MNFY|99|20220725110839|000256",
                "paymentReference": "1234567890-abcdef", "authorizedAmount": 100
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.ChargeAsync(CreateChargeRequest());

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/merchant/cards/charge", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"number\":\"4111111111111111\"", handler.RequestBodies[0]);
        Assert.Contains("\"collectionChannel\":\"API_NOTIFICATION\"", handler.RequestBodies[0]);
        Assert.Equal("SUCCESS", result.Status);
        Assert.Equal(100, result.AuthorizedAmount);
        Assert.Equal("1234567890-abcdef", result.PaymentReference);
    }

    [Fact]
    public async Task ChargeAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.ChargeAsync(null!));
    }

    [Fact]
    public async Task ChargeAsync_FailedCard_ThrowsMonnifyApiException()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.BadRequest, """
            { "requestSuccessful": false, "responseMessage": "Invalid card number", "responseCode": "99" }
            """));
        var client = CreateClient(handler);

        var ex = await Assert.ThrowsAsync<MonnifyApiException>(
            () => client.ChargeAsync(CreateChargeRequest("4111111111111110")));

        Assert.Equal("Invalid card number", ex.ResponseMessage);
    }

    [Fact]
    public async Task AuthorizeOtpAsync_SendsPostWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "paymentStatus": "SUCCESSFUL", "paymentDescription": "Payment Successful",
                "transactionReference": "MNFY|67|20220725114827|000285",
                "paymentReference": "1568577644707", "amountPaid": 100, "currencyPaid": "NGN"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.AuthorizeOtpAsync(new AuthorizeCardOtpRequest
        {
            TransactionReference = "MNFY|67|20220725114827|000285",
            TokenId = "100.00-b66bef0aa8e660863c4e1177a08fefba",
            Token = "123456",
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/merchant/cards/otp/authorize", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"token\":\"123456\"", handler.RequestBodies[0]);
        Assert.Equal("SUCCESSFUL", result.PaymentStatus);
        Assert.Equal(100, result.AmountPaid);
        Assert.Equal("NGN", result.CurrencyPaid);
    }

    [Fact]
    public async Task AuthorizeOtpAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.AuthorizeOtpAsync(null!));
    }

    [Fact]
    public async Task Authorize3dsAsync_SendsPostWithApiKeyInBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "paymentStatus": "SUCCESSFUL", "paymentDescription": "Payment Successful",
                "transactionReference": "MNFY|99|20220725125351|000271",
                "paymentReference": "1568577644707", "amountPaid": 100, "currencyPaid": "NGN"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.Authorize3dsAsync(new Authorize3dsCardRequest
        {
            TransactionReference = "MNFY|99|20220725125351|000271",
            ApiKey = "MK_TEST_PLACEHOLDER123",
            Card = new Card
            {
                Number = "4000000000000002",
                ExpiryMonth = "12",
                ExpiryYear = "2025",
                Cvv = "123",
                Pin = "1234",
            },
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/sdk/cards/secure-3d/authorize", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"apiKey\":\"MK_TEST_PLACEHOLDER123\"", handler.RequestBodies[0]);
        Assert.Equal("SUCCESSFUL", result.PaymentStatus);
    }

    [Fact]
    public async Task Authorize3dsAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.Authorize3dsAsync(null!));
    }
}
