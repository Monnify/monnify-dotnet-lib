namespace Monnify.Exceptions;

/// <summary>
/// Thrown when Monnify returns a well-formed response whose envelope reports
/// <c>requestSuccessful: false</c>, or whose HTTP status code indicates failure.
/// </summary>
public sealed class MonnifyApiException : MonnifyException
{
    public MonnifyApiException(string responseCode, string responseMessage, int httpStatusCode, string? rawResponseBody)
        : base($"Monnify API error {responseCode}: {responseMessage}")
    {
        ResponseCode = responseCode;
        ResponseMessage = responseMessage;
        HttpStatusCode = httpStatusCode;
        RawResponseBody = rawResponseBody;
    }

    /// <summary>Monnify's <c>responseCode</c> value, e.g. "99" or a named error code.</summary>
    public string ResponseCode { get; }

    /// <summary>Monnify's <c>responseMessage</c> value.</summary>
    public string ResponseMessage { get; }

    /// <summary>The HTTP status code of the underlying response.</summary>
    public int HttpStatusCode { get; }

    /// <summary>The raw response body, for diagnostics when you need more than the message/code.</summary>
    public string? RawResponseBody { get; }
}
