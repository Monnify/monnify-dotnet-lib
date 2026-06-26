namespace Monnify.Exceptions;

/// <summary>
/// Thrown when a response that should have matched Monnify's standard envelope shape could not
/// be parsed. Carries the raw body so the unexpected shape is debuggable rather than silently lost.
/// </summary>
public sealed class MonnifyDeserializationException : MonnifyException
{
    public MonnifyDeserializationException(string message, string rawResponseBody, Exception innerException)
        : base(message, innerException)
    {
        RawResponseBody = rawResponseBody;
    }

    /// <summary>The raw, unparsed response body.</summary>
    public string RawResponseBody { get; }
}
