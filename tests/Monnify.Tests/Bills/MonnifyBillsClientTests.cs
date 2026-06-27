using System.Net;
using Monnify.Bills;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Bills;

// Response payloads below are captured from real calls against the sandbox, not our own docs
// samples - our docs turned out to disagree with the real API on three points caught here: paging
// is 0-based (docs claim page=1 by default), GetBillerProductsAsync's category/biller fields are
// singular objects (docs show them as categories/billers arrays), and a required
// validationReference lives inside vendInstruction (docs only showed the "not required" case, which
// doesn't reveal where it lives). See docs/COMPATIBILITY.md and CHANGELOG.md for the writeup.
public class MonnifyBillsClientTests
{
    private static MonnifyBillsClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    [Fact]
    public async Task GetBillerCategoriesAsync_SendsGetWithZeroBasedDefaultPaging_AndDeserializesPagedResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [
                  { "code": "AIRTIME", "name": "AIRTIME" },
                  { "code": "DATA_BUNDLE", "name": "DATA_BUNDLE" },
                  { "code": "ELECTRICITY", "name": "ELECTRICITY" },
                  { "code": "TRANSPORTATION", "name": "TRANSPORTATION" },
                  { "code": "CABLE_TV", "name": "CABLE_TV" },
                  { "code": "DATA", "name": "DATA" },
                  { "code": "BETTING", "name": "BETTING" },
                  { "code": "EDUCATION", "name": "EDUCATION" }
                ],
                "totalElements": 8,
                "size": 20,
                "number": 0,
                "empty": false,
                "nextPage": null
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetBillerCategoriesAsync();

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/vas/bills-payment/biller-categories", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("page=0&size=20", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Equal(8, result.Content.Count);
        Assert.Equal("CABLE_TV", result.Content[4].Code);
        Assert.Equal(8, result.TotalElements);
        Assert.Null(result.NextPage);
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
                  { "code": "AIRTEL", "name": "AIRTEL", "categories": [ { "code": "AIRTIME", "name": "AIRTIME" }, { "code": "DATA_BUNDLE", "name": "DATA_BUNDLE" } ] },
                  { "code": "GLO", "name": "GLO", "categories": [ { "code": "AIRTIME", "name": "AIRTIME" }, { "code": "DATA_BUNDLE", "name": "DATA_BUNDLE" } ] }
                ],
                "totalElements": 40,
                "size": 5,
                "number": 0,
                "empty": false,
                "nextPage": 1
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetBillersAsync();

        Assert.Equal("page=0&size=20", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Equal(2, result.Content.Count);
        Assert.Equal("AIRTIME", result.Content[0].Categories[0].Code);
        Assert.Equal(1, result.NextPage);
    }

    [Fact]
    public async Task GetBillersAsync_WithCategoryFilter_IncludesItInQuery()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": { "content": [], "totalElements": 0, "size": 20, "number": 0, "empty": true, "nextPage": null } }
            """));
        var client = CreateClient(handler);

        await client.GetBillersAsync(categoryCode: "CABLE_TV");

        Assert.Equal("page=0&size=20&category_code=CABLE_TV", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
    }

    [Fact]
    public async Task GetBillerProductsAsync_SendsBillerCodeInQuery_AndDeserializesSingularCategoryAndBiller()
    {
        var handler = new FakeHttpMessageHandler();
        // Real sandbox shape: "category"/"biller" are singular objects, and "metadata.volume" comes
        // back as a quoted string here, not the bare number our docs sample showed.
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [
                  {
                    "code": "11",
                    "name": "Airtel Mobile Top up",
                    "category": { "code": "AIRTIME", "name": "AIRTIME" },
                    "biller": { "code": "AIRTEL", "name": "AIRTEL" },
                    "minAmount": null,
                    "maxAmount": null,
                    "price": null,
                    "priceType": "OPEN",
                    "metadata": {
                      "volume": "0",
                      "duration": 1,
                      "productType": { "code": "6", "name": "Airtime" },
                      "durationUnit": null,
                      "productCategory": null
                    }
                  }
                ],
                "totalElements": 1,
                "size": 20,
                "number": 0,
                "empty": false,
                "nextPage": null
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetBillerProductsAsync("AIRTEL");

        Assert.Equal("biller_code=AIRTEL&page=0&size=20", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Single(result.Content);
        Assert.Equal("OPEN", result.Content[0].PriceType);
        Assert.Null(result.Content[0].Price);
        Assert.Equal("AIRTIME", result.Content[0].Category!.Code);
        Assert.Equal("AIRTEL", result.Content[0].Biller!.Code);
        Assert.Equal(0m, result.Content[0].Metadata!.Volume);
        Assert.Equal("Airtime", result.Content[0].Metadata!.ProductType!.Name);
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
    public async Task ValidateCustomerAsync_NoReferenceRequired_DeserializesVendInstruction()
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
        Assert.Null(result.VendInstruction.ValidationReference);
        Assert.Null(result.MinAmount);
    }

    [Fact]
    public async Task ValidateCustomerAsync_ReferenceRequired_DeserializesNestedReferenceAndAmountLimits()
    {
        // Real sandbox response for an electricity prepaid product (productCode "product-ikedc-pre"):
        // validationReference lives inside vendInstruction, and minAmount/maxAmount appear at the
        // top level - neither was visible in our docs sample, which only covered a fixed-price
        // airtime product that doesn't require a reference.
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "priceType": "OPEN",
                "customerName": "Adene Jonah",
                "vendInstruction": { "requireValidationRef": true, "validationReference": "431986193642" },
                "minAmount": 500.0000,
                "maxAmount": 500000.0000
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.ValidateCustomerAsync(new ValidateBillCustomerRequest
        {
            ProductCode = "product-ikedc-pre",
            CustomerId = "55555666666",
        });

        Assert.True(result.VendInstruction!.RequireValidationRef);
        Assert.Equal("431986193642", result.VendInstruction.ValidationReference);
        Assert.Equal(500m, result.MinAmount);
        Assert.Equal(500000m, result.MaxAmount);
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
        // Captured from a real sandbox airtime vend (productCode "11", customerId "080123456789").
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "vendReference": "sdktest-1782576961",
                "transactionReference": "MFBP2606271716027a41",
                "vendStatus": "SUCCESS",
                "description": "Okay, purchase was successfully created.",
                "vendAmount": 100,
                "payableAmount": 110.00,
                "commission": 10.00,
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
            Amount = 100,
            Reference = "sdktest-1782576961",
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/vas/bills-payment/vend", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("SUCCESS", result.VendStatus);
        Assert.Equal(100m, result.VendAmount);
        Assert.Equal(110.00m, result.PayableAmount);
        Assert.Equal(10.00m, result.Commission);
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
                "vendReference": "sdktest-1782576961",
                "transactionReference": "MFBP2606271716027a41",
                "vendStatus": "SUCCESS",
                "description": "Okay, purchase was successfully created.",
                "vendAmount": 100,
                "payableAmount": 110.0,
                "commission": 10.0,
                "customerId": "080123456789",
                "productCode": "11",
                "productName": "Airtel Mobile Top up",
                "billerCode": "AIRTEL",
                "billerName": "AIRTEL"
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.RequeryAsync("sdktest-1782576961");

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/vas/bills-payment/requery", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("reference=sdktest-1782576961", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Equal("SUCCESS", result.VendStatus);
    }

    [Fact]
    public async Task RequeryAsync_TransactionNotFound_ThrowsMonnifyApiException()
    {
        var handler = new FakeHttpMessageHandler();
        // Real sandbox error response for an unknown reference.
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": false, "responseMessage": "Transaction not found for reference: bogus-ref", "responseCode": "99" }
            """));
        var client = CreateClient(handler);

        var ex = await Assert.ThrowsAsync<Monnify.Exceptions.MonnifyApiException>(() => client.RequeryAsync("bogus-ref"));

        Assert.Equal("Transaction not found for reference: bogus-ref", ex.ResponseMessage);
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
