# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/).

Each entry that introduces or changes an API call should cross-reference the
relevant row in [docs/COMPATIBILITY.md](docs/COMPATIBILITY.md), since the SDK's
own version is independent of Monnify's API versioning.

## [Unreleased]

### Fixed
- Error responses that don't match Monnify's standard envelope (a gateway-level rejection, an
  HTTP error page, or any 401/403 returned because a feature like Disbursements isn't activated
  for the merchant yet) now surface a clear fallback message instead of an empty
  `responseCode`/`responseMessage`. The envelope's string fields were defaulting to `""` rather
  than `null`, which silently defeated the fallback logic in both `MonnifyHttpClientBase` and
  `MonnifyTokenProvider`.

### Added
- Initial repository scaffolding: solution layout, Central Package Management,
  shared build properties, CI/CD workflow skeletons.
- Core HTTP and authentication infrastructure: `AddMonnify(...)` DI registration,
  `MonnifyClientOptions` (environment/credentials/HTTPS validation), a singleton
  token provider that caches and proactively refreshes Monnify's bearer token
  (with reactive 401 retry-once), `MonnifyHttpClientBase` for envelope unwrapping,
  and the `MonnifyApiException` / `MonnifyAuthenticationException` /
  `MonnifyDeserializationException` exception model. Built-in HTTP resilience
  (`Microsoft.Extensions.Http.Resilience`) is net8.0-only — its dependency chain
  doesn't ship real netstandard2.0 binaries despite nuspec metadata claiming
  support, so netstandard2.0 consumers get plain typed clients without it.
- `IMonnifyBanksClient` (`GetBanksAsync`, `GetUssdEnabledBanksAsync`) and
  `IMonnifyVerificationClient` (`ValidateAccountNumberAsync` for account name
  enquiry, `MatchBvnDetailsAsync`, `MatchBvnToAccountAsync`, `VerifyNinAsync`),
  plus the `MonnifyClient` facade. `VerifyNinAsync` is available only in
  Monnify's Live environment; the BVN/NIN methods bill the merchant's wallet
  per call.
- Sandbox integration test infrastructure (`tests/Monnify.IntegrationTests`):
  reads credentials from environment variables, uses `Xunit.SkippableFact` so
  CI (no sandbox access) reports skips rather than failures.
- `IMonnifyCollectionsClient`: `InitializeTransactionAsync` (checkout), reserved
  accounts (`CreateReservedAccountAsync`, `GetReservedAccountAsync`,
  `GetReservedAccountTransactionsAsync`, `DeleteReservedAccountAsync`), and
  invoices (`CreateInvoiceAsync`, `GetInvoiceAsync`, `GetInvoicesAsync`,
  `CancelInvoiceAsync`). Payment Links are dashboard-only and have no API, so
  they're out of scope. Sub-accounts/splitting, direct debit, and card
  tokenization are deferred to a follow-up.
- `IMonnifyCollectionsClient`: `InitiateBankTransferAsync` (one-time dynamic
  account for an initialized transaction), `SearchTransactionsAsync` (filtered,
  paged transaction search), `GetTransactionAsync` and `QueryTransactionAsync`
  (transaction status by Monnify reference or merchant payment reference).
- `IMonnifyDisbursementsClient`: single transfers (`InitiateSingleTransferAsync`,
  `AuthorizeSingleTransferAsync`, `ResendSingleTransferOtpAsync`,
  `GetSingleTransferAsync`, `GetSingleTransfersAsync`), bulk transfers
  (`InitiateBulkTransferAsync`, `AuthorizeBulkTransferAsync`,
  `ResendBulkTransferOtpAsync`, `GetBulkTransferSummaryAsync`,
  `GetBulkTransferTransactionsAsync`, `GetBulkTransfersAsync`), plus
  `SearchTransactionsAsync` and `GetWalletBalanceAsync`.
  `GetBulkTransfersAsync` calls `GET /api/v2/disbursements/bulk` — Monnify's
  docs show this path with a `/transactions` suffix, which 404s in the
  sandbox. The Transfer feature requires Monnify to activate it
  for the merchant first (contact sales@monnify.com). Registered with automatic
  retry disabled: an ambiguous failure on a transfer-initiating call must be
  resolved by querying status with the same reference, not by resending the
  request, to avoid a double disbursement. `AddMonnifyDefaults` now takes an
  `allowAutomaticRetry` flag to support this per-client.
- `MonnifyWebhookValidator.IsValid` / `ComputeSignature`: verifies the
  `monnify-signature` header as `HMAC-SHA512(key: secretKey, message:
  rawRequestBody)`, comparing case-insensitively in constant time. Verified
  against Monnify's own documented sample secret/body/hash. Note: Monnify's
  webhooks docs describe the scheme as a plain `SHA-512(secretKey + body)`
  hash (not HMAC) and their JS sample pretty-prints the body before hashing —
  neither reproduces the "Hashed Value" published on the same docs page; only
  hashing the *compact* JSON form with HMAC-SHA512 does, which is what their
  own Java sample does and what this validator implements.
