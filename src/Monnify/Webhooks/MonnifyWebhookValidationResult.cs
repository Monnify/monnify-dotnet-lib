namespace Monnify.Webhooks;

/// <summary>The result of <see cref="MonnifyWebhookHttpRequestExtensions.ValidateMonnifyWebhookAsync"/>.</summary>
public sealed class MonnifyWebhookValidationResult
{
    internal MonnifyWebhookValidationResult(bool isValid, string rawBody)
    {
        IsValid = isValid;
        RawBody = rawBody;
    }

    /// <summary>
    /// Whether the <c>monnify-signature</c> header matched. Always <see langword="false"/> against
    /// Monnify's sandbox, which sends no signature header at all - see the remarks on
    /// <see cref="MonnifyWebhookValidator"/>.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>The raw request body that was read and validated, for logging/diagnostics.</summary>
    public string RawBody { get; }

    /// <summary>Parses the envelope out of <see cref="RawBody"/>.</summary>
    public MonnifyWebhookEnvelope GetEnvelope() => MonnifyWebhookParser.Parse(RawBody);
}
