using Microsoft.Extensions.DependencyInjection;

namespace Monnify.IntegrationTests;

/// <summary>Builds a real DI container wired up exactly as a consumer's app would via AddMonnify.</summary>
public sealed class SandboxClientFixture : IDisposable
{
    public SandboxClientFixture()
    {
        var services = new ServiceCollection();
        services.AddMonnify(options =>
        {
            options.ApiKey = SandboxCredentials.ApiKey ?? string.Empty;
            options.SecretKey = SandboxCredentials.SecretKey ?? string.Empty;
            options.Environment = MonnifyEnvironment.Sandbox;
        });

        Provider = services.BuildServiceProvider();
    }

    public ServiceProvider Provider { get; }

    public void Dispose() => Provider.Dispose();
}
