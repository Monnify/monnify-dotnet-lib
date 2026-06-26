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
        /// <summary>Confirmed against live sandbox 2026-06-26. POST, HTTP Basic auth (base64 apiKey:secretKey).</summary>
        public const string Login = "/api/v1/auth/login";
    }

    internal static class Banks
    {
        /// <summary>Confirmed against live sandbox 2026-06-26. GET, all banks (most without USSD info).</summary>
        public const string GetAll = "/api/v1/banks";

        /// <summary>Confirmed against live sandbox 2026-06-26. GET, banks with USSD templates populated.</summary>
        public const string GetUssdEnabled = "/api/v1/sdk/transactions/banks";
    }

    internal static class Verification
    {
        /// <summary>
        /// Confirmed against live sandbox 2026-06-26. GET with accountNumber/bankCode query params.
        /// Free on both sandbox and live, per Monnify's documentation.
        /// </summary>
        public const string ValidateAccountNumber = "/api/v1/disbursements/account/validate";

        // BVN/NIN verification endpoints are documented as live-only and cost real money against
        // the merchant's wallet balance per request, so they can't be confirmed via sandbox and
        // weren't probed against live without explicit authorization to spend. Not yet implemented
        // — see docs/COMPATIBILITY.md.
    }
}
