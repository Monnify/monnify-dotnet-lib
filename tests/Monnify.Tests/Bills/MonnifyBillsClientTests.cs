using System.Net;
using Monnify.Bills;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Bills;

// Every response payload below is Monnify's own documented sample for that endpoint, copied
// verbatim, so these tests prove the client/models match real shapes rather than ones we guessed.
public class MonnifyBillsClientTests
{
    private static MonnifyBillsClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    [Fact]
    public async Task GetBillerCategoriesAsync_SendsGetWithDefaultPaging_AndDeserializesPagedResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [
                  { "code": "TRANSPORTATION", "name": "TRANSPORTATION" },
                  { "code": "CABLE_TV", "name": "CABLE_TV" },
                  { "code": "DATA", "name": "DATA" }
                ],
                "totalElements": 8,
                "size": 3,
                "number": 1,
                "empty": false,
                "nextPage": 2
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetBillerCategoriesAsync();

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/vas/bills-payment/biller-categories", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("page=1&size=10", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Equal(3, result.Content.Count);
        Assert.Equal("CABLE_TV", result.Content[1].Code);
        Assert.Equal(8, result.TotalElements);
        Assert.Equal(2, result.NextPage);
        Assert.False(result.IsEmpty);
    }

    [Fact]
    public async Task GetBillersAsync_NoCategoryFilter_OmitsItFromQuery_AndDeserializesNestedCategories()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [
                  { "code": "biller-dstv", "name": "DSTV", "categories": [ { "code": "CABLE_TV", "name": "CABLE_TV" } ] },
                  { "code": "biller-spectranet", "name": "Spectranet", "categories": [ { "code": "DATA", "name": "DATA" } ] }
                ],
                "totalElements": 66,
                "size": 20,
                "number": 1,
                "empty": false,
                "nextPage": null
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetBillersAsync();

        Assert.Equal("page=1&size=20", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Equal(2, result.Content.Count);
        Assert.Equal("CABLE_TV", result.Content[0].Categories[0].Code);
        Assert.Null(result.NextPage);
    }

    [Fact]
    public async Task GetBillersAsync_WithCategoryFilter_IncludesItInQuery()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": { "content": [], "totalElements": 0, "size": 20, "number": 1, "empty": true, "nextPage": null } }
            """));
        var client = CreateClient(handler);

        await client.GetBillersAsync(categoryCode: "CABLE_TV");

        Assert.Equal("page=1&size=20&category_code=CABLE_TV", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
    }

    [Fact]
    public async Task GetBillerProductsAsync_SendsBillerCodeInQuery_AndDeserializesPricingAndMetadata()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [
                  {
                    "code": "11",
                    "name": "Airtel Mobile Top up",
                    "categories": [ { "code": "AIRTIME", "name": "AIRTIME" } ],
                    "billers": [ { "code": "AIRTEL", "name": "AIRTEL" } ],
                    "minAmount": null,
                    "maxAmount": null,
                    "price": null,
                    "priceType": "OPEN",
                    "metadata": {
                      "volume": 0,
                      "duration": 1,
                      "productType": { "code": "6", "name": "Airtime" },
                      "durationUnit": null,
                      "productCategory": null
                    }
                  },
                  {
                    "code": "19523",
                    "name": "DataPlan 750MB",
                    "categories": [ { "code": "DATA_BUNDLE", "name": "DATA_BUNDLE" } ],
                    "billers": [ { "code": "MTN", "name": "MTN" } ],
                    "minAmount": null,
                    "maxAmount": null,
                    "price": 500,
                    "priceType": "FIXED",
                    "metadata": {
                      "volume": 750,
                      "duration": 1,
                      "productType": { "code": "9", "name": "Data" },
                      "durationUnit": "WEEKLY",
                      "productCategory": "DAILY"
                    }
                  }
                ],
                "totalElements": 14,
                "size": 5,
                "number": 0,
                "empty": false,
                "nextPage": 1
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetBillerProductsAsync("AIRTEL");

        Assert.Equal("biller_code=AIRTEL&page=1&size=20", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Equal(2, result.Content.Count);
        Assert.Equal("OPEN", result.Content[0].PriceType);
        Assert.Null(result.Content[0].Price);
        Assert.Equal("FIXED", result.Content[1].PriceType);
        Assert.Equal(500m, result.Content[1].Price);
        Assert.Equal(750m, result.Content[1].Metadata!.Volume);
        Assert.Equal("Data", result.Content[1].Metadata!.ProductType!.Name);
        Assert.Equal("MTN", result.Content[1].Billers[0].Code);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetBillerProductsAsync_MissingBillerCode_ThrowsArgumentException(string? billerCode)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetBillerProductsAsync(billerCode!));
    }

    [Fact]
    public async Task ValidateCustomerAsync_SendsPostWithJsonBody_AndDeserializesVendInstruction()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "priceType": "OPEN",
                "customerName": "+2349135914842",
                "vendInstruction": { "requireValidationRef": false }
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.ValidateCustomerAsync(new ValidateBillCustomerRequest
        {
            ProductCode = "11",
            CustomerId = "080123456789",
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/vas/bills-payment/validate-customer", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"productCode\":\"11\"", handler.RequestBodies[0]);
        Assert.Equal("OPEN", result.PriceType);
        Assert.False(result.VendInstruction!.RequireValidationRef);
        Assert.Null(result.ValidationReference);
    }

    [Fact]
    public async Task ValidateCustomerAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.ValidateCustomerAsync(null!));
    }

    [Fact]
    public async Task VendAsync_SendsPostWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "vendReference": "REF-MH0H27YL-966MVT",
                "transactionReference": "MFBP251021121937dbf0",
                "vendStatus": "SUCCESS",
                "description": "Okay, purchase was successfully created.",
                "vendAmount": 300,
                "payableAmount": 310,
                "commission": 10,
                "customerId": "080123456789",
                "productCode": "11",
                "productName": "Airtel Mobile Top up",
                "billerCode": "AIRTEL",
                "billerName": "AIRTEL"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.VendAsync(new VendBillRequest
        {
            ProductCode = "11",
            CustomerId = "080123456789",
            Amount = 300,
            ValidationReference = "REF-MH0H27YL-966MVT",
            EmailAddress = "customer@example.com",
            Reference = "REF-MH0H27YL-966MVT",
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/vas/bills-payment/vend", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("SUCCESS", result.VendStatus);
        Assert.Equal(300m, result.VendAmount);
        Assert.Equal(310m, result.PayableAmount);
        Assert.Equal(10m, result.Commission);
        Assert.Equal("AIRTEL", result.BillerCode);
    }

    [Fact]
    public async Task VendAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.VendAsync(null!));
    }

    [Fact]
    public async Task RequeryAsync_SendsGetWithReferenceQuery_AndDeserializesSameShapeAsVend()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "vendReference": "REF-MH0H27YL-966MVT",
                "transactionReference": "MFBP251021121937dbf0",
                "vendStatus": "SUCCESS",
                "description": "Okay, purchase was successfully created.",
                "vendAmount": 300,
                "payableAmount": 310,
                "commission": 10,
                "customerId": "080123456789",
                "productCode": "11",
                "productName": "Airtel Mobile Top up",
                "billerCode": "AIRTEL",
                "billerName": "AIRTEL"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.RequeryAsync("REF-MH0H27YL-966MVT");

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/vas/bills-payment/requery", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("reference=REF-MH0H27YL-966MVT", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Equal("SUCCESS", result.VendStatus);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task RequeryAsync_MissingReference_ThrowsArgumentException(string? reference)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.RequeryAsync(reference!));
    }
}
