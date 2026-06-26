using Monnify.Http;

namespace Monnify.Banks;

internal sealed class MonnifyBanksClient : MonnifyHttpClientBase, IMonnifyBanksClient
{
    public MonnifyBanksClient(HttpClient httpClient) : base(httpClient) { }

    public Task<IReadOnlyList<Bank>> GetBanksAsync(CancellationToken cancellationToken = default) =>
        SendAsync<IReadOnlyList<Bank>>(new HttpRequestMessage(HttpMethod.Get, MonnifyApiPaths.Banks.GetAll), cancellationToken);

    public Task<IReadOnlyList<Bank>> GetUssdEnabledBanksAsync(CancellationToken cancellationToken = default) =>
        SendAsync<IReadOnlyList<Bank>>(new HttpRequestMessage(HttpMethod.Get, MonnifyApiPaths.Banks.GetUssdEnabled), cancellationToken);
}
