namespace Monnify.Http;

/// <summary>
/// Named constants for every Monnify endpoint path the SDK calls, one per endpoint, each owning
/// its own version segment independently (Monnify versions endpoints per-category, not globally).
/// This is what keeps public client method signatures stable if Monnify bumps one endpoint's
/// internal version later — only the constant changes. See docs/COMPATIBILITY.md for which of
/// these are sandbox-confirmed versus inferred from documentation.
/// </summary>
internal static class MonnifyApiPaths
{
    internal static class Auth
    {
        /// <summary>Confirmed. POST, HTTP Basic auth (base64 apiKey:secretKey).</summary>
        public const string Login = "/api/v1/auth/login";
    }
}
