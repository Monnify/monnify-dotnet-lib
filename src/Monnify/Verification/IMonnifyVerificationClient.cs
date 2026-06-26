namespace Monnify.Verification;

public interface IMonnifyVerificationClient
{
    /// <summary>
    /// Resolves the account name for a bank account number ("name enquiry"). Free on both
    /// sandbox and live environments, per Monnify's documentation.
    /// </summary>
    /// <param name="accountNumber">The 10-digit NUBAN account number.</param>
    /// <param name="bankCode">The bank's Monnify bank code (see <c>IMonnifyBanksClient</c>).</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    Task<AccountNumberValidationResult> ValidateAccountNumberAsync(
        string accountNumber, string bankCode, CancellationToken cancellationToken = default);
}
