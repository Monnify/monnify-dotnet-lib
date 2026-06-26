namespace Monnify.Authentication;

internal interface IMonnifyTokenProvider
{
    /// <summary>Returns a cached, non-expired access token, logging in (or refreshing) if needed.</summary>
    ValueTask<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>Forces the next call to <see cref="GetAccessTokenAsync"/> to log in again.</summary>
    void Invalidate();
}
