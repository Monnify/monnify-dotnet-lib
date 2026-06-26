namespace Monnify.IntegrationTests;

/// <summary>
/// The merchant contract code needed by Collections operations (checkout, reserved accounts,
/// invoices). Distinct from the API key/secret since it's a non-secret merchant identifier.
/// </summary>
internal static class SandboxContractCode
{
    public static string? Value { get; } = Environment.GetEnvironmentVariable("MONNIFY_SANDBOX_CONTRACT_CODE");

    public static bool IsAvailable => !string.IsNullOrWhiteSpace(Value);
}
