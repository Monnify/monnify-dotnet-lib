using Monnify.Http;

namespace Monnify.Cards;

internal sealed class MonnifyCardsClient : MonnifyHttpClientBase, IMonnifyCardsClient
{
    public MonnifyCardsClient(HttpClient httpClient) : base(httpClient) { }

    public Task<ChargeCardResult> ChargeAsync(ChargeCardRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Cards.Charge)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<ChargeCardResult>(httpRequest, cancellationToken);
    }

    public Task<AuthorizeCardOtpResult> AuthorizeOtpAsync(AuthorizeCardOtpRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Cards.AuthorizeOtp)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<AuthorizeCardOtpResult>(httpRequest, cancellationToken);
    }

    public Task<AuthorizeCardOtpResult> Authorize3dsAsync(Authorize3dsCardRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Cards.Authorize3ds)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<AuthorizeCardOtpResult>(httpRequest, cancellationToken);
    }
}
