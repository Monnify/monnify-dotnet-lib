namespace Monnify.Verification;

public interface IMonnifyVerificationClient
{
    /// <summary>
    /// Resolves the account name for a bank account number ("name enquiry"). Free on both
    /// sandbox and live environments, per our documentation.
    /// </summary>
    /// <param name="accountNumber">The 10-digit NUBAN account number.</param>
    /// <param name="bankCode">The bank's Monnify bank code (see <c>IMonnifyBanksClient</c>).</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    Task<AccountNumberValidationResult> ValidateAccountNumberAsync(
        string accountNumber, string bankCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a person's claimed name, date of birth, and mobile number match BVN records.
    /// <b>Bills the merchant's wallet per request</b> — only call this when you actually need a
    /// verification, not speculatively.
    /// </summary>
    Task<BvnDetailsMatchResult> MatchBvnDetailsAsync(BvnDetailsMatchRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a BVN matches the owner of a given bank account. <b>Bills the merchant's
    /// wallet per request.</b>
    /// </summary>
    Task<BvnAccountMatchResult> MatchBvnToAccountAsync(BvnAccountMatchRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies a National Identification Number (NIN). Only available in Monnify's Live
    /// environment, and <b>bills the merchant's wallet per request</b>.
    /// </summary>
    Task<NinVerificationResult> VerifyNinAsync(string nin, CancellationToken cancellationToken = default);
}
