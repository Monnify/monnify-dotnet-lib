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
