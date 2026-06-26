using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Monnify.Authentication;

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
}
