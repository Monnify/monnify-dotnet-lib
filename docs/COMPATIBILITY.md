# Endpoint Compatibility Ledger

Tracks each SDK method against the Monnify endpoint it calls, for maintainers
adding or changing a client (see [CONTRIBUTING.md](../CONTRIBUTING.md)).

Status legend:
- **Implemented** — shipped, with the path/payload sourced from a live
  sandbox call or Monnify's official API reference.
- **Planned** — designed in the implementation plan, not yet built.

| SDK Method | Monnify Endpoint | Status | Notes |
|---|---|---|---|
| `MonnifyTokenProvider.LoginAsync` | `POST /api/v1/auth/login` | **Implemented** | Basic auth (`base64(apiKey:secretKey)`); envelope `{requestSuccessful, responseMessage, responseCode, responseBody:{accessToken, expiresIn}}` |
| `IMonnifyCollectionsClient.InitializeTransactionAsync` | `POST /api/v1/merchant/transactions/init-transaction` | **Implemented** | Returns a hosted `checkoutUrl`; `contractCode` is required |
| `IMonnifyCollectionsClient.CreateReservedAccountAsync` | `POST /api/v2/bank-transfer/reserved-accounts` | **Implemented** | `getAllAvailableBanks` is required (bool); if `false`, `preferredBanks` must be supplied |
| `IMonnifyCollectionsClient.GetReservedAccountAsync` | `GET /api/v2/bank-transfer/reserved-accounts/{accountReference}` | **Implemented** | |
| `IMonnifyCollectionsClient.GetReservedAccountTransactionsAsync` | `GET /api/v1/bank-transfer/reserved-accounts/transactions?accountReference=&page=&size=` | **Implemented** | Per-transaction item fields modeled from general Monnify conventions; no real example with content was observed (test account had zero transactions) |
| `IMonnifyCollectionsClient.DeleteReservedAccountAsync` | `DELETE /api/v1/bank-transfer/reserved-accounts/reference/{accountReference}` | **Implemented** | Note the `/reference/` path segment — easy to miss |
| `IMonnifyCollectionsClient.CreateInvoiceAsync` | `POST /api/v1/invoice/create` | **Implemented** | Sandbox rejects `expiryDate` too far in the future |
| `IMonnifyCollectionsClient.GetInvoiceAsync` | `GET /api/v1/invoice/{invoiceReference}/details` | **Implemented** | |
| `IMonnifyCollectionsClient.GetInvoicesAsync` | `GET /api/v1/invoice/all?page=&size=` | **Implemented** | |
| `IMonnifyCollectionsClient.CancelInvoiceAsync` | `DELETE /api/v1/invoice/{invoiceReference}/cancel` | **Implemented** | |
| `IMonnifyCollectionsClient.InitiateBankTransferAsync` | `POST /api/v1/merchant/bank-transfer/init-payment` | **Implemented** | Generates a one-time dynamic account for an already-initialized transaction |
| `IMonnifyCollectionsClient.SearchTransactionsAsync` | `GET /api/v1/transactions/search?page=&size=&...` | **Implemented** | Many optional filters (reference, amount range, customer, status, date range) |
| `IMonnifyCollectionsClient.GetTransactionAsync` | `GET /api/v2/transactions/{transactionReference}` | **Implemented** | `amountPaid`/`totalPayable`/`settlementAmount` are quoted strings in this response; deserialized into `decimal` via `JsonNumberHandling.AllowReadingFromString` |
| `IMonnifyCollectionsClient.QueryTransactionAsync` | `GET /api/v2/merchant/transactions/query?transactionReference=&paymentReference=` | **Implemented** | Same response shape as `GetTransactionAsync`; requires at least one of the two query parameters |
| *(Collections — sub-accounts/splitting)* | TBD | Planned | Phase 3 follow-up |
| *(Collections — direct debit/mandates)* | TBD | Planned | Phase 3 follow-up |
| *(Collections — card tokenization)* | TBD | Planned | Phase 3 follow-up |
| *(Collections — Payment Links)* | N/A | Out of scope | Dashboard-only feature, no API |
| *(Disbursements — single transfer)* | `POST /api/v2/disbursements/single` | Planned | Phase 4 |
| *(Disbursements — bulk transfer)* | `POST /api/v2/disbursements/batch` | Planned | Phase 4 |
| *(Disbursements — wallet balance)* | `GET /api/v2/disbursements/wallet-balance` | Planned | Phase 4 |
| `IMonnifyVerificationClient.ValidateAccountNumberAsync` | `GET /api/v1/disbursements/account/validate?accountNumber=&bankCode=` | **Implemented** | Free on both sandbox and live; `responseBody: {accountNumber, accountName, bankCode, currencyCode}` |
| `IMonnifyVerificationClient.MatchBvnDetailsAsync` | `POST /api/v1/vas/bvn-details-match` | **Implemented** | Bills the merchant wallet per request; response shape mixes a structured match object (`name`) with plain match-status strings (`dateOfBirth`, `mobileNo`) |
| `IMonnifyVerificationClient.MatchBvnToAccountAsync` | `POST /api/v1/vas/bvn-account-match` | **Implemented** | Bills the merchant wallet per request |
| `IMonnifyVerificationClient.VerifyNinAsync` | `POST /api/v1/vas/nin-details` | **Implemented** | Live environment only; bills the merchant wallet per request |
| `IMonnifyBanksClient.GetBanksAsync` | `GET /api/v1/banks` | **Implemented** | `responseBody` is a JSON array of `{name, code, nipBankCode, ussdTemplate, baseUssdCode, transferUssdTemplate}`; most entries have null USSD fields |
| `IMonnifyBanksClient.GetUssdEnabledBanksAsync` | `GET /api/v1/sdk/transactions/banks` | **Implemented** | Subset of banks with USSD info; not every entry on this list has a populated `ussdTemplate` (e.g. Suntrust Bank) |
| *(Webhooks — signature)* | `monnify-signature` header, `SHA-512(secretKey + rawBody)` | Planned | Phase 5 |
| *(Bills payment)* | TBD | Planned | Phase 6 |

Update this table whenever a method moves from Planned to Implemented, or its
endpoint changes.
