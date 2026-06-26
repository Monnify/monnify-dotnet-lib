namespace Monnify.IntegrationTests;

/// <summary>
/// Reads Monnify sandbox credentials from environment variables so these tests can run against
/// the real sandbox as ground truth (see CONTRIBUTING.md) without ever committing a secret.
/// Tests that need these should call <see cref="Xunit.Skip.IfNot"/> with <see cref="IsAvailable"/>
/// so CI (which has no sandbox creds) reports them as skipped, not failed.
/// </summary>
internal static class SandboxCredentials
{
    public static string? ApiKey { get; } = Environment.GetEnvironmentVariable("MONNIFY_SANDBOX_API_KEY");

    public static string? SecretKey { get; } = Environment.GetEnvironmentVariable("MONNIFY_SANDBOX_SECRET_KEY");

    public static bool IsAvailable => !string.IsNullOrWhiteSpace(ApiKey) && !string.IsNullOrWhiteSpace(SecretKey);
}
