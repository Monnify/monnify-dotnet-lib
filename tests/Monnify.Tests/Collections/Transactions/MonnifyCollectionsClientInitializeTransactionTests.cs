using System.Net;
using Monnify.Collections;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Collections.Transactions;

public class MonnifyCollectionsClientInitializeTransactionTests
{
    private static MonnifyCollectionsClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    [Fact]
    public async Task InitializeTransactionAsync_SendsPostWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        // Sample payload from a real sandbox call.
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "transactionReference": "MNFY|17|20260626231846|000311",
                "paymentReference": "test-ref-001",
                "merchantName": "Test01",
                "apiKey": "MK_TEST_XXXXXXXXXX",
                "redirectUrl": "https://example.com/callback",
                "enabledPaymentMethod": ["ACCOUNT_TRANSFER", "CARD"],
                "checkoutUrl": "https://sandbox.sdk.monnify.com/checkout/MNFY|17|20260626231846|000311"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.InitializeTransactionAsync(new InitializeTransactionRequest
        {
            Amount = 100,
            CustomerName = "Ada Lovelace",
            CustomerEmail = "ada@example.com",
            PaymentReference = "test-ref-001",
            PaymentDescription = "Test payment",
            ContractCode = "1234567890",
            RedirectUrl = "https://example.com/callback",
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/merchant/transactions/init-transaction", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"contractCode\":\"1234567890\"", handler.RequestBodies[0]);
        Assert.Equal("MNFY|17|20260626231846|000311", result.TransactionReference);
        Assert.Equal(2, result.EnabledPaymentMethod.Count);
        Assert.Equal("https://sandbox.sdk.monnify.com/checkout/MNFY|17|20260626231846|000311", result.CheckoutUrl);
    }

    [Fact]
    public async Task InitializeTransactionAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.InitializeTransactionAsync(null!));
    }
}
