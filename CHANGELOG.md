# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/).

Each entry that introduces or changes an API call should cross-reference the
relevant row in [docs/COMPATIBILITY.md](docs/COMPATIBILITY.md), since the SDK's
own version is independent of Monnify's API versioning.

## [0.11.0](https://github.com/Monnify/monnify-dotnet-lib/compare/v0.10.0...v0.11.0) (2026-07-02)


### Features

* add card transactions and automated release tagging ([25a62b3](https://github.com/Monnify/monnify-dotnet-lib/commit/25a62b30c36c7468faad960f48e5872eb9264d8d))
* add card transactions client (charge, OTP, 3DS authorize) ([73cc17e](https://github.com/Monnify/monnify-dotnet-lib/commit/73cc17ea37382099bbefa50423ed48856ac9e8dc))
* add direct debit mandates client ([d8f772d](https://github.com/Monnify/monnify-dotnet-lib/commit/d8f772d03d17b6463abe86ebd7adbde6db48f411))
* add direct debit mandates client ([456d369](https://github.com/Monnify/monnify-dotnet-lib/commit/456d3697fd32f61d9f1fc76f10c58f41b079c352))
* Add limit profiles and sub-account management to IMonnifyCollectionsClient ([e13af00](https://github.com/Monnify/monnify-dotnet-lib/commit/e13af001ee07b831277cb8a12beb2b594a7f4866))
* **collections:** add paycode API support ([a2191b0](https://github.com/Monnify/monnify-dotnet-lib/commit/a2191b0d863e7dad3422d7cdf8e01796920cf7b5))
* customer wallets, paycodes, and envelope fix ([cafe7d2](https://github.com/Monnify/monnify-dotnet-lib/commit/cafe7d2324cc00279733010e9a9a61278d7bed00))
* **disbursements:** add customer wallet API support ([e2bc8d2](https://github.com/Monnify/monnify-dotnet-lib/commit/e2bc8d26f9e4f5d0bad40b62b8cf6498ae6d4355))
* Implement refund management in IMonnifyCollectionsClient with new endpoints and models ([807beee](https://github.com/Monnify/monnify-dotnet-lib/commit/807beeeb8aba3969debdd4afb9fa42c81aa43625))
* sub-accounts, limit profiles, and refunds ([338c5de](https://github.com/Monnify/monnify-dotnet-lib/commit/338c5de8e7823a6b25c6f4885a2068e6a468abe8))


### Bug Fixes

* add nbgv cloud step before build to prevent invalid GITHUB_ENV format error ([253ad61](https://github.com/Monnify/monnify-dotnet-lib/commit/253ad610c2ec3a28745cc9fdafdcf06f8149b8bb))
* improve error handling for non-JSON responses and update tests a… ([1cf31cf](https://github.com/Monnify/monnify-dotnet-lib/commit/1cf31cf6626784b8ca4d9ef5b0bdfd224a6dfa47))
* improve error handling for non-JSON responses and update tests accordingly ([c9b0c40](https://github.com/Monnify/monnify-dotnet-lib/commit/c9b0c4086cbd4447d51ad4b27dafb35494d7f160))
* skip-github-release silently skipped tag creation too ([6fc0e40](https://github.com/Monnify/monnify-dotnet-lib/commit/6fc0e402bdfc8b27a09841a6a4b41ff151dee83d))
* update action-gh-release to version 3 for publishing GitHub releases ([427c440](https://github.com/Monnify/monnify-dotnet-lib/commit/427c440d096c54f3fc61cb8b77e23b5875d4730c))

## [0.10.0](https://github.com/Monnify/monnify-dotnet-lib/compare/v0.9.0...v0.10.0) (2026-07-02)


### Features

* add card transactions and automated release tagging ([25a62b3](https://github.com/Monnify/monnify-dotnet-lib/commit/25a62b30c36c7468faad960f48e5872eb9264d8d))
* add card transactions client (charge, OTP, 3DS authorize) ([73cc17e](https://github.com/Monnify/monnify-dotnet-lib/commit/73cc17ea37382099bbefa50423ed48856ac9e8dc))
* add direct debit mandates client ([d8f772d](https://github.com/Monnify/monnify-dotnet-lib/commit/d8f772d03d17b6463abe86ebd7adbde6db48f411))
* add direct debit mandates client ([456d369](https://github.com/Monnify/monnify-dotnet-lib/commit/456d3697fd32f61d9f1fc76f10c58f41b079c352))
* Add limit profiles and sub-account management to IMonnifyCollectionsClient ([e13af00](https://github.com/Monnify/monnify-dotnet-lib/commit/e13af001ee07b831277cb8a12beb2b594a7f4866))
* **collections:** add paycode API support ([a2191b0](https://github.com/Monnify/monnify-dotnet-lib/commit/a2191b0d863e7dad3422d7cdf8e01796920cf7b5))
* customer wallets, paycodes, and envelope fix ([cafe7d2](https://github.com/Monnify/monnify-dotnet-lib/commit/cafe7d2324cc00279733010e9a9a61278d7bed00))
* **disbursements:** add customer wallet API support ([e2bc8d2](https://github.com/Monnify/monnify-dotnet-lib/commit/e2bc8d26f9e4f5d0bad40b62b8cf6498ae6d4355))
* Implement refund management in IMonnifyCollectionsClient with new endpoints and models ([807beee](https://github.com/Monnify/monnify-dotnet-lib/commit/807beeeb8aba3969debdd4afb9fa42c81aa43625))
* sub-accounts, limit profiles, and refunds ([338c5de](https://github.com/Monnify/monnify-dotnet-lib/commit/338c5de8e7823a6b25c6f4885a2068e6a468abe8))


### Bug Fixes

* add nbgv cloud step before build to prevent invalid GITHUB_ENV format error ([253ad61](https://github.com/Monnify/monnify-dotnet-lib/commit/253ad610c2ec3a28745cc9fdafdcf06f8149b8bb))
* improve error handling for non-JSON responses and update tests a… ([1cf31cf](https://github.com/Monnify/monnify-dotnet-lib/commit/1cf31cf6626784b8ca4d9ef5b0bdfd224a6dfa47))
* improve error handling for non-JSON responses and update tests accordingly ([c9b0c40](https://github.com/Monnify/monnify-dotnet-lib/commit/c9b0c4086cbd4447d51ad4b27dafb35494d7f160))
* skip-github-release silently skipped tag creation too ([6fc0e40](https://github.com/Monnify/monnify-dotnet-lib/commit/6fc0e402bdfc8b27a09841a6a4b41ff151dee83d))
* update action-gh-release to version 3 for publishing GitHub releases ([427c440](https://github.com/Monnify/monnify-dotnet-lib/commit/427c440d096c54f3fc61cb8b77e23b5875d4730c))

## [0.9.0](https://github.com/Monnify/monnify-dotnet-lib/compare/v0.8.0...v0.9.0) (2026-07-01)


### Features

* add card transactions and automated release tagging ([25a62b3](https://github.com/Monnify/monnify-dotnet-lib/commit/25a62b30c36c7468faad960f48e5872eb9264d8d))
* add card transactions client (charge, OTP, 3DS authorize) ([73cc17e](https://github.com/Monnify/monnify-dotnet-lib/commit/73cc17ea37382099bbefa50423ed48856ac9e8dc))
* add direct debit mandates client ([d8f772d](https://github.com/Monnify/monnify-dotnet-lib/commit/d8f772d03d17b6463abe86ebd7adbde6db48f411))
* add direct debit mandates client ([456d369](https://github.com/Monnify/monnify-dotnet-lib/commit/456d3697fd32f61d9f1fc76f10c58f41b079c352))
* Add limit profiles and sub-account management to IMonnifyCollectionsClient ([e13af00](https://github.com/Monnify/monnify-dotnet-lib/commit/e13af001ee07b831277cb8a12beb2b594a7f4866))
* **collections:** add paycode API support ([a2191b0](https://github.com/Monnify/monnify-dotnet-lib/commit/a2191b0d863e7dad3422d7cdf8e01796920cf7b5))
* customer wallets, paycodes, and envelope fix ([cafe7d2](https://github.com/Monnify/monnify-dotnet-lib/commit/cafe7d2324cc00279733010e9a9a61278d7bed00))
* **disbursements:** add customer wallet API support ([e2bc8d2](https://github.com/Monnify/monnify-dotnet-lib/commit/e2bc8d26f9e4f5d0bad40b62b8cf6498ae6d4355))
* Implement refund management in IMonnifyCollectionsClient with new endpoints and models ([807beee](https://github.com/Monnify/monnify-dotnet-lib/commit/807beeeb8aba3969debdd4afb9fa42c81aa43625))
* sub-accounts, limit profiles, and refunds ([338c5de](https://github.com/Monnify/monnify-dotnet-lib/commit/338c5de8e7823a6b25c6f4885a2068e6a468abe8))


### Bug Fixes

* add nbgv cloud step before build to prevent invalid GITHUB_ENV format error ([253ad61](https://github.com/Monnify/monnify-dotnet-lib/commit/253ad610c2ec3a28745cc9fdafdcf06f8149b8bb))
* skip-github-release silently skipped tag creation too ([6fc0e40](https://github.com/Monnify/monnify-dotnet-lib/commit/6fc0e402bdfc8b27a09841a6a4b41ff151dee83d))
* update action-gh-release to version 3 for publishing GitHub releases ([427c440](https://github.com/Monnify/monnify-dotnet-lib/commit/427c440d096c54f3fc61cb8b77e23b5875d4730c))

## [0.8.0](https://github.com/Monnify/monnify-dotnet-lib/compare/v0.7.0...v0.8.0) (2026-06-30)


### Features

* add card transactions and automated release tagging ([25a62b3](https://github.com/Monnify/monnify-dotnet-lib/commit/25a62b30c36c7468faad960f48e5872eb9264d8d))
* add card transactions client (charge, OTP, 3DS authorize) ([73cc17e](https://github.com/Monnify/monnify-dotnet-lib/commit/73cc17ea37382099bbefa50423ed48856ac9e8dc))
* add direct debit mandates client ([d8f772d](https://github.com/Monnify/monnify-dotnet-lib/commit/d8f772d03d17b6463abe86ebd7adbde6db48f411))
* add direct debit mandates client ([456d369](https://github.com/Monnify/monnify-dotnet-lib/commit/456d3697fd32f61d9f1fc76f10c58f41b079c352))
* Add limit profiles and sub-account management to IMonnifyCollectionsClient ([e13af00](https://github.com/Monnify/monnify-dotnet-lib/commit/e13af001ee07b831277cb8a12beb2b594a7f4866))
* **collections:** add paycode API support ([a2191b0](https://github.com/Monnify/monnify-dotnet-lib/commit/a2191b0d863e7dad3422d7cdf8e01796920cf7b5))
* customer wallets, paycodes, and envelope fix ([cafe7d2](https://github.com/Monnify/monnify-dotnet-lib/commit/cafe7d2324cc00279733010e9a9a61278d7bed00))
* **disbursements:** add customer wallet API support ([e2bc8d2](https://github.com/Monnify/monnify-dotnet-lib/commit/e2bc8d26f9e4f5d0bad40b62b8cf6498ae6d4355))
* Implement refund management in IMonnifyCollectionsClient with new endpoints and models ([807beee](https://github.com/Monnify/monnify-dotnet-lib/commit/807beeeb8aba3969debdd4afb9fa42c81aa43625))
* sub-accounts, limit profiles, and refunds ([338c5de](https://github.com/Monnify/monnify-dotnet-lib/commit/338c5de8e7823a6b25c6f4885a2068e6a468abe8))


### Bug Fixes

* add nbgv cloud step before build to prevent invalid GITHUB_ENV format error ([253ad61](https://github.com/Monnify/monnify-dotnet-lib/commit/253ad610c2ec3a28745cc9fdafdcf06f8149b8bb))
* skip-github-release silently skipped tag creation too ([6fc0e40](https://github.com/Monnify/monnify-dotnet-lib/commit/6fc0e402bdfc8b27a09841a6a4b41ff151dee83d))
* update action-gh-release to version 3 for publishing GitHub releases ([427c440](https://github.com/Monnify/monnify-dotnet-lib/commit/427c440d096c54f3fc61cb8b77e23b5875d4730c))

## [0.7.0](https://github.com/Monnify/monnify-dotnet-lib/compare/v0.6.0...v0.7.0) (2026-06-30)


### Features

* add card transactions and automated release tagging ([25a62b3](https://github.com/Monnify/monnify-dotnet-lib/commit/25a62b30c36c7468faad960f48e5872eb9264d8d))
* add card transactions client (charge, OTP, 3DS authorize) ([73cc17e](https://github.com/Monnify/monnify-dotnet-lib/commit/73cc17ea37382099bbefa50423ed48856ac9e8dc))
* add direct debit mandates client ([d8f772d](https://github.com/Monnify/monnify-dotnet-lib/commit/d8f772d03d17b6463abe86ebd7adbde6db48f411))
* add direct debit mandates client ([456d369](https://github.com/Monnify/monnify-dotnet-lib/commit/456d3697fd32f61d9f1fc76f10c58f41b079c352))
* Add limit profiles and sub-account management to IMonnifyCollectionsClient ([e13af00](https://github.com/Monnify/monnify-dotnet-lib/commit/e13af001ee07b831277cb8a12beb2b594a7f4866))
* **collections:** add paycode API support ([a2191b0](https://github.com/Monnify/monnify-dotnet-lib/commit/a2191b0d863e7dad3422d7cdf8e01796920cf7b5))
* customer wallets, paycodes, and envelope fix ([cafe7d2](https://github.com/Monnify/monnify-dotnet-lib/commit/cafe7d2324cc00279733010e9a9a61278d7bed00))
* **disbursements:** add customer wallet API support ([e2bc8d2](https://github.com/Monnify/monnify-dotnet-lib/commit/e2bc8d26f9e4f5d0bad40b62b8cf6498ae6d4355))
* Implement refund management in IMonnifyCollectionsClient with new endpoints and models ([807beee](https://github.com/Monnify/monnify-dotnet-lib/commit/807beeeb8aba3969debdd4afb9fa42c81aa43625))
* sub-accounts, limit profiles, and refunds ([338c5de](https://github.com/Monnify/monnify-dotnet-lib/commit/338c5de8e7823a6b25c6f4885a2068e6a468abe8))


### Bug Fixes

* add nbgv cloud step before build to prevent invalid GITHUB_ENV format error ([253ad61](https://github.com/Monnify/monnify-dotnet-lib/commit/253ad610c2ec3a28745cc9fdafdcf06f8149b8bb))
* skip-github-release silently skipped tag creation too ([6fc0e40](https://github.com/Monnify/monnify-dotnet-lib/commit/6fc0e402bdfc8b27a09841a6a4b41ff151dee83d))
* update action-gh-release to version 3 for publishing GitHub releases ([427c440](https://github.com/Monnify/monnify-dotnet-lib/commit/427c440d096c54f3fc61cb8b77e23b5875d4730c))

## [0.6.0](https://github.com/Monnify/monnify-dotnet-lib/compare/v0.5.0...v0.6.0) (2026-06-30)


### Features

* add card transactions and automated release tagging ([25a62b3](https://github.com/Monnify/monnify-dotnet-lib/commit/25a62b30c36c7468faad960f48e5872eb9264d8d))
* add card transactions client (charge, OTP, 3DS authorize) ([73cc17e](https://github.com/Monnify/monnify-dotnet-lib/commit/73cc17ea37382099bbefa50423ed48856ac9e8dc))
* add direct debit mandates client ([d8f772d](https://github.com/Monnify/monnify-dotnet-lib/commit/d8f772d03d17b6463abe86ebd7adbde6db48f411))
* add direct debit mandates client ([456d369](https://github.com/Monnify/monnify-dotnet-lib/commit/456d3697fd32f61d9f1fc76f10c58f41b079c352))
* Add limit profiles and sub-account management to IMonnifyCollectionsClient ([e13af00](https://github.com/Monnify/monnify-dotnet-lib/commit/e13af001ee07b831277cb8a12beb2b594a7f4866))
* **collections:** add paycode API support ([a2191b0](https://github.com/Monnify/monnify-dotnet-lib/commit/a2191b0d863e7dad3422d7cdf8e01796920cf7b5))
* customer wallets, paycodes, and envelope fix ([cafe7d2](https://github.com/Monnify/monnify-dotnet-lib/commit/cafe7d2324cc00279733010e9a9a61278d7bed00))
* **disbursements:** add customer wallet API support ([e2bc8d2](https://github.com/Monnify/monnify-dotnet-lib/commit/e2bc8d26f9e4f5d0bad40b62b8cf6498ae6d4355))
* Implement refund management in IMonnifyCollectionsClient with new endpoints and models ([807beee](https://github.com/Monnify/monnify-dotnet-lib/commit/807beeeb8aba3969debdd4afb9fa42c81aa43625))
* sub-accounts, limit profiles, and refunds ([338c5de](https://github.com/Monnify/monnify-dotnet-lib/commit/338c5de8e7823a6b25c6f4885a2068e6a468abe8))


### Bug Fixes

* add nbgv cloud step before build to prevent invalid GITHUB_ENV format error ([253ad61](https://github.com/Monnify/monnify-dotnet-lib/commit/253ad610c2ec3a28745cc9fdafdcf06f8149b8bb))
* skip-github-release silently skipped tag creation too ([6fc0e40](https://github.com/Monnify/monnify-dotnet-lib/commit/6fc0e402bdfc8b27a09841a6a4b41ff151dee83d))
* update action-gh-release to version 3 for publishing GitHub releases ([427c440](https://github.com/Monnify/monnify-dotnet-lib/commit/427c440d096c54f3fc61cb8b77e23b5875d4730c))

## [Unreleased]

### Features

* add paycode support — `CreatePaycodeAsync`, `GetPaycodesAsync`, `GetPaycodeAsync`, `CancelPaycodeAsync`, `GetUnmaskedPaycodeAsync` (`IMonnifyCollectionsClient`)
* add customer wallet support — `CreateWalletAsync`, `GetWalletsAsync`, `GetCustomerWalletBalanceAsync`, `GetWalletTransactionsAsync` (`IMonnifyDisbursementsClient`)

### Fixed

* envelope handling now tolerates endpoints (e.g. paycodes) that omit `requestSuccessful` from their response
* a non-JSON error body (e.g. a proxy/gateway's own error page for a 502/504) on a failing HTTP status now throws `MonnifyApiException` with the real status code, instead of a misleading `MonnifyDeserializationException` — found via a real sandbox 502 while dogfooding the published package before v1

## [0.5.0](https://github.com/Monnify/monnify-dotnet-lib/compare/v0.4.0...v0.5.0) (2026-06-30)


### Features

* add card transactions and automated release tagging ([25a62b3](https://github.com/Monnify/monnify-dotnet-lib/commit/25a62b30c36c7468faad960f48e5872eb9264d8d))
* add card transactions client (charge, OTP, 3DS authorize) ([73cc17e](https://github.com/Monnify/monnify-dotnet-lib/commit/73cc17ea37382099bbefa50423ed48856ac9e8dc))
* add direct debit mandates client ([d8f772d](https://github.com/Monnify/monnify-dotnet-lib/commit/d8f772d03d17b6463abe86ebd7adbde6db48f411))
* add direct debit mandates client ([456d369](https://github.com/Monnify/monnify-dotnet-lib/commit/456d3697fd32f61d9f1fc76f10c58f41b079c352))
* Add limit profiles and sub-account management to IMonnifyCollectionsClient ([e13af00](https://github.com/Monnify/monnify-dotnet-lib/commit/e13af001ee07b831277cb8a12beb2b594a7f4866))
* Implement refund management in IMonnifyCollectionsClient with new endpoints and models ([807beee](https://github.com/Monnify/monnify-dotnet-lib/commit/807beeeb8aba3969debdd4afb9fa42c81aa43625))
* sub-accounts, limit profiles, and refunds ([338c5de](https://github.com/Monnify/monnify-dotnet-lib/commit/338c5de8e7823a6b25c6f4885a2068e6a468abe8))


### Bug Fixes

* skip-github-release silently skipped tag creation too ([6fc0e40](https://github.com/Monnify/monnify-dotnet-lib/commit/6fc0e402bdfc8b27a09841a6a4b41ff151dee83d))
* update action-gh-release to version 3 for publishing GitHub releases ([427c440](https://github.com/Monnify/monnify-dotnet-lib/commit/427c440d096c54f3fc61cb8b77e23b5875d4730c))

## [0.4.0](https://github.com/Monnify/monnify-dotnet-lib/compare/v0.3.0...v0.4.0) (2026-06-30)


### Features

* add card transactions and automated release tagging ([25a62b3](https://github.com/Monnify/monnify-dotnet-lib/commit/25a62b30c36c7468faad960f48e5872eb9264d8d))
* add card transactions client (charge, OTP, 3DS authorize) ([73cc17e](https://github.com/Monnify/monnify-dotnet-lib/commit/73cc17ea37382099bbefa50423ed48856ac9e8dc))
* add direct debit mandates client ([d8f772d](https://github.com/Monnify/monnify-dotnet-lib/commit/d8f772d03d17b6463abe86ebd7adbde6db48f411))
* add direct debit mandates client ([456d369](https://github.com/Monnify/monnify-dotnet-lib/commit/456d3697fd32f61d9f1fc76f10c58f41b079c352))
* Add limit profiles and sub-account management to IMonnifyCollectionsClient ([e13af00](https://github.com/Monnify/monnify-dotnet-lib/commit/e13af001ee07b831277cb8a12beb2b594a7f4866))
* Implement refund management in IMonnifyCollectionsClient with new endpoints and models ([807beee](https://github.com/Monnify/monnify-dotnet-lib/commit/807beeeb8aba3969debdd4afb9fa42c81aa43625))
* sub-accounts, limit profiles, and refunds ([338c5de](https://github.com/Monnify/monnify-dotnet-lib/commit/338c5de8e7823a6b25c6f4885a2068e6a468abe8))


### Bug Fixes

* skip-github-release silently skipped tag creation too ([6fc0e40](https://github.com/Monnify/monnify-dotnet-lib/commit/6fc0e402bdfc8b27a09841a6a4b41ff151dee83d))
* update action-gh-release to version 3 for publishing GitHub releases ([427c440](https://github.com/Monnify/monnify-dotnet-lib/commit/427c440d096c54f3fc61cb8b77e23b5875d4730c))

## [0.3.0](https://github.com/Monnify/monnify-dotnet-lib/compare/v0.2.0...v0.3.0) (2026-06-30)


### Features

* add direct debit mandates client ([d8f772d](https://github.com/Monnify/monnify-dotnet-lib/commit/d8f772d03d17b6463abe86ebd7adbde6db48f411))
* add direct debit mandates client ([456d369](https://github.com/Monnify/monnify-dotnet-lib/commit/456d3697fd32f61d9f1fc76f10c58f41b079c352))


### Bug Fixes

* skip-github-release silently skipped tag creation too ([6fc0e40](https://github.com/Monnify/monnify-dotnet-lib/commit/6fc0e402bdfc8b27a09841a6a4b41ff151dee83d))
* update action-gh-release to version 3 for publishing GitHub releases ([427c440](https://github.com/Monnify/monnify-dotnet-lib/commit/427c440d096c54f3fc61cb8b77e23b5875d4730c))

## [0.2.0](https://github.com/Monnify/monnify-dotnet-lib/compare/v0.1.0...v0.2.0) (2026-06-30)


### Features

* add card transactions and automated release tagging ([25a62b3](https://github.com/Monnify/monnify-dotnet-lib/commit/25a62b30c36c7468faad960f48e5872eb9264d8d))
* add card transactions client (charge, OTP, 3DS authorize) ([73cc17e](https://github.com/Monnify/monnify-dotnet-lib/commit/73cc17ea37382099bbefa50423ed48856ac9e8dc))

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
