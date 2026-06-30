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

        internal static class Cards
        {
            public const string Charge = "/api/v1/merchant/cards/charge";
            public const string AuthorizeOtp = "/api/v1/merchant/cards/otp/authorize";
            public const string Authorize3ds = "/api/v1/sdk/cards/secure-3d/authorize";
        }

        internal static class SubAccounts
        {
            public const string Base = "/api/v1/sub-accounts";
        }

        internal static class Refunds
        {
            public const string Initiate = "/api/v1/refunds/initiate-refund";
            public const string Base = "/api/v1/refunds";
        }

        internal static class LimitProfiles
        {
            public const string Base = "/api/v1/limit-profile/";
            public const string ReservedAccountLimit = "/api/v1/bank-transfer/reserved-accounts/limit";
        }

        internal static class Mandates
        {
            public const string Create = "/api/v1/direct-debit/mandate/create";
            public const string Base = "/api/v1/direct-debit/mandate/";
            public const string Debit = "/api/v1/direct-debit/mandate/debit";
            public const string DebitStatus = "/api/v1/direct-debit/mandate/debit-status";
            public const string Cancel = "/api/v1/direct-debit/mandate/cancel-mandate";

            /// <summary>Plural - a different base path than every other mandate endpoint above.</summary>
            public const string List = "/api/v1/direct-debit/mandates";
        }
    }

    internal static class Disbursements
    {
        internal static class Single
        {
            public const string Initiate = "/api/v2/disbursements/single";
            public const string ValidateOtp = "/api/v2/disbursements/single/validate-otp";
            public const string ResendOtp = "/api/v2/disbursements/single/resend-otp";
            public const string Summary = "/api/v2/disbursements/single/summary";
            public const string Transactions = "/api/v2/disbursements/single/transactions";
        }

        internal static class Bulk
        {
            public const string Initiate = "/api/v2/disbursements/batch";
            public const string ValidateOtp = "/api/v2/disbursements/batch/validate-otp";
            public const string ResendOtp = "/api/v2/disbursements/batch/resend-otp";
            public const string Summary = "/api/v2/disbursements/batch/summary";
            public const string TransactionsBase = "/api/v2/disbursements/bulk";
        }

        public const string SearchTransactions = "/api/v2/disbursements/search-transactions";
        public const string WalletBalance = "/api/v2/disbursements/wallet-balance";
    }

    internal static class Bills
    {
        public const string BillerCategories = "/api/v1/vas/bills-payment/biller-categories";
        public const string Billers = "/api/v1/vas/bills-payment/billers";
        public const string BillerProducts = "/api/v1/vas/bills-payment/biller-products";
        public const string Vend = "/api/v1/vas/bills-payment/vend";
        public const string Requery = "/api/v1/vas/bills-payment/requery";
        public const string ValidateCustomer = "/api/v1/vas/bills-payment/validate-customer";
    }
}
