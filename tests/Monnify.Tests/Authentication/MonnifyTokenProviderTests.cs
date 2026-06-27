using System.Net;
using Microsoft.Extensions.Options;
using Monnify.Authentication;
using Monnify.Exceptions;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Authentication;

public class MonnifyTokenProviderTests
{
    private static MonnifyTokenProvider CreateProvider(FakeHttpMessageHandler handler, MonnifyClientOptions? options = null)
    {
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") };
        var factory = new SingleClientHttpClientFactory(httpClient);
        var optionsMonitor = new StaticOptionsMonitor<MonnifyClientOptions>(
            options ?? new MonnifyClientOptions { ApiKey = "test-key", SecretKey = "test-secret" });
        return new MonnifyTokenProvider(factory, optionsMonitor);
    }

    [Fact]
    public async Task GetAccessTokenAsync_LogsIn_AndReturnsToken()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.LoginSuccess("token-1"));
        using var provider = CreateProvider(handler);

        var token = await provider.GetAccessTokenAsync();

        Assert.Equal("token-1", token);
        Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/auth/login", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("Basic", handler.Requests[0].Headers.Authorization!.Scheme);
    }

    [Fact]
    public async Task GetAccessTokenAsync_SendsBasicAuthHeader_EncodingApiKeyAndSecret()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.LoginSuccess("token-1"));
        using var provider = CreateProvider(handler, new MonnifyClientOptions { ApiKey = "MK_KEY", SecretKey = "MK_SECRET" });

        await provider.GetAccessTokenAsync();

        var expected = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("MK_KEY:MK_SECRET"));
        Assert.Equal(expected, handler.Requests[0].Headers.Authorization!.Parameter);
    }

    [Fact]
    public async Task GetAccessTokenAsync_WithinBuffer_ReturnsCachedToken_WithoutSecondLogin()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.LoginSuccess("token-1", expiresIn: 3600));
        using var provider = CreateProvider(handler);

        var first = await provider.GetAccessTokenAsync();
        var second = await provider.GetAccessTokenAsync();

        Assert.Equal("token-1", first);
        Assert.Equal("token-1", second);
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task GetAccessTokenAsync_AfterExpiryBuffer_LogsInAgain()
    {
        var handler = new FakeHttpMessageHandler();
        // expiresIn (1s) is shorter than the 60s refresh buffer, so the cached token is
        // immediately treated as not-usable on the very next call without waiting real time.
        handler.Enqueue(HttpResponseFactory.LoginSuccess("token-1", expiresIn: 1));
        handler.Enqueue(HttpResponseFactory.LoginSuccess("token-2", expiresIn: 3600));
        using var provider = CreateProvider(handler);

        var first = await provider.GetAccessTokenAsync();
        var second = await provider.GetAccessTokenAsync();

        Assert.Equal("token-1", first);
        Assert.Equal("token-2", second);
        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task GetAccessTokenAsync_ConcurrentCallers_OnlyLogInOnce()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.LoginSuccess("token-1"));
        using var provider = CreateProvider(handler);

        var results = await Task.WhenAll(Enumerable.Range(0, 20).Select(_ => provider.GetAccessTokenAsync().AsTask()));

        Assert.All(results, token => Assert.Equal("token-1", token));
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task GetAccessTokenAsync_HttpFailure_ThrowsMonnifyAuthenticationException()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.Unauthorized, """{ "requestSuccessful": false, "responseMessage": "bad credentials", "responseCode": "99" }"""));
        using var provider = CreateProvider(handler);

        var exception = await Assert.ThrowsAsync<MonnifyAuthenticationException>(() => provider.GetAccessTokenAsync().AsTask());
        Assert.DoesNotContain("test-secret", exception.Message);
    }

    [Fact]
    public async Task GetAccessTokenAsync_MalformedJson_ThrowsMonnifyAuthenticationException()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, "not json"));
        using var provider = CreateProvider(handler);

        await Assert.ThrowsAsync<MonnifyAuthenticationException>(() => provider.GetAccessTokenAsync().AsTask());
    }

    [Fact]
    public async Task GetAccessTokenAsync_RequestSuccessfulFalse_ThrowsMonnifyAuthenticationException_WithRealMessage()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """{ "requestSuccessful": false, "responseMessage": "invalid key", "responseCode": "99" }"""));
        using var provider = CreateProvider(handler);

        var ex = await Assert.ThrowsAsync<MonnifyAuthenticationException>(() => provider.GetAccessTokenAsync().AsTask());
        Assert.Contains("invalid key", ex.Message);
    }

    [Fact]
    public async Task GetAccessTokenAsync_RequestSuccessfulFalse_NonEnvelopeBody_FallsBackToUnknownError()
    {
        // A 200 OK that doesn't match Monnify's usual envelope shape at all (e.g. a misconfigured
        // gateway). Confirms the "unknown error" fallback actually fires instead of rendering an
        // empty responseMessage.
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """{ "status": "ok but not the shape we expect" }"""));
        using var provider = CreateProvider(handler);

        var ex = await Assert.ThrowsAsync<MonnifyAuthenticationException>(() => provider.GetAccessTokenAsync().AsTask());
        Assert.Contains("unknown error", ex.Message);
    }

    [Fact]
    public async Task Invalidate_ForcesNextCall_ToLogInAgain()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.LoginSuccess("token-1"));
        handler.Enqueue(HttpResponseFactory.LoginSuccess("token-2"));
        using var provider = CreateProvider(handler);

        await provider.GetAccessTokenAsync();
        provider.Invalidate();
        var second = await provider.GetAccessTokenAsync();

        Assert.Equal("token-2", second);
        Assert.Equal(2, handler.Requests.Count);
    }
}
