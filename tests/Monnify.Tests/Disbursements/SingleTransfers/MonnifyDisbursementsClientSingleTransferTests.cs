using System.Net;
using Monnify.Disbursements;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Disbursements.SingleTransfers;

public class MonnifyDisbursementsClientSingleTransferTests
{
    private static MonnifyDisbursementsClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    [Fact]
    public async Task InitiateSingleTransferAsync_SendsPostWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        // Sample payload from a real sandbox call.
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "amount": 100, "reference": "disb-test-1782549167", "status": "SUCCESS",
                "dateCreated": "2026-06-27T08:32:49.289+00:00", "totalFee": 10.00,
                "destinationAccountName": "Test Recipient", "destinationBankName": "GTBank",
                "destinationAccountNumber": "0123456789", "destinationBankCode": "058",
                "comment": "Transaction successful"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.InitiateSingleTransferAsync(new SingleTransferRequest
        {
            Amount = 100,
            Reference = "disb-test-1782549167",
            Narration = "SDK test disbursement",
            DestinationBankCode = "058",
            DestinationAccountNumber = "0123456789",
            DestinationAccountName = "Test Recipient",
            SourceAccountNumber = "9988776655",
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v2/disbursements/single", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("SUCCESS", result.Status);
        Assert.Equal(10.00m, result.TotalFee);
        Assert.Equal("GTBank", result.DestinationBankName);
    }

    [Fact]
    public async Task InitiateSingleTransferAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.InitiateSingleTransferAsync(null!));
    }

    [Fact]
    public async Task AuthorizeSingleTransferAsync_SendsPostWithReferenceAndCode_AndDeserializesSenderInfo()
    {
        var handler = new FakeHttpMessageHandler();
        // Sample payload from Monnify's official API reference.
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "amount": 200, "reference": "referen00ce---1290034", "status": "SUCCESS",
                "dateCreated": "2022-07-31T14:31:33.759+0000", "totalFee": 35,
                "destinationAccountName": "Ciroma Chukwuka Adekunle", "destinationBankName": "Moniepoint Microfinance bank",
                "destinationAccountNumber": "2085886393", "destinationBankCode": "50515",
                "senderInfo": { "sourceAccountNumber": "9988776655", "sourceAccountName": "Marvelous Benji", "senderBankCode": "50515" }
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.AuthorizeSingleTransferAsync("referen00ce---1290034", "491763");

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v2/disbursements/single/validate-otp", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"reference\":\"referen00ce---1290034\"", handler.RequestBodies[0]);
        Assert.Contains("\"authorizationCode\":\"491763\"", handler.RequestBodies[0]);
        Assert.Equal("SUCCESS", result.Status);
        Assert.Equal("Marvelous Benji", result.SenderInfo!.SourceAccountName);
    }

    [Theory]
    [InlineData(null, "code")]
    [InlineData("", "code")]
    [InlineData("ref", null)]
    [InlineData("ref", "")]
    public async Task AuthorizeSingleTransferAsync_MissingArgument_Throws(string? reference, string? code)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.AuthorizeSingleTransferAsync(reference!, code!));
    }

    [Fact]
    public async Task ResendSingleTransferOtpAsync_SendsPostWithReference()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": { "message": "Authorization code will be processed and sent to predefined email addresses(s)" } }
            """));
        var client = CreateClient(handler);

        var result = await client.ResendSingleTransferOtpAsync("referen00ce---1290034");

        Assert.Equal("/api/v2/disbursements/single/resend-otp", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"reference\":\"referen00ce---1290034\"", handler.RequestBodies[0]);
        Assert.Contains("predefined email", result.Message);
    }

    [Fact]
    public async Task GetSingleTransferAsync_SendsGetWithReferenceQueryParam()
    {
        var handler = new FakeHttpMessageHandler();
        // Sample payload from a real sandbox call.
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "amount": 100.00, "reference": "disb-test-1782549167", "narration": "SDK test disbursement",
                "currency": "NGN", "fee": 10.00, "twoFaEnabled": false, "status": "SUCCESS",
                "transactionDescription": "Transaction successful", "transactionReference": "MFDS87620260627093249000027JDW8UZ",
                "createdOn": "2026-06-27T08:32:49.000+00:00", "sourceAccountNumber": "9988776655",
                "destinationAccountNumber": "0123456789", "destinationAccountName": "Test Recipient",
                "destinationBankCode": "058", "destinationBankName": "GTBank"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetSingleTransferAsync("disb-test-1782549167");

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v2/disbursements/single/summary", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("reference=disb-test-1782549167", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Equal("SUCCESS", result.Status);
        Assert.False(result.TwoFaEnabled);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetSingleTransferAsync_MissingReference_Throws(string? reference)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetSingleTransferAsync(reference!));
    }

    [Fact]
    public async Task GetSingleTransfersAsync_SendsGetWithPagingQueryParams()
    {
        var handler = new FakeHttpMessageHandler();
        // Sample payload from a real sandbox call.
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [
                  { "amount": 20.00, "reference": "MFDS238...", "narration": "", "currency": "NGN", "fee": 10.00,
                    "twoFaEnabled": false, "status": "SUCCESS", "transactionDescription": "Transaction successful",
                    "transactionReference": "MFDS238...", "createdOn": "2026-06-23T13:49:21.000+00:00",
                    "sourceAccountNumber": "9988776655", "destinationAccountNumber": "5544332211",
                    "destinationAccountName": "ONYEWUCHI HUMPHREY GODWILL", "destinationBankCode": "305", "destinationBankName": "OPAY 2" }
                ],
                "totalElements": 1, "totalPages": 1, "number": 0, "size": 3, "first": true, "last": true } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetSingleTransfersAsync(pageNo: 0, pageSize: 3);

        Assert.Equal("/api/v2/disbursements/single/transactions", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("pageNo=0&pageSize=3", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Single(result.Content);
        Assert.Equal("OPAY 2", result.Content[0].DestinationBankName);
    }
}
