using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Monnify.Authentication;
using Monnify.Disbursements;

namespace Monnify.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddMonnify_NullServices_Throws()
    {
        IServiceCollection services = null!;
        Assert.Throws<ArgumentNullException>(() => services.AddMonnify(_ => { }));
    }

    [Fact]
    public void AddMonnify_NullConfigureOptions_Throws()
    {
        var services = new ServiceCollection();
        Assert.Throws<ArgumentNullException>(() => services.AddMonnify(null!));
    }

    [Fact]
    public void AddMonnify_RegistersTokenProvider_AsSingleton()
    {
        var services = new ServiceCollection();
        services.AddMonnify(o =>
        {
            o.ApiKey = "key";
            o.SecretKey = "secret";
        });
        using var provider = services.BuildServiceProvider();

        var first = provider.GetRequiredService<IMonnifyTokenProvider>();
        var second = provider.GetRequiredService<IMonnifyTokenProvider>();

        Assert.Same(first, second);
    }

    [Fact]
    public void AddMonnify_MissingApiKey_ThrowsOptionsValidationException_OnAccess()
    {
        var services = new ServiceCollection();
        services.AddMonnify(o => o.SecretKey = "secret");
        using var provider = services.BuildServiceProvider();

        var options = provider.GetRequiredService<IOptionsMonitor<MonnifyClientOptions>>();
        Assert.Throws<OptionsValidationException>(() => options.CurrentValue);
    }

    [Fact]
    public void AddMonnify_MissingSecretKey_ThrowsOptionsValidationException_OnAccess()
    {
        var services = new ServiceCollection();
        services.AddMonnify(o => o.ApiKey = "key");
        using var provider = services.BuildServiceProvider();

        var options = provider.GetRequiredService<IOptionsMonitor<MonnifyClientOptions>>();
        Assert.Throws<OptionsValidationException>(() => options.CurrentValue);
    }

    [Fact]
    public void AddMonnify_NonHttpsBaseUrl_ThrowsOptionsValidationException_OnAccess()
    {
        var services = new ServiceCollection();
        services.AddMonnify(o =>
        {
            o.ApiKey = "key";
            o.SecretKey = "secret";
            o.BaseUrl = new Uri("http://insecure.example.com");
        });
        using var provider = services.BuildServiceProvider();

        var options = provider.GetRequiredService<IOptionsMonitor<MonnifyClientOptions>>();
        Assert.Throws<OptionsValidationException>(() => options.CurrentValue);
    }

    [Fact]
    public void AddMonnify_ValidOptions_DoesNotThrow_OnAccess()
    {
        var services = new ServiceCollection();
        services.AddMonnify(o =>
        {
            o.ApiKey = "key";
            o.SecretKey = "secret";
        });
        using var provider = services.BuildServiceProvider();

        var options = provider.GetRequiredService<IOptionsMonitor<MonnifyClientOptions>>();
        Assert.Equal("key", options.CurrentValue.ApiKey);
    }

    [Fact]
    public void AddMonnify_ResolvesDisbursementsClient_WithRetryDisabled_WithoutThrowing()
    {
        // Disbursements registers with allowAutomaticRetry: false (MaxRetryAttempts = 0); confirms
        // that's a value the resilience pipeline actually accepts, rather than only finding out at
        // first real HTTP call time.
        var services = new ServiceCollection();
        services.AddMonnify(o =>
        {
            o.ApiKey = "key";
            o.SecretKey = "secret";
        });
        using var provider = services.BuildServiceProvider();

        var client = provider.GetRequiredService<IMonnifyDisbursementsClient>();

        Assert.NotNull(client);
    }

    [Fact]
    public void AddMonnify_ResolvesMonnifyClientFacade_WithEveryTypedClient()
    {
        var services = new ServiceCollection();
        services.AddMonnify(o =>
        {
            o.ApiKey = "key";
            o.SecretKey = "secret";
        });
        using var provider = services.BuildServiceProvider();

        var facade = provider.GetRequiredService<MonnifyClient>();

        Assert.NotNull(facade.Banks);
        Assert.NotNull(facade.Verification);
        Assert.NotNull(facade.Collections);
        Assert.NotNull(facade.Disbursements);
    }
}
