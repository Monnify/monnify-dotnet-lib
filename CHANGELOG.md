# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/).

Each entry that introduces or changes an API call should cross-reference the
relevant row in [docs/COMPATIBILITY.md](docs/COMPATIBILITY.md), since the SDK's
own version is independent of Monnify's API versioning.

## [0.2.0](https://github.com/Monnify/monnify-dotnet-lib/compare/v0.1.0...v0.2.0) (2026-06-30)


### Features

* add card transactions and automated release tagging ([25a62b3](https://github.com/Monnify/monnify-dotnet-lib/commit/25a62b30c36c7468faad960f48e5872eb9264d8d))
* add card transactions client (charge, OTP, 3DS authorize) ([73cc17e](https://github.com/Monnify/monnify-dotnet-lib/commit/73cc17ea37382099bbefa50423ed48856ac9e8dc))

## [Unreleased]

### Added
- `IMonnifyDisbursementsClient`: customer wallets - `CreateWalletAsync`, `GetWalletsAsync`,
  `GetCustomerWalletBalanceAsync`, `GetWalletTransactionsAsync`. Note: the balance endpoint uses
  `accountNumber` (not `walletReference` as our docs show).
- `IMonnifyCollectionsClient`: refunds - `InitiateRefundAsync`, `GetRefundAsync`, `GetRefundsAsync`.
- `IMonnifyCollectionsClient`: limit profiles - `CreateLimitProfileAsync`, `GetLimitProfilesAsync`,
  `UpdateLimitProfileAsync`; and `CreateReservedAccountWithLimitAsync`,
  `UpdateReservedAccountLimitAsync` for attaching limit profiles to reserved accounts.
- `IMonnifyCollectionsClient`: sub-accounts - `CreateSubAccountsAsync`, `GetSubAccountsAsync`,
  `UpdateSubAccountAsync`, `DeleteSubAccountAsync`. Create takes an array body (not a single object)
  and the delete endpoint returns no `responseBody`. Requires relationship-manager approval for live.
- `IMonnifyCollectionsClient`: direct debit mandates - `CreateMandateAsync`, `GetMandatesAsync`,
  `DebitMandateAsync`, `GetMandateDebitStatusAsync`, `CancelMandateAsync`, `ListMandatesAsync`.
  Sandbox testing surfaced several real discrepancies with our own docs (wrong field name for the
  merchant reference, undocumented fields, an extra `mandateStatus` value, a paging shape missing
  fields our sample shows) - see docs/COMPATIBILITY.md for each. Also added
  `LenientStringJsonConverter` for `GetMandateDebitStatusAsync`'s `responseMessage`, which our docs
  sample shows as an empty object instead of a string there.

## [0.1.0] - 2026-06-29

### Fixed
- Error responses that don't match Monnify's standard envelope (a gateway-level rejection, an
  HTTP error page, or any 401/403 returned because a feature like Disbursements isn't activated
  for the merchant yet) now surface a clear fallback message instead of an empty
  `responseCode`/`responseMessage`. The envelope's string fields were defaulting to `""` rather
  than `null`, which silently defeated the fallback logic in both `MonnifyHttpClientBase` and
  `MonnifyTokenProvider`.
- `SECURITY.md` still described the webhook signature scheme as plain
  `SHA-512(secretKey + rawBody)` from before that was corrected to
  `HMAC-SHA512` earlier in this project - updated to match what
  `MonnifyWebhookValidator` actually implements, plus a note that sandbox
  sends no signature header at all.

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
  own Java sample does and what this validator implements. Monnify's sandbox
  sends no `monnify-signature` header at all, so `IsValid` always returns
  `false` for sandbox webhook traffic by design — there is intentionally no
  environment-aware bypass for this check.
- `MonnifyWebhookEnvelope` and `MonnifyWebhookParser` (`Parse`,
  `ParseEventData<T>`, `ParseMetaData<T>`), plus typed event-data classes for
  all 12 documented webhook event types: `CollectionTransactionEventData`
  (covers both a regular collection payment and an offline/agent payment),
  `DisbursementStatusEventData` (shared by the successful/failed/reversed
  disbursement events), `RefundEventData` (shared by the successful/failed
  refund events), `SettlementEventData`, `MandateStatusEventData`,
  `WalletActivityEventData`, `LowBalanceAlertEventData`, and
  `RejectedPaymentEventData`. Modeled directly from Monnify's own documented
  sample payloads, which surfaced several real shape inconsistencies handled
  defensively rather than assumed away: `paymentSourceInformation` arrives as
  an array, an empty object, or a single populated object depending on the
  event variant (`SingleOrArrayJsonConverter`); several amount fields are
  sometimes quoted strings and sometimes numbers; the rejected-payment event
  uses `created_on` where every other event uses `createdOn`; the wallet
  activity event nests `metaData` as a sibling of `eventData` at the envelope
  root rather than inside it.
- `HttpRequest.ValidateMonnifyWebhookAsync(secretKey, ct)`: an ASP.NET Core
  convenience that reads the request body once, verifies its signature, and
  returns a result exposing `IsValid`, `RawBody`, and `GetEnvelope()`.
- `IMonnifyBillsClient` (Bills payment - airtime, data, cable TV, electricity,
  etc.): `GetBillerCategoriesAsync`, `GetBillersAsync`, `GetBillerProductsAsync`,
  `ValidateCustomerAsync`, `VendAsync`, `RequeryAsync`. Sandbox-verified, which
  caught three real discrepancies with Monnify's own published docs:
  pagination is 0-based (`page` defaults to `0`), not the 1-based default the
  docs describe; `GetBillerProductsAsync` returns singular `category`/`biller`
  objects on each product, not the `categories`/`billers` arrays the docs
  show; and `ValidateCustomerAsync`'s `validationReference` (needed by
  `VendAsync` for certain products, e.g. electricity) lives nested inside
  `vendInstruction`, not at the top level — the docs' only sample happened to
  be the case that doesn't need a reference at all, so it never revealed
  where one would go. Registered with automatic retry disabled, same
  reasoning as Disbursements: `VendAsync` moves money, so an ambiguous
  failure must be resolved via `RequeryAsync` with the same reference, not
  retried blindly. `MonnifyPagedResult<T>` gained two new nullable fields
  (`IsEmpty`, `NextPage`) to capture the pagination shape Bills uses.
- `samples/Monnify.Samples.ConsoleApp`: DI registration via the generic host
  outside ASP.NET Core, listing banks and initializing a checkout. Verified
  end-to-end against the real sandbox.
- `samples/Monnify.Samples.WebApi`: a minimal API webhook receiver
  demonstrating signature verification, envelope parsing, and dispatch by
  event type, responding immediately per our own best-practice guidance.
  Verified end-to-end (signature rejection and a correctly-signed,
  correctly-dispatched webhook) against a running instance.
- README quickstart section covering installation, service registration,
  initiating a checkout, and receiving webhooks, linking to both samples.
- CI/CD: `ci.yml` (restore, `dotnet format --verify-no-changes`, build, test,
  pack-validation-only on every PR and push to `main`), `codeql.yml` (weekly +
  on push/PR), and `dependabot.yml` (weekly NuGet and GitHub Actions update
  checks). `release.yml` triggers on a `vX.Y.Z` tag, builds/tests/packs, then
  requires manual approval via a `nuget-release` GitHub Environment before
  publishing to NuGet.org and creating a GitHub Release - nothing publishes
  automatically. Versioning is computed by the already-present
  Nerdbank.GitVersioning setup from `version.json`, confirmed locally to
  produce a sensible prerelease version per commit.
- Relaxed `dotnet_style_require_accessibility_modifiers` from `always` to
  `for_non_interface_members` in `.editorconfig` - every interface in this
  codebase has consistently omitted the (redundant, since interface members
  are implicitly public) `public` modifier, so the rule was rewritten to
  match that actual, consistent convention rather than mechanically adding
  the modifier across every interface file.
- `Monnify.csproj` now packs the root `README.md` into the NuGet package
  (`PackageReadmeFile`), fixing a "missing a readme" warning from `dotnet pack`.
- Added `.github/ISSUE_TEMPLATE/` (bug report, feature request, and a
  `config.yml` redirecting security reports to private vulnerability
  reporting) and `.github/PULL_REQUEST_TEMPLATE.md` matching the checklist
  already documented in `CONTRIBUTING.md`.
