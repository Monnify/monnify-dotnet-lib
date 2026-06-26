# Monnify .NET SDK

[![CI](https://github.com/monnify-dotnet/monnify-dotnet/actions/workflows/ci.yml/badge.svg)](https://github.com/monnify-dotnet/monnify-dotnet/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Monnify.svg)](https://www.nuget.org/packages/Monnify)

An idiomatic, community-maintained .NET SDK for the
[Monnify](https://developers.monnify.com) payment gateway API — collections,
disbursements, verification, banks, and webhook signature validation —
targeting `netstandard2.0` and `net8.0`. This is not an official Monnify
project.

> **Status: pre-release / under active development.** See
> [implementation phasing](#status) below and
> [docs/COMPATIBILITY.md](docs/COMPATIBILITY.md) for which endpoints are
> implemented.

## Status

This SDK is being built incrementally. Each milestone ships working, tested
code:

- [x] Phase 0 — repo scaffolding
- [x] Phase 1 — core HTTP + authentication infrastructure
- [x] Phase 2 — verification + banks clients
- [x] Phase 3 — collections client
- [ ] Phase 4 — disbursements client
- [ ] Phase 5 — webhooks
- [ ] Phase 6 — bills payment
- [ ] Phase 7 — samples + quickstarts (this README gets filled in here)
- [ ] Phase 8 — CI/CD + first NuGet release

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md). Security issues: see [SECURITY.md](SECURITY.md).

## License

[MIT](LICENSE)
