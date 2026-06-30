using System.Net;
using Monnify.Collections;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Collections.Mandates;

// Payloads below are from our own documented samples for these endpoints; see
// docs/COMPATIBILITY.md for sandbox-verification status.
public class MonnifyCollectionsClientMandatesTests
{
    private static MonnifyCollectionsClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    [Fact]
    public async Task CreateMandateAsync_SendsPostWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "responseMessage": "Your request for creating a mandate is submitted. An authorization instruction will be sent to the customer email.",
                "mandateReference": "unique_ref3_02s600972",
                "mandateCode": "MTDD|01HY8W3FBKHTFBZP9DQ9KNHR1G",
                "mandateStatus": "INITIATED",
                "redirectUrl": "https://my-merchants-page.com/transaction/confirm"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.CreateMandateAsync(new CreateMandateRequest
        {
            ContractCode = "4934121686",
            MandateReference = "unique_ref3_02s600972",
            MandateAmount = 50000,
            AutoRenew = false,
            CustomerCancellation = true,
            CustomerName = "Ankit Kushwaha",
            CustomerPhoneNumber = "1234567890",
            CustomerEmailAddress = "test@moniepoint.com",
            CustomerAddress = "123 Example Street, City, Country",
            CustomerAccountNumber = "0051762787",
            CustomerAccountBankCode = "044",
            MandateDescription = "Subscription Fee",
            MandateStartDate = "2024-12-19T10:15:30",
            MandateEndDate = "2025-12-19T10:15:30",
            RedirectUrl = "https://my-merchants-page.com/direct-debit/success",
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/direct-debit/mandate/create", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"mandateReference\":\"unique_ref3_02s600972\"", handler.RequestBodies[0]);
        Assert.Equal("INITIATED", result.MandateStatus);
        Assert.Equal("MTDD|01HY8W3FBKHTFBZP9DQ9KNHR1G", result.MandateCode);
        Assert.Equal("https://my-merchants-page.com/transaction/confirm", result.RedirectUrl);
    }

