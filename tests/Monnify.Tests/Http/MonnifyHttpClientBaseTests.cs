using System.Net;
using System.Text.Json.Serialization;
using Monnify.Exceptions;
using Monnify.Http;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Http;

public class MonnifyHttpClientBaseTests
{
    private sealed class FakePayload
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    private sealed class FakeTypedClient : MonnifyHttpClientBase
    {
        public FakeTypedClient(HttpClient httpClient) : base(httpClient) { }

        public Task<TResponseBody> Call<TResponseBody>(CancellationToken cancellationToken = default) =>
            SendAsync<TResponseBody>(new HttpRequestMessage(HttpMethod.Get, "/anything"), cancellationToken);
    }

    private static FakeTypedClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    [Fact]
    public async Task SendAsync_SuccessfulEnvelope_ReturnsDeserializedResponseBody()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0", "responseBody": { "name": "Ada" } }
            """));
        var client = CreateClient(handler);

        var result = await client.Call<FakePayload>();

        Assert.Equal("Ada", result.Name);
    }

    [Fact]
    public async Task SendAsync_RequestSuccessfulFalse_ThrowsMonnifyApiException_WithResponseCodeAndMessage()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": false, "responseMessage": "Invalid request", "responseCode": "99" }
            """));
        var client = CreateClient(handler);

        var ex = await Assert.ThrowsAsync<MonnifyApiException>(() => client.Call<FakePayload>());

        Assert.Equal("99", ex.ResponseCode);
        Assert.Equal("Invalid request", ex.ResponseMessage);
        Assert.Equal(200, ex.HttpStatusCode);
    }

    [Fact]
    public async Task SendAsync_NonSuccessHttpStatus_ThrowsMonnifyApiException_PreservingRawBody()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.InternalServerError, "not an envelope at all"));
        var client = CreateClient(handler);

        var ex = await Assert.ThrowsAsync<MonnifyDeserializationException>(() => client.Call<FakePayload>());

        Assert.Equal("not an envelope at all", ex.RawResponseBody);
    }

    [Fact]
    public async Task SendAsync_SuccessfulButNullResponseBody_ThrowsMonnifyDeserializationException()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0", "responseBody": null }
            """));
        var client = CreateClient(handler);

        await Assert.ThrowsAsync<MonnifyDeserializationException>(() => client.Call<FakePayload>());
    }
}
