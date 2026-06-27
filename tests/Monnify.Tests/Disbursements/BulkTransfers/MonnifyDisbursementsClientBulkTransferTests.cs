using System.Net;
using Monnify.Disbursements;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Disbursements.BulkTransfers;

public class MonnifyDisbursementsClientBulkTransferTests
{
    private static MonnifyDisbursementsClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    [Fact]
    public async Task InitiateBulkTransferAsync_SendsPostWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        // Sample payload from a real sandbox call.
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "totalAmount": 50.00, "totalFee": 10.00, "batchReference": "disb-batch-test-1782549335",
                "transactionBatchReference": "MFDB87620260627093538000001ZK0LHX", "batchStatus": "AWAITING_PROCESSING",
                "totalTransactionsCount": 1, "dateCreated": "2026-06-27T08:35:38.887+00:00"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.InitiateBulkTransferAsync(new BulkTransferRequest
        {
            Title = "SDK batch test",
            BatchReference = "disb-batch-test-1782549335",
            Narration = "SDK batch test",
            SourceAccountNumber = "9988776655",
            OnValidationFailure = "CONTINUE",
            NotificationInterval = 25,
            TransactionList = new[]
            {
                new BulkTransferItem
                {
                    Amount = 50,
                    Reference = "disb-batch-item-1",
                    Narration = "SDK batch item",
                    DestinationBankCode = "058",
                    DestinationAccountNumber = "0123456789",
                    DestinationAccountName = "Test Recipient",
                },
            },
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v2/disbursements/batch", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("AWAITING_PROCESSING", result.BatchStatus);
        Assert.Equal("MFDB87620260627093538000001ZK0LHX", result.TransactionBatchReference);
    }

    [Fact]
    public async Task InitiateBulkTransferAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.InitiateBulkTransferAsync(null!));
    }

    [Fact]
    public async Task AuthorizeBulkTransferAsync_SendsPostWithReferenceAndCode()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "totalAmount": 2100, "totalFee": 105, "batchReference": "batchreference--12934",
                "batchStatus": "AWAITING_PROCESSING", "totalTransactionsCount": 3, "dateCreated": "2022-07-31T14:41:19.588+0000"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.AuthorizeBulkTransferAsync("batchreference--12934", "491763");

        Assert.Equal("/api/v2/disbursements/batch/validate-otp", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"reference\":\"batchreference--12934\"", handler.RequestBodies[0]);
        Assert.Equal(3, result.TotalTransactionsCount);
    }

    [Fact]
    public async Task ResendBulkTransferOtpAsync_SendsPostWithBatchReference()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": { "message": "Authorization code will be processed and sent to predefined email addresses(s)" } }
            """));
        var client = CreateClient(handler);

        await client.ResendBulkTransferOtpAsync("batch-reference-1");

        Assert.Equal("/api/v2/disbursements/batch/resend-otp", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"batchReference\":\"batch-reference-1\"", handler.RequestBodies[0]);
    }

    [Fact]
    public async Task GetBulkTransferSummaryAsync_SendsGetWithReferenceQueryParam()
    {
        var handler = new FakeHttpMessageHandler();
        // Sample payload from a real sandbox call.
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "title": "SDK batch test", "totalAmount": 50.00, "totalFee": 10.00, "batchReference": "disb-batch-test-1782549335",
                "totalTransactionsCount": 1, "failedCount": 1, "successfulCount": 0, "pendingCount": 0,
                "pendingAmount": 0.00, "failedAmount": 50.00, "successfulAmount": 0.00,
                "batchStatus": "COMPLETED", "dateCreated": "2026-06-27T08:35:39.000+00:00"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetBulkTransferSummaryAsync("disb-batch-test-1782549335");

        Assert.Equal("/api/v2/disbursements/batch/summary", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("reference=disb-batch-test-1782549335", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Equal("COMPLETED", result.BatchStatus);
        Assert.Equal(1, result.FailedCount);
    }

    [Fact]
    public async Task GetBulkTransferTransactionsAsync_SendsGetWithBatchReferenceInPath()
    {
        var handler = new FakeHttpMessageHandler();
        // Sample payload from a real sandbox call.
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [
                  { "amount": 50.00, "reference": "disb-batch-item-1782549335", "narration": "SDK batch item", "currency": "NGN",
                    "fee": 10.00, "twoFaEnabled": false, "status": "FAILED",
                    "transactionDescription": "Account number could not be validated",
                    "transactionReference": "MFDS87620260627093539000028T9APDA", "createdOn": "2026-06-27T08:35:40.000+00:00",
                    "sourceAccountNumber": "9988776655", "destinationAccountNumber": "0123456789", "destinationBankCode": "058" }
                ],
                "totalElements": 1, "totalPages": 1, "number": 0, "size": 10, "first": true, "last": true } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetBulkTransferTransactionsAsync("disb-batch-test-1782549335", pageNo: 0, pageSize: 10);

        Assert.Equal("/api/v2/disbursements/bulk/disb-batch-test-1782549335/transactions", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("pageSize=10&pageNo=0", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Single(result.Content);
        Assert.Equal("FAILED", result.Content[0].Status);
        Assert.Null(result.Content[0].DestinationAccountName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetBulkTransferTransactionsAsync_MissingBatchReference_Throws(string? batchReference)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetBulkTransferTransactionsAsync(batchReference!));
    }

    [Fact]
    public async Task GetBulkTransfersAsync_SendsGetToBulkBase_WithPagingAndOptionalSourceAccount()
    {
        var handler = new FakeHttpMessageHandler();
        // Sample payload from a real sandbox call.
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [
                  { "totalAmount": 50.00, "totalFee": 10.00, "batchReference": "it-disb-batch-1782553699",
                    "transactionBatchReference": "MFDB98020260627104824000001JXBXV6", "batchStatus": "COMPLETED",
                    "totalTransactionsCount": 1, "dateCreated": "2026-06-27T09:48:25.000+00:00" }
                ],
                "totalElements": 74, "totalPages": 25, "number": 0, "size": 3, "first": true, "last": false } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetBulkTransfersAsync("9988776655", pageNo: 0, pageSize: 3);

        Assert.Equal("/api/v2/disbursements/bulk", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("pageNo=0&pageSize=3&sourceAccountNumber=9988776655", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Single(result.Content);
        Assert.Equal("COMPLETED", result.Content[0].BatchStatus);
    }

    [Fact]
    public async Task GetBulkTransfersAsync_NoSourceAccount_OmitsItFromQuery()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": { "content": [], "totalElements": 0, "totalPages": 0, "number": 0, "size": 20, "first": true, "last": true } }
            """));
        var client = CreateClient(handler);

        await client.GetBulkTransfersAsync();

        Assert.Equal("pageNo=0&pageSize=20", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
    }
}
