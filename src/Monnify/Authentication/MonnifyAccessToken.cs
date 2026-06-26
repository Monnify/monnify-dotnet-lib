namespace Monnify.Authentication;

internal sealed class MonnifyAccessToken
{
    public MonnifyAccessToken(string token, DateTimeOffset expiresAtUtc)
    {
        Token = token;
        ExpiresAtUtc = expiresAtUtc;
    }

    public string Token { get; }

    public DateTimeOffset ExpiresAtUtc { get; }
}
