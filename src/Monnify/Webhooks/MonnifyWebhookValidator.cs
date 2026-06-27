using System.Security.Cryptography;
using System.Text;

namespace Monnify.Webhooks;

/// <summary>
/// Verifies the <c>monnify-signature</c> header Monnify sends with every webhook request, computed
/// as <c>HMAC-SHA512(key: secretKey, message: rawRequestBody)</c>. Always verify the signature
/// before acting on a webhook body — anyone can otherwise POST a fake event to your endpoint.
/// Treat an IP allowlist as defense-in-depth at most, since Monnify's sending IP can change;
/// signature verification is the primary control.
/// </summary>
public static class MonnifyWebhookValidator
{
    /// <summary>
    /// Computes the expected signature for a webhook body and compares it against the
    /// <c>monnify-signature</c> header value using a constant-time comparison.
    /// </summary>
    /// <param name="rawRequestBody">The exact, unmodified request body bytes/text Monnify sent - re-serializing a parsed object will not match.</param>
    /// <param name="signatureHeader">The value of the <c>monnify-signature</c> request header.</param>
    /// <param name="secretKey">Your Monnify secret key.</param>
    public static bool IsValid(string rawRequestBody, string? signatureHeader, string secretKey)
    {
        RequireValue(rawRequestBody, nameof(rawRequestBody));
        RequireValue(secretKey, nameof(secretKey));

        if (string.IsNullOrWhiteSpace(signatureHeader))
        {
            return false;
        }

        var expected = ComputeSignature(rawRequestBody, secretKey);
        return FixedTimeEqualsIgnoreCase(expected, signatureHeader!.Trim());
    }

    /// <summary>Computes the lowercase hex-encoded signature Monnify expects for a given body and secret key.</summary>
    public static string ComputeSignature(string rawRequestBody, string secretKey)
    {
        RequireValue(rawRequestBody, nameof(rawRequestBody));
        RequireValue(secretKey, nameof(secretKey));

        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawRequestBody));
        return ToLowerHex(hash);
    }

    private static string ToLowerHex(byte[] bytes)
    {
#if NET8_0_OR_GREATER
        return Convert.ToHexString(bytes).ToLowerInvariant();
#else
        var builder = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
#endif
    }

    private static bool FixedTimeEqualsIgnoreCase(string expectedLowerHex, string actual)
    {
        var actualLower = actual.ToLowerInvariant();
        if (expectedLowerHex.Length != actualLower.Length)
        {
            return false;
        }

#if NET8_0_OR_GREATER
        return CryptographicOperations.FixedTimeEquals(
            Encoding.ASCII.GetBytes(expectedLowerHex), Encoding.ASCII.GetBytes(actualLower));
#else
        var diff = 0;
        for (var i = 0; i < expectedLowerHex.Length; i++)
        {
            diff |= expectedLowerHex[i] ^ actualLower[i];
        }

        return diff == 0;
#endif
    }

    private static void RequireValue(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value must be provided.", paramName);
        }
    }
}
