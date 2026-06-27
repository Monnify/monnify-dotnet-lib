using System.Net;
using System.Text.Json.Serialization;
using Monnify.Authentication;
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

    [Fact]
    public async Task SendAsync_Forbidden_ThrowsMonnifyApiException_WithRealResponseMessage()
    {
        // e.g. a feature (like Disbursements) that hasn't been activated for the merchant yet.
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.Forbidden, """
            { "requestSuccessful": false, "responseMessage": "Merchant is not permitted to perform this operation", "responseCode": "99" }
            """));
        var client = CreateClient(handler);

        var ex = await Assert.ThrowsAsync<MonnifyApiException>(() => client.Call<FakePayload>());

        Assert.Equal(403, ex.HttpStatusCode);
        Assert.Equal("Merchant is not permitted to perform this operation", ex.ResponseMessage);
        Assert.Equal("99", ex.ResponseCode);
    }

    [Fact]
    public async Task SendAsync_NonEnvelopeErrorBody_ThrowsMonnifyApiException_WithFallbackMessage()
    {
        // Some failures (e.g. a gateway-level rejection) don't come back as Monnify's usual
        // envelope at all. Confirms the SDK still surfaces a clear exception instead of crashing
        // or misreporting success, even when there's no responseMessage to forward.
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.Forbidden, """
            { "timestamp": "2026-06-27T08:00:00.000+00:00", "status": 403, "error": "Forbidden", "path": "/api/v2/disbursements/single" }
            """));
        var client = CreateClient(handler);

        var ex = await Assert.ThrowsAsync<MonnifyApiException>(() => client.Call<FakePayload>());

        Assert.Equal(403, ex.HttpStatusCode);
        Assert.Contains("HTTP 403", ex.ResponseMessage);
    }

    [Fact]
    public async Task SendAsync_PersistentlyUnauthorized_SurfacesRealMonnifyMessage_ThroughAuthRetry()
    {
        // Simulates a 401 caused by a permission/feature issue rather than an actually-stale
        // token: MonnifyAuthHandler still retries once (it can't tell the difference), but the
        // real Monnify message from the second attempt must reach the caller, not get swallowed
        // by the retry-on-401 logic.
        var inner = new FakeHttpMessageHandler();
        const string body = """
            { "requestSuccessful": false, "responseMessage": "Disbursement feature not enabled for this merchant", "responseCode": "99" }
            """;
        inner.Enqueue(HttpResponseFactory.Json(HttpStatusCode.Unauthorized, body));
        inner.Enqueue(HttpResponseFactory.Json(HttpStatusCode.Unauthorized, body));
        var tokenProvider = new SequentialFakeTokenProvider("token-1", "token-2");
        var authHandler = new MonnifyAuthHandler(tokenProvider) { InnerHandler = inner };
        var client = new FakeTypedClient(new HttpClient(authHandler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

        var ex = await Assert.ThrowsAsync<MonnifyApiException>(() => client.Call<FakePayload>());

        Assert.Equal(2, inner.Requests.Count);
        Assert.Equal(401, ex.HttpStatusCode);
        Assert.Equal("Disbursement feature not enabled for this merchant", ex.ResponseMessage);
        Assert.Equal("99", ex.ResponseCode);
    }
}
