# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/).

Each entry that introduces or changes an API call should cross-reference the
relevant row in [docs/COMPATIBILITY.md](docs/COMPATIBILITY.md), since the SDK's
own version is independent of Monnify's API versioning.

## [Unreleased]

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
