using Microsoft.Extensions.DependencyInjection;
using Monnify.Authentication;

namespace Monnify.IntegrationTests.Authentication;

[Trait("Category", "Sandbox")]
public class AuthenticationSandboxTests : IClassFixture<SandboxClientFixture>
{
    private readonly SandboxClientFixture _fixture;

    public AuthenticationSandboxTests(SandboxClientFixture fixture) => _fixture = fixture;

    [SkippableFact]
    public async Task GetAccessTokenAsync_AgainstRealSandbox_ReturnsNonEmptyToken()
    {
        Skip.IfNot(SandboxCredentials.IsAvailable, "MONNIFY_SANDBOX_API_KEY / MONNIFY_SANDBOX_SECRET_KEY are not set.");

        var tokenProvider = _fixture.Provider.GetRequiredService<IMonnifyTokenProvider>();

        var token = await tokenProvider.GetAccessTokenAsync();

        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [SkippableFact]
    public async Task GetAccessTokenAsync_CalledTwice_ReturnsCachedToken_WithinExpiry()
    {
        Skip.IfNot(SandboxCredentials.IsAvailable, "MONNIFY_SANDBOX_API_KEY / MONNIFY_SANDBOX_SECRET_KEY are not set.");

        var tokenProvider = _fixture.Provider.GetRequiredService<IMonnifyTokenProvider>();

        var first = await tokenProvider.GetAccessTokenAsync();
        var second = await tokenProvider.GetAccessTokenAsync();

        Assert.Equal(first, second);
    }
}
