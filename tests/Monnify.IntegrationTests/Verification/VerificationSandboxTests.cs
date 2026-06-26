using Microsoft.Extensions.DependencyInjection;
using Monnify.Verification;

namespace Monnify.IntegrationTests.Verification;

[Trait("Category", "Sandbox")]
public class VerificationSandboxTests : IClassFixture<SandboxClientFixture>
{
    private readonly SandboxClientFixture _fixture;

    public VerificationSandboxTests(SandboxClientFixture fixture) => _fixture = fixture;

    [SkippableFact]
    public async Task ValidateAccountNumberAsync_AgainstRealSandbox_ResolvesAccountName()
    {
        Skip.IfNot(SandboxCredentials.IsAvailable, "MONNIFY_SANDBOX_API_KEY / MONNIFY_SANDBOX_SECRET_KEY are not set.");

        var client = _fixture.Provider.GetRequiredService<IMonnifyVerificationClient>();

        var result = await client.ValidateAccountNumberAsync("0123456789", "044");

        Assert.Equal("0123456789", result.AccountNumber);
        Assert.False(string.IsNullOrWhiteSpace(result.AccountName));
        Assert.Equal("NGN", result.CurrencyCode);
    }
}
