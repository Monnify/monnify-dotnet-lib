namespace Monnify.Http;

/// <summary>
/// Named constants for every Monnify endpoint path the SDK calls, one per endpoint, each owning
/// its own version segment independently (Monnify versions endpoints per-category, not globally).
/// This is what keeps public client method signatures stable if Monnify bumps one endpoint's
/// internal version later — only the constant changes. See docs/COMPATIBILITY.md for provenance
/// and verification status of each path.
/// </summary>
internal static class MonnifyApiPaths
{
    internal static class Auth
    {
        public const string Login = "/api/v1/auth/login";
    }

    internal static class Banks
    {
        public const string GetAll = "/api/v1/banks";
        public const string GetUssdEnabled = "/api/v1/sdk/transactions/banks";
    }

    internal static class Verification
    {
        public const string ValidateAccountNumber = "/api/v1/disbursements/account/validate";
        public const string BvnDetailsMatch = "/api/v1/vas/bvn-details-match";
        public const string BvnAccountMatch = "/api/v1/vas/bvn-account-match";
        public const string NinDetails = "/api/v1/vas/nin-details";
    }

    internal static class Collections
    {
        internal static class Transactions
        {
            public const string Initialize = "/api/v1/merchant/transactions/init-transaction";
            public const string InitiateBankTransfer = "/api/v1/merchant/bank-transfer/init-payment";
            public const string Search = "/api/v1/transactions/search";
            public const string ByReference = "/api/v2/transactions";
            public const string Query = "/api/v2/merchant/transactions/query";
        }

        internal static class ReservedAccounts
        {
            public const string Base = "/api/v2/bank-transfer/reserved-accounts";
            public const string BaseV1 = "/api/v1/bank-transfer/reserved-accounts";
        }

        internal static class Invoices
        {
            public const string Create = "/api/v1/invoice/create";
            public const string Base = "/api/v1/invoice";
        }
    }
}
