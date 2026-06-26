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
}
