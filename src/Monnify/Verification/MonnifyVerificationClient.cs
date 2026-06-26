using Monnify.Http;

namespace Monnify.Verification;

internal sealed class MonnifyVerificationClient : MonnifyHttpClientBase, IMonnifyVerificationClient
{
    public MonnifyVerificationClient(HttpClient httpClient) : base(httpClient) { }

    public Task<AccountNumberValidationResult> ValidateAccountNumberAsync(
        string accountNumber, string bankCode, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            throw new ArgumentException("Account number must be provided.", nameof(accountNumber));
        }

        if (string.IsNullOrWhiteSpace(bankCode))
        {
            throw new ArgumentException("Bank code must be provided.", nameof(bankCode));
        }

        var path = $"{MonnifyApiPaths.Verification.ValidateAccountNumber}" +
                    $"?accountNumber={Uri.EscapeDataString(accountNumber)}&bankCode={Uri.EscapeDataString(bankCode)}";
        var request = new HttpRequestMessage(HttpMethod.Get, path);
        return SendAsync<AccountNumberValidationResult>(request, cancellationToken);
    }

    public Task<BvnDetailsMatchResult> MatchBvnDetailsAsync(BvnDetailsMatchRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Verification.BvnDetailsMatch)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<BvnDetailsMatchResult>(httpRequest, cancellationToken);
    }

    public Task<BvnAccountMatchResult> MatchBvnToAccountAsync(BvnAccountMatchRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Verification.BvnAccountMatch)
        {
            Content = CreateJsonContent(request),
        };
        return SendAsync<BvnAccountMatchResult>(httpRequest, cancellationToken);
    }

    public Task<NinVerificationResult> VerifyNinAsync(string nin, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nin))
        {
            throw new ArgumentException("NIN must be provided.", nameof(nin));
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Verification.NinDetails)
        {
            Content = CreateJsonContent(new { nin }),
        };
        return SendAsync<NinVerificationResult>(httpRequest, cancellationToken);
    }
}
