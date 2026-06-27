namespace Monnify.IntegrationTests;

/// <summary>
/// A real, name-enquiry-validatable bank account to use as the destination in Disbursements
/// integration tests. A made-up account number gets rejected by Monnify's own validation before
/// a transfer is even created, so this needs to be a genuine account - not a secret, just specific
/// to whoever is running these tests locally.
/// </summary>
internal static class SandboxDisbursementDestination
{
    public static string? BankCode { get; } = Environment.GetEnvironmentVariable("MONNIFY_SANDBOX_DISBURSEMENT_DESTINATION_BANK_CODE");

    public static string? AccountNumber { get; } = Environment.GetEnvironmentVariable("MONNIFY_SANDBOX_DISBURSEMENT_DESTINATION_ACCOUNT");

    public static bool IsAvailable => !string.IsNullOrWhiteSpace(BankCode) && !string.IsNullOrWhiteSpace(AccountNumber);
}
