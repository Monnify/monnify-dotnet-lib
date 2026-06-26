using System.Net;
using Monnify.Banks;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Banks;

public class MonnifyBanksClientTests
{
    private static MonnifyBanksClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    [Fact]
    public async Task GetBanksAsync_SendsGetToBanksEndpoint_AndDeserializesList()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": [
                { "name": "Access bank", "code": "044", "nipBankCode": "000014" },
                { "name": "Zenith Bank", "code": "057", "nipBankCode": "000015" }
              ] }
            """));
        var client = CreateClient(handler);

        var banks = await client.GetBanksAsync();

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/banks", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(2, banks.Count);
        Assert.Equal("Access bank", banks[0].Name);
        Assert.Equal("044", banks[0].Code);
    }

    [Fact]
    public async Task GetUssdEnabledBanksAsync_SendsGetToUssdEndpoint_AndDeserializesTemplates()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": [
                { "name": "Access bank", "code": "044", "ussdTemplate": "*901*Amount*AccountNumber#", "baseUssdCode": "*901#", "transferUssdTemplate": "*901*AccountNumber#" }
              ] }
            """));
        var client = CreateClient(handler);

        var banks = await client.GetUssdEnabledBanksAsync();

        Assert.Equal("/api/v1/sdk/transactions/banks", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("*901*Amount*AccountNumber#", banks[0].UssdTemplate);
        Assert.Equal("*901#", banks[0].BaseUssdCode);
    }
}
