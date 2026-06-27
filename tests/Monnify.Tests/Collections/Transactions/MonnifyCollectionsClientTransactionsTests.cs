using System.Net;
using Monnify.Collections;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Collections.Transactions;

public class MonnifyCollectionsClientTransactionsTests
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

    [Fact]
    public async Task InitiateBankTransferAsync_SendsPostWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        // Sample payload from a real sandbox call.
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "accountNumber": "2209149705", "accountName": "Test01-Test payment", "bankName": "Sterling bank",
                "bankCode": "232", "accountDurationSeconds": 2400, "ussdPayment": null,
                "requestTime": "2026-06-27T09:11:14.596774984", "expiresOn": "2026-06-27T09:51:14",
                "transactionReference": "MNFY|17|20260627090811|000340", "paymentReference": "endpoints-test-1782547690",
                "amount": 100.00, "fee": 0.00, "totalPayable": 100.00, "collectionChannel": "API_NOTIFICATION"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.InitiateBankTransferAsync(new InitiateBankTransferRequest
        {
            TransactionReference = "MNFY|17|20260627090811|000340",
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/merchant/bank-transfer/init-payment", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("2209149705", result.AccountNumber);
        Assert.Equal("Sterling bank", result.BankName);
        Assert.Equal(100.00m, result.TotalPayable);
    }

    [Fact]
    public async Task InitiateBankTransferAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.InitiateBankTransferAsync(null!));
    }

    [Fact]
    public async Task SearchTransactionsAsync_SendsGetWithPageAndFilters()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [
                  { "customerDTO": { "email": "ada@example.com", "name": "Ada Lovelace" },
                    "paymentMethod": "ACCOUNT_TRANSFER", "createdOn": "2026-06-27T08:08:11.000+00:00",
                    "amount": 100.00, "currencyCode": "NGN", "paymentDescription": "Test payment",
                    "paymentStatus": "PENDING", "transactionReference": "MNFY|17|20260627090811|000340",
                    "paymentReference": "endpoints-test-1782547690", "merchantName": "Test01", "completed": false,
                    "paymentMethodList": ["ACCOUNT_TRANSFER", "CARD"], "collectionChannel": "API_NOTIFICATION" }
                ],
                "totalElements": 1, "totalPages": 1, "number": 0, "size": 5, "first": true, "last": true } }
            """));
        var client = CreateClient(handler);

        var result = await client.SearchTransactionsAsync(
            new SearchTransactionsRequest { CustomerEmail = "ada@example.com", PaymentStatus = "PENDING" },
            page: 0, size: 5);

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/transactions/search", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("page=0&size=5&customerEmail=ada%40example.com&paymentStatus=PENDING", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Single(result.Content);
        Assert.Equal("Ada Lovelace", result.Content[0].Customer!.Name);
    }

    [Fact]
    public async Task GetTransactionAsync_SendsGetToV2Endpoint_WithReferenceInPath()
    {
        var handler = new FakeHttpMessageHandler();
        // Sample payload from a real sandbox call.
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "transactionReference": "MNFY|17|20260627090811|000340", "paymentReference": "endpoints-test-1782547690",
                "amountPaid": "0.00", "totalPayable": "0.00", "settlementAmount": null, "paidOn": null,
                "paymentStatus": "PENDING", "paymentDescription": "Test payment", "currency": "NGN",
                "paymentMethod": "ACCOUNT_TRANSFER",
                "product": { "type": "API_NOTIFICATION", "reference": "endpoints-test-1782547690" },
                "cardDetails": null, "customer": { "email": "ada@example.com", "name": "Ada Lovelace" } } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetTransactionAsync("MNFY|17|20260627090811|000340");

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v2/transactions/MNFY|17|20260627090811|000340", Uri.UnescapeDataString(handler.Requests[0].RequestUri!.AbsolutePath));
        Assert.Equal("PENDING", result.PaymentStatus);
        Assert.Equal(0.00m, result.AmountPaid);
        Assert.Equal("Ada Lovelace", result.Customer!.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetTransactionAsync_MissingReference_Throws(string? reference)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetTransactionAsync(reference!));
    }

    [Fact]
    public async Task QueryTransactionAsync_SendsGetWithQueryParams()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "transactionReference": "MNFY|17|20260627090811|000340", "paymentReference": "endpoints-test-1782547690",
                "amountPaid": "0.00", "totalPayable": "0.00", "paymentStatus": "PENDING", "currency": "NGN",
                "paymentMethod": "ACCOUNT_TRANSFER" } }
            """));
        var client = CreateClient(handler);

        var result = await client.QueryTransactionAsync(paymentReference: "endpoints-test-1782547690");

        Assert.Equal("/api/v2/merchant/transactions/query", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("paymentReference=endpoints-test-1782547690", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Equal("PENDING", result.PaymentStatus);
    }

    [Fact]
    public async Task QueryTransactionAsync_NoReferencesProvided_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.QueryTransactionAsync());
    }
}
