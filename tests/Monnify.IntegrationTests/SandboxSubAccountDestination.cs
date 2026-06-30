namespace Monnify.IntegrationTests;

/// <summary>
/// A real bank account to use as the settlement destination in sub-account integration tests.
/// A made-up account is rejected by Monnify before the sub-account is even created, so this
/// must be a genuine account - not a secret, just specific to whoever is running these tests.
/// </summary>
internal static class SandboxSubAccountDestination
{
    public static string? BankCode { get; } = Environment.GetEnvironmentVariable("MONNIFY_SANDBOX_SUBACCOUNT_BANK_CODE");

    public static string? AccountNumber { get; } = Environment.GetEnvironmentVariable("MONNIFY_SANDBOX_SUBACCOUNT_ACCOUNT");

    public static bool IsAvailable => !string.IsNullOrWhiteSpace(BankCode) && !string.IsNullOrWhiteSpace(AccountNumber);
}
