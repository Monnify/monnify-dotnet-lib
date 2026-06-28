# Monnify .NET SDK

[![CI](https://github.com/monnify-dotnet/monnify-dotnet/actions/workflows/ci.yml/badge.svg)](https://github.com/monnify-dotnet/monnify-dotnet/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Monnify.svg)](https://www.nuget.org/packages/Monnify)

An idiomatic, community-maintained .NET SDK for the
[Monnify](https://developers.monnify.com) payment gateway API — collections,
disbursements, bills payment, verification, banks, and webhook signature
validation — targeting `netstandard2.0` and `net8.0`. This is not an official
Monnify project.

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
- [x] Phase 4 — disbursements client
- [x] Phase 5 — webhooks
- [x] Phase 6 — bills payment
- [x] Phase 7 — samples + quickstarts
- [x] Phase 8a — CI/CD (build/test/format/pack-validation on every PR, CodeQL, Dependabot)
- [ ] Phase 8b — first NuGet release (infrastructure is in place; publishing is
      gated behind a manual approval and hasn't happened yet — see
      [Releasing](#releasing) below)

## Quickstart

### Install

Not yet published to NuGet.org (see [Status](#status)). Until then, reference
the project directly or build the `.nupkg` locally:

```bash
dotnet pack src/Monnify/Monnify.csproj -c Release
```

### Register services

```csharp
using Monnify;

builder.Services.AddMonnify(options =>
{
    options.ApiKey = "<your API key>";
    options.SecretKey = "<your secret key>";
    options.Environment = MonnifyEnvironment.Sandbox; // or MonnifyEnvironment.Live
});
```

This registers every typed client (`IMonnifyCollectionsClient`,
`IMonnifyDisbursementsClient`, `IMonnifyBillsClient`, `IMonnifyVerificationClient`,
`IMonnifyBanksClient`) plus the `MonnifyClient` facade. Inject whichever one you
need.

### Collect a payment

```csharp
var checkout = await collections.InitializeTransactionAsync(new InitializeTransactionRequest
{
    Amount = 1000,
    CustomerName = "Jane Doe",
    CustomerEmail = "jane.doe@example.com",
    PaymentReference = Guid.NewGuid().ToString("N"),
    PaymentDescription = "Order #1234",
    ContractCode = "<your contract code>",
});

// Redirect the customer to checkout.CheckoutUrl
```

### Receive webhooks

```csharp
app.MapPost("/webhooks/monnify", async (HttpRequest request, ILogger<Program> logger) =>
{
    var validation = await request.ValidateMonnifyWebhookAsync(secretKey);
    if (!validation.IsValid)
    {
        return Results.Unauthorized();
    }

    var envelope = validation.GetEnvelope();
    if (envelope.EventType == MonnifyWebhookEventTypes.SuccessfulTransaction)
    {
        var data = MonnifyWebhookParser.ParseEventData<CollectionTransactionEventData>(envelope);
        // ... record the payment
    }

    return Results.Ok(); // acknowledge immediately; do slow work out of band
});
```

Note: our sandbox sends no `monnify-signature` header at all, so `IsValid` is
always `false` there by design - see the remarks on `MonnifyWebhookValidator`.

### Full runnable samples

- [samples/Monnify.Samples.ConsoleApp](samples/Monnify.Samples.ConsoleApp) — DI
  via the generic host outside ASP.NET Core, listing banks and initializing a
  checkout.
- [samples/Monnify.Samples.WebApi](samples/Monnify.Samples.WebApi) — a minimal
  API webhook receiver with signature verification and event dispatch.

Both read credentials from environment variables; see the comment at the top
of each `Program.cs` for which ones to set.

## Releasing

Versioning is computed by Nerdbank.GitVersioning from `version.json` and git
history - every commit gets a meaningful prerelease version
(`0.1.18-alpha.gb301a5cb61`-style) with no manual bumping.

Pushing a `vX.Y.Z` tag triggers [release.yml](.github/workflows/release.yml),
which builds, tests, and packs, then **requires manual approval** before
publishing: the publish job runs under a `nuget-release` GitHub Environment,
which needs required reviewers configured (Settings → Environments) and a
`NUGET_API_KEY` secret scoped to that environment specifically, not a
repo-wide secret. Until that environment is set up, the publish job has no
reviewers and nothing will publish — that's the intended state until this is
deliberately configured and approved.

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md). Security issues: see [SECURITY.md](SECURITY.md).

## License

[MIT](LICENSE)
