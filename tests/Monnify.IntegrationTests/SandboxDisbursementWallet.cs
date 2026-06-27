namespace Monnify.IntegrationTests;

/// <summary>The Monnify wallet account number Disbursements operations send from. Not a secret.</summary>
internal static class SandboxDisbursementWallet
{
    public static string? Value { get; } = Environment.GetEnvironmentVariable("MONNIFY_SANDBOX_DISBURSEMENT_WALLET");

    public static bool IsAvailable => !string.IsNullOrWhiteSpace(Value);
}
