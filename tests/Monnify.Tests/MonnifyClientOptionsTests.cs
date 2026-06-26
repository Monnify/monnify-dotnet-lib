namespace Monnify.Tests;

public class MonnifyClientOptionsTests
{
    [Fact]
    public void DefaultBaseUrlFor_Sandbox_ReturnsSandboxUrl() =>
        Assert.Equal(new Uri("https://sandbox.monnify.com"), MonnifyClientOptions.DefaultBaseUrlFor(MonnifyEnvironment.Sandbox));

    [Fact]
    public void DefaultBaseUrlFor_Live_ReturnsLiveUrl() =>
        Assert.Equal(new Uri("https://api.monnify.com"), MonnifyClientOptions.DefaultBaseUrlFor(MonnifyEnvironment.Live));

    [Fact]
    public void EffectiveBaseUrl_NoOverride_UsesEnvironmentDefault()
    {
        var options = new MonnifyClientOptions { Environment = MonnifyEnvironment.Live };
        Assert.Equal(new Uri("https://api.monnify.com"), options.EffectiveBaseUrl);
    }

    [Fact]
    public void EffectiveBaseUrl_WithOverride_UsesOverride()
    {
        var options = new MonnifyClientOptions { BaseUrl = new Uri("https://proxy.example.com") };
        Assert.Equal(new Uri("https://proxy.example.com"), options.EffectiveBaseUrl);
    }
}
