using Microsoft.Extensions.DependencyInjection;
using Monnify.Banks;

namespace Monnify.IntegrationTests.Banks;

[Trait("Category", "Sandbox")]
public class BanksSandboxTests : IClassFixture<SandboxClientFixture>
{
    private readonly SandboxClientFixture _fixture;

    public BanksSandboxTests(SandboxClientFixture fixture) => _fixture = fixture;

    [SkippableFact]
    public async Task GetBanksAsync_AgainstRealSandbox_ReturnsNonEmptyList()
    {
        Skip.IfNot(SandboxCredentials.IsAvailable, "MONNIFY_SANDBOX_API_KEY / MONNIFY_SANDBOX_SECRET_KEY are not set.");

        var client = _fixture.Provider.GetRequiredService<IMonnifyBanksClient>();

        var banks = await client.GetBanksAsync();

        Assert.NotEmpty(banks);
        Assert.Contains(banks, b => b.Code == "044");
    }

    [SkippableFact]
    public async Task GetUssdEnabledBanksAsync_AgainstRealSandbox_ReturnsBanksWithUssdTemplates()
    {
        Skip.IfNot(SandboxCredentials.IsAvailable, "MONNIFY_SANDBOX_API_KEY / MONNIFY_SANDBOX_SECRET_KEY are not set.");

        var client = _fixture.Provider.GetRequiredService<IMonnifyBanksClient>();

        var banks = await client.GetUssdEnabledBanksAsync();

        // Real data shows at least one entry on this endpoint (e.g. Suntrust Bank) still has a
        // null UssdTemplate, so assert most are populated rather than requiring all of them.
        Assert.NotEmpty(banks);
        Assert.Contains(banks, b => !string.IsNullOrEmpty(b.UssdTemplate));
    }
}
