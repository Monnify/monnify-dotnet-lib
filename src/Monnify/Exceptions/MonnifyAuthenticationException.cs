namespace Monnify.Exceptions;

/// <summary>
/// Thrown when Monnify rejects a login attempt or the login response can't be parsed.
/// The message never includes the configured API key or secret key.
/// </summary>
public sealed class MonnifyAuthenticationException : MonnifyException
{
    public MonnifyAuthenticationException(string message) : base(message) { }

    public MonnifyAuthenticationException(string message, Exception innerException) : base(message, innerException) { }
}
