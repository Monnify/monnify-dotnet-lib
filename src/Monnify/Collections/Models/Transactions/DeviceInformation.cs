using System.Text.Json.Serialization;

namespace Monnify.Collections;

/// <summary>
/// Browser/device fingerprint sent alongside a card charge, used for fraud screening and 3DS.
/// Field names mirror the EMV 3-D Secure browser-data fields our docs use verbatim.
/// </summary>
public sealed class DeviceInformation
{
    [JsonPropertyName("httpBrowserLanguage")]
    public string HttpBrowserLanguage { get; set; } = string.Empty;

    [JsonPropertyName("httpBrowserJavaEnabled")]
    public bool HttpBrowserJavaEnabled { get; set; }

    [JsonPropertyName("httpBrowserJavaScriptEnabled")]
    public bool HttpBrowserJavaScriptEnabled { get; set; }

    [JsonPropertyName("httpBrowserColorDepth")]
    public int HttpBrowserColorDepth { get; set; }

    [JsonPropertyName("httpBrowserScreenHeight")]
    public int HttpBrowserScreenHeight { get; set; }

    [JsonPropertyName("httpBrowserScreenWidth")]
    public int HttpBrowserScreenWidth { get; set; }

    /// <summary>The browser's UTC offset in minutes, as a string. Our own documented sample leaves this empty.</summary>
    [JsonPropertyName("httpBrowserTimeDifference")]
    public string HttpBrowserTimeDifference { get; set; } = string.Empty;

    [JsonPropertyName("userAgentBrowserValue")]
    public string UserAgentBrowserValue { get; set; } = string.Empty;
}
