using System.Net;
using Monnify.Collections;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Collections.Invoices;

public class MonnifyCollectionsClientInvoicesTests
{
    private static MonnifyCollectionsClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    private const string SampleInvoice = """
        { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
          "responseBody": {
            "amount": 999,
            "invoiceReference": "inv-ref-1",
            "invoiceStatus": "PENDING",
            "description": "test invoice",
            "contractCode": "1234567890",
            "customerEmail": "johnsnow@example.com",
            "customerName": "John Snow",
            "expiryDate": "2026-07-15 12:00:00",
            "createdBy": "MK_TEST_XXXXXXXXXX",
            "createdOn": "2026-06-26 23:26:41",
            "checkoutUrl": "https://sandbox.sdk.monnify.com/checkout/MNFY|17|20260626232640|000313",
            "accountNumber": "1003440679",
            "accountName": "test invoice",
            "bankName": "Wema bank",
            "bankCode": "035",
            "redirectUrl": "http://app.monnify.com",
            "transactionReference": "MNFY|17|20260626232640|000313"
          } }
        """;

    [Fact]
    public async Task CreateInvoiceAsync_SendsPostWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, SampleInvoice));
        var client = CreateClient(handler);

        var result = await client.CreateInvoiceAsync(new CreateInvoiceRequest
        {
            Amount = 999,
            InvoiceReference = "inv-ref-1",
            CustomerName = "John Snow",
            CustomerEmail = "johnsnow@example.com",
            ContractCode = "1234567890",
            Description = "test invoice",
            ExpiryDate = "2026-07-15 12:00:00",
            RedirectUrl = "http://app.monnify.com",
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/invoice/create", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("PENDING", result.InvoiceStatus);
        Assert.Equal("1003440679", result.AccountNumber);
    }

    [Fact]
    public async Task CreateInvoiceAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.CreateInvoiceAsync(null!));
    }

    [Fact]
    public async Task GetInvoiceAsync_SendsGetToDetailsPath()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, SampleInvoice));
        var client = CreateClient(handler);

        var result = await client.GetInvoiceAsync("inv-ref-1");

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/invoice/inv-ref-1/details", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("inv-ref-1", result.InvoiceReference);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetInvoiceAsync_MissingReference_Throws(string? reference)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetInvoiceAsync(reference!));
    }

    [Fact]
    public async Task GetInvoicesAsync_SendsGetToAllEndpoint_WithPageAndSizeQueryParams()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": { "content": [], "totalElements": 0, "totalPages": 0, "number": 1, "size": 3, "first": false, "last": true } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetInvoicesAsync(page: 1, size: 3);

        Assert.Equal("/api/v1/invoice/all", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("page=1&size=3", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(3, result.PageSize);
    }

    [Fact]
    public async Task CancelInvoiceAsync_SendsDeleteToCancelPath()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, SampleInvoice.Replace("PENDING", "CANCELLED")));
        var client = CreateClient(handler);

        var result = await client.CancelInvoiceAsync("inv-ref-1");

        Assert.Equal(HttpMethod.Delete, handler.Requests[0].Method);
        Assert.Equal("/api/v1/invoice/inv-ref-1/cancel", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("CANCELLED", result.InvoiceStatus);
    }
}