    [Fact]
    public async Task CreateMandateAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.CreateMandateAsync(null!));
    }

    [Fact]
    public async Task GetMandatesAsync_SendsGetWithReferenceQueryParam_AndDeserializesArray()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": [
                {
                  "mandateCode": "MTDD|01HY8WMN8JYKDRJC67QPQVS1N0",
                  "externalMandateReference": "unique_ref3_02gs600s972",
                  "startDate": "2024-05-19T09:15:30.000+0000",
                  "endDate": "2024-09-22T09:15:30.000+0000",
                  "mandateStatus": "ACTIVE",
                  "mandateAmount": 50000,
                  "contractCode": "4934121686",
                  "autoRenew": false,
                  "customerPhoneNumber": "1234567890",
                  "customerEmailAddress": "test@moniepoint.com",
                  "customerAddress": "123 Example Street, City, Country",
                  "customerName": "Ankit Kushwaha",
                  "customerAccountName": "Ankit Kushwaha",
                  "customerAccountNumber": "0051762787",
                  "customerAccountBankCode": "998",
                  "mandateDescription": "Subscription Fee",
                  "debitAmount": null,
                  "authorizationMessage": "Request Ankit Kushwaha to kindly proceed with a token payment.",
                  "authorizationLink": "https://paylink.monnify.com/mandate-auth/abc"
                }
              ] }
            """));
        var client = CreateClient(handler);

        var result = await client.GetMandatesAsync("unique_ref3_02gs600s972");

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/direct-debit/mandate/", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("mandateReferences=unique_ref3_02gs600s972", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Single(result);
        Assert.Equal("ACTIVE", result[0].MandateStatus);
        Assert.Null(result[0].DebitAmount);
        Assert.Equal("Ankit Kushwaha", result[0].CustomerAccountName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetMandatesAsync_MissingReference_Throws(string? mandateReferences)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetMandatesAsync(mandateReferences!));
    }

    [Fact]
    public async Task DebitMandateAsync_SendsPostWithJsonBody_AndDeserializesStringResponseMessage()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "transactionStatus": "PENDING",
                "responseMessage": "Debit mandate request accepted for processing",
                "transactionReference": "MNFY|20240519180055|000001",
                "paymentReference": "PR1234567991002",
                "debitAmount": 1000,
                "narration": "Payment for Services",
                "mandateCode": "MTDD|01HY8WMN8JYKDRJC67QPQVS1N0"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.DebitMandateAsync(new DebitMandateRequest
        {
            PaymentReference = "PR1234567991002",
            MandateCode = "MTDD|01HY8WMN8JYKDRJC67QPQVS1N0",
            DebitAmount = 1000,
            Narration = "Payment for Services",
            CustomerEmail = "ahsan.saleem@gmail.com",
            IncomeSplitConfig = new[]
            {
                new IncomeSplitConfig
                {
                    SubAccountCode = "MFY_SUB_319452883228",
                    FeePercentage = 10.5m,
                    SplitAmount = 20,
                    SplitPercentage = 20,
                    FeeBearer = true,
                },
            },
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/direct-debit/mandate/debit", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"subAccountCode\":\"MFY_SUB_319452883228\"", handler.RequestBodies[0]);
        Assert.Equal("PENDING", result.TransactionStatus);
        Assert.Equal("Debit mandate request accepted for processing", result.ResponseMessage);
    }

    [Fact]
    public async Task DebitMandateAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.DebitMandateAsync(null!));
    }

    [Fact]
    public async Task GetMandateDebitStatusAsync_ResponseMessageIsEmptyObject_DeserializesToNullInsteadOfThrowing()
    {
        var handler = new FakeHttpMessageHandler();
        // Per our own documented sample: responseMessage is an empty object here, not a string
        // like it is on DebitMandateAsync's response.
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "transactionStatus": "PAID",
                "responseMessage": {},
                "transactionReference": "MNFY|20240519180055|000001",
                "paymentReference": "PR1234567991002",
                "debitAmount": 1000,
                "narration": "Payment for Services",
                "mandateCode": "MTDD|01HY8WMN8JYKDRJC67QPQVS1N0"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetMandateDebitStatusAsync("PR1234567991002");

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/direct-debit/mandate/debit-status", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("paymentReference=PR1234567991002", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Equal("PAID", result.TransactionStatus);
        Assert.Null(result.ResponseMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetMandateDebitStatusAsync_MissingReference_Throws(string? paymentReference)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetMandateDebitStatusAsync(paymentReference!));
    }

    [Fact]
    public async Task CancelMandateAsync_SendsPatchWithMandateCodeInPath()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "responseMessage": "Mandate has been requested to be cancelled.",
                "mandateReference": "unique_ref3_02s600972",
                "mandateCode": "MTDD|01HY8W3FBKHTFBZP9DQ9KNHR1G",
                "mandateStatus": "ACTIVE"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.CancelMandateAsync("MTDD|01HY8W3FBKHTFBZP9DQ9KNHR1G");

        Assert.Equal("PATCH", handler.Requests[0].Method.Method);
        Assert.Equal(
            "/api/v1/direct-debit/mandate/cancel-mandate/MTDD%7C01HY8W3FBKHTFBZP9DQ9KNHR1G",
            handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("ACTIVE", result.MandateStatus);
        Assert.Null(result.RedirectUrl);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CancelMandateAsync_MissingMandateCode_Throws(string? mandateCode)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.CancelMandateAsync(mandateCode!));
    }

    [Fact]
    public async Task ListMandatesAsync_SendsGetWithRequiredDateRangeAndOptionalFilters_AndDeserializesPagedResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [
                  { "mandateCode": "MTDD|01HY8WMN8JYKDRJC67QPQVS1N0", "externalMandateReference": "unique_ref3_02gs600s972",
                    "startDate": "2024-05-19T09:15:30.000+0000", "endDate": "2024-09-22T09:15:30.000+0000",
                    "mandateStatus": "ACTIVE", "mandateAmount": 50000, "contractCode": "4934121686", "autoRenew": false,
                    "customerPhoneNumber": "1234567890", "customerEmailAddress": "test@moniepoint.com",
                    "customerAddress": "123 Example Street, City, Country", "customerName": "Ankit Kushwaha",
                    "customerAccountName": "Ankit Kushwaha", "customerAccountNumber": "0051762787",
                    "customerAccountBankCode": "998", "mandateDescription": "Subscription Fee", "debitAmount": null,
                    "authorizationMessage": "...", "authorizationLink": "https://paylink.monnify.com/mandate-auth/abc" }
                ],
                "totalElements": 42, "totalPages": 3, "pageNumber": 0, "pageSize": 20,
                "first": true, "last": false, "numberOfElements": 20, "empty": false
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.ListMandatesAsync(new ListMandatesFilter
        {
            StartDate = "2024-01-01T00:00:00",
            EndDate = "2024-03-31T00:00:00",
            MandateStatus = "ACTIVE",
        });

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/direct-debit/mandates", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(
            "startDate=2024-01-01T00%3A00%3A00&endDate=2024-03-31T00%3A00%3A00&page=0&limit=20&mandateStatus=ACTIVE",
            handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Single(result.Content);
        Assert.Equal(42, result.TotalElements);
        Assert.Equal(3, result.TotalPages);
        Assert.Equal(20, result.NumberOfElements);
        Assert.True(result.IsFirst);
        Assert.False(result.IsLast);
    }

    [Fact]
    public async Task ListMandatesAsync_NullFilter_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.ListMandatesAsync(null!));
    }

    [Theory]
    [InlineData("", "2024-03-31T00:00:00")]
    [InlineData("2024-01-01T00:00:00", "")]
    public async Task ListMandatesAsync_MissingRequiredDateRange_Throws(string startDate, string endDate)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(
            () => client.ListMandatesAsync(new ListMandatesFilter { StartDate = startDate, EndDate = endDate }));
    }
}
