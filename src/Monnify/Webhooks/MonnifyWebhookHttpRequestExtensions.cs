using System.Text;
using Microsoft.AspNetCore.Http;

namespace Monnify.Webhooks;

/// <summary>ASP.NET Core convenience for validating an incoming Monnify webhook request.</summary>
public static class MonnifyWebhookHttpRequestExtensions
{
    private const string SignatureHeaderName = "monnify-signature";

    /// <summary>
    /// Reads the request body once, verifies its <c>monnify-signature</c> header, and returns a
    /// result you can use to get the typed envelope. This reads <see cref="HttpRequest.Body"/> to
    /// completion and does not reset its position - use this for a dedicated webhook endpoint that
    /// only needs the raw body, not one that also relies on framework model binding for the same
    /// request.
    /// </summary>
    public static async Task<MonnifyWebhookValidationResult> ValidateMonnifyWebhookAsync(
        this HttpRequest request, string secretKey, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new ArgumentException("Value must be provided.", nameof(secretKey));
        }

        string rawBody;
        using (var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true))
        {
#if NET8_0_OR_GREATER
            rawBody = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
#else
            rawBody = await reader.ReadToEndAsync().ConfigureAwait(false);
#endif
        }

        var signatureHeader = request.Headers[SignatureHeaderName].ToString();
        var isValid = MonnifyWebhookValidator.IsValid(rawBody, signatureHeader, secretKey);
        return new MonnifyWebhookValidationResult(isValid, rawBody);
    }
}
