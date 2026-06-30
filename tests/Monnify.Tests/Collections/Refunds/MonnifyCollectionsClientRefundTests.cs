using System.Net;
using Monnify.Collections;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Collections.Refunds;

public class MonnifyCollectionsClientRefundTests
{
    private static MonnifyCollectionsClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    private const string RefundJson = """
        {
          "refundReference": "202100op3456",
          "transactionReference": "MNFY|65|20220727094724|000477",
          "refundReason": "Order cancelled!",
          "customerNote": "An optional note",
          "refundAmount": 100,
          "refundType": "PARTIAL_REFUND",
          "refundStatus": "COMPLETED",
          "refundStrategy": "MERCHANT_WALLET",
          "comment": "Transaction refund is in progress.",
          "createdOn": "02/08/2022 03:35:22 AM"
        }
        """;

    [Fact]
    public async Task InitiateRefundAsync_SendsPostWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK,
            $$"""{ "requestSuccessful": true, "responseMessage": "success", "responseCode": "0", "responseBody": {{RefundJson}} }"""));
        var client = CreateClient(handler);

        var result = await client.InitiateRefundAsync(new InitiateRefundRequest
        {
            TransactionReference = "MNFY|65|20220727094724|000477",
            RefundAmount = 100m,
            RefundReference = "202100op3456",
            RefundReason = "Order cancelled!",
            CustomerNote = "An optional note",
            DestinationAccountNumber = "3270005594",
            DestinationAccountBankCode = "050",
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/refunds/initiate-refund", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"refundReference\":\"202100op3456\"", handler.RequestBodies[0]);
        Assert.Contains("\"destinationAccountNumber\":\"3270005594\"", handler.RequestBodies[0]);
        Assert.Equal("202100op3456", result.RefundReference);
        Assert.Equal("PARTIAL_REFUND", result.RefundType);
        Assert.Equal("COMPLETED", result.RefundStatus);
        Assert.Equal("MERCHANT_WALLET", result.RefundStrategy);
    }

    [Fact]
    public async Task InitiateRefundAsync_WithoutOptionalDestination_OmitsFieldsFromBody()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK,
            $$"""{ "requestSuccessful": true, "responseMessage": "success", "responseCode": "0", "responseBody": {{RefundJson}} }"""));
        var client = CreateClient(handler);

        await client.InitiateRefundAsync(new InitiateRefundRequest
        {
            TransactionReference = "MNFY|65|20220727094724|000477",
            RefundAmount = 100m,
            RefundReference = "202100op3456",
            RefundReason = "Order cancelled!",
            CustomerNote = "An optional note",
        });

        Assert.DoesNotContain("destinationAccountNumber", handler.RequestBodies[0]);
        Assert.DoesNotContain("destinationAccountBankCode", handler.RequestBodies[0]);
    }

    [Fact]
    public async Task InitiateRefundAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.InitiateRefundAsync(null!));
    }

    [Fact]
    public async Task GetRefundAsync_SendsGetWithReferenceInPath_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK,
            $$"""{ "requestSuccessful": true, "responseMessage": "success", "responseCode": "0", "responseBody": {{RefundJson}} }"""));
        var client = CreateClient(handler);

        var result = await client.GetRefundAsync("202100op3456");

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/refunds/202100op3456", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("202100op3456", result.RefundReference);
        Assert.Equal(100m, result.RefundAmount);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetRefundAsync_MissingReference_Throws(string? refundReference)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetRefundAsync(refundReference!));
    }

    [Fact]
    public async Task GetRefundsAsync_SendsGetWithPagingParams_AndDeserializesPagedResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, $$"""
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [ {{RefundJson}} ],
                "pageable": null, "last": false, "totalElements": 17, "totalPages": 2,
                "sort": null, "first": true, "numberOfElements": 10, "size": 10, "number": 0, "empty": false
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetRefundsAsync(page: 0, size: 10);

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/refunds", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("page=0&size=10", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Single(result.Content);
        Assert.Equal(17, result.TotalElements);
        Assert.Equal(2, result.TotalPages);
        Assert.Equal("COMPLETED", result.Content[0].RefundStatus);
    }
}
