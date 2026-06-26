using System.Net;
using System.Text;
using Monnify.Authentication;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Authentication;

public class MonnifyAuthHandlerTests
{
    private static (HttpClient Client, FakeHttpMessageHandler Inner, SequentialFakeTokenProvider TokenProvider) CreateClient(params string[] tokens)
    {
        var inner = new FakeHttpMessageHandler();
        var tokenProvider = new SequentialFakeTokenProvider(tokens);
        var handler = new MonnifyAuthHandler(tokenProvider) { InnerHandler = inner };
        var client = new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") };
        return (client, inner, tokenProvider);
    }

    [Fact]
    public async Task SendAsync_AddsBearerTokenFromProvider()
    {
        var (client, inner, _) = CreateClient("token-1");
        inner.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, "{}"));

        await client.GetAsync("/ping");

        Assert.Equal("Bearer", inner.Requests[0].Headers.Authorization!.Scheme);
        Assert.Equal("token-1", inner.Requests[0].Headers.Authorization!.Parameter);
    }

    [Fact]
    public async Task SendAsync_RetriesOnceOn401_WithRefreshedToken()
    {
        var (client, inner, tokenProvider) = CreateClient("token-1", "token-2");
        inner.Enqueue(HttpResponseFactory.Unauthorized());
        inner.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, "{}"));

        var response = await client.GetAsync("/ping");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, inner.Requests.Count);
        Assert.Equal("token-1", inner.Requests[0].Headers.Authorization!.Parameter);
        Assert.Equal("token-2", inner.Requests[1].Headers.Authorization!.Parameter);
        Assert.Equal(1, tokenProvider.InvalidateCount);
    }

    [Fact]
    public async Task SendAsync_DoesNotRetry_MoreThanOnce()
    {
        var (client, inner, _) = CreateClient("token-1", "token-2");
        inner.Enqueue(HttpResponseFactory.Unauthorized());
        inner.Enqueue(HttpResponseFactory.Unauthorized());

        var response = await client.GetAsync("/ping");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal(2, inner.Requests.Count);
    }

    [Fact]
    public async Task SendAsync_OnRetry_PreservesRequestBody()
    {
        var (client, inner, _) = CreateClient("token-1", "token-2");
        inner.Enqueue(HttpResponseFactory.Unauthorized());
        inner.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, "{}"));

        await client.PostAsync("/pay", new StringContent("""{"amount":100}""", Encoding.UTF8, "application/json"));

        Assert.Equal("""{"amount":100}""", inner.RequestBodies[1]);
    }

    [Fact]
    public async Task SendAsync_NonUnauthorizedResponse_DoesNotInvalidateOrRetry()
    {
        var (client, inner, tokenProvider) = CreateClient("token-1");
        inner.Enqueue(HttpResponseFactory.Json(HttpStatusCode.BadRequest, "{}"));

        var response = await client.GetAsync("/ping");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Single(inner.Requests);
        Assert.Equal(0, tokenProvider.InvalidateCount);
    }
}
