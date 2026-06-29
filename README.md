# Monnify .NET SDK

[![CI](https://github.com/Monnify/monnify-dotnet-lib/actions/workflows/ci.yml/badge.svg)](https://github.com/Monnify/monnify-dotnet-lib/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Monnify.svg)](https://www.nuget.org/packages/Monnify)

An idiomatic .NET SDK for the [Monnify](https://developers.monnify.com)
payment gateway API, targeting `netstandard2.0` and `net8.0`.

> **Status: early release (pre-1.0).** Published on NuGet.org starting with
> `0.1.0` — the public API may still change before a stable `1.0`. See
> [docs/COMPATIBILITY.md](docs/COMPATIBILITY.md) for every endpoint this SDK
> calls and how it was verified.

## Features

- **Collections** — hosted checkout, reserved (virtual) accounts, invoices,
  bank-transfer payment links, transaction search
- **Disbursements** — single and bulk transfers, OTP authorization, wallet
  balance, transaction search
- **Bills payment** — airtime, data, cable TV, electricity, and other billers
- **Verification** — account name enquiry, BVN/NIN checks
- **Banks** — bank list, USSD-enabled banks
- **Webhooks** — signature verification, typed event parsing for all 12
  documented event types
- One `AddMonnify(...)` call registers every typed client via
  `Microsoft.Extensions.DependencyInjection`
- Nullable reference types and `CancellationToken` on every async method

## Install

```bash
dotnet add package Monnify
```

## Configure

```csharp
using Monnify;

builder.Services.AddMonnify(options =>
{
    options.ApiKey = "<your API key>";
    options.SecretKey = "<your secret key>";
    options.Environment = MonnifyEnvironment.Sandbox; // or MonnifyEnvironment.Live
});
```

This registers `IMonnifyCollectionsClient`, `IMonnifyDisbursementsClient`,
`IMonnifyBillsClient`, `IMonnifyVerificationClient`, `IMonnifyBanksClient`, and
the `MonnifyClient` facade that aggregates all of them — inject whichever you
need.

## Usage

The examples below assume `collections`, `disbursements`, `bills`,
`verification`, and `banks` are constructor-injected via the interfaces
registered above (e.g. a constructor parameter of type
`IMonnifyCollectionsClient collections`).

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

### Send money

```csharp
var transfer = await disbursements.InitiateSingleTransferAsync(new SingleTransferRequest
{
    Amount = 5000,
    Reference = Guid.NewGuid().ToString("N"),
    Narration = "Refund for order #1234",
    DestinationBankCode = "058",
    DestinationAccountNumber = "0123456789",
    DestinationAccountName = "Jane Doe",
    SourceAccountNumber = "<your Monnify wallet account number>",
});
```

A failed or ambiguous transfer should never be blindly retried with the same
reference — see the remarks on `IMonnifyDisbursementsClient` for the safe
retry pattern (query status first; only retry with a *new* reference).

### Pay a bill

Some products (electricity prepaid plans, for example) require validating the
customer first, which returns a `ValidationReference` to pass into the vend
request — check `VendInstruction.RequireValidationRef` to know if a given
product needs this step:

```csharp
var validation = await bills.ValidateCustomerAsync(new ValidateBillCustomerRequest
{
    ProductCode = "product-ikedc-pre",
    CustomerId = "55555666666",
});

var requiresValidation = validation.VendInstruction?.RequireValidationRef == true;

var vend = await bills.VendAsync(new VendBillRequest
{
    ProductCode = "product-ikedc-pre",
    CustomerId = "55555666666",
    Amount = 500,
    Reference = Guid.NewGuid().ToString("N"),
    ValidationReference = requiresValidation ? validation.VendInstruction!.ValidationReference : null,
});
```

Products that don't need validation (airtime top-ups, for example) can skip
straight to `VendAsync` and leave `ValidationReference` unset.

### Verify an account

```csharp
var account = await verification.ValidateAccountNumberAsync("0123456789", "044");
// account.AccountName
```

### List banks

```csharp
var allBanks = await banks.GetBanksAsync();
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
always `false` there by design — see the remarks on `MonnifyWebhookValidator`.

## Error handling

Every client throws `MonnifyApiException` (carrying `ResponseCode`,
`ResponseMessage`, `HttpStatusCode`, and `RawResponseBody`) when Monnify
reports a failure, rather than returning a null or empty result — payments
are a domain where a silently swallowed failure is dangerous.

```csharp
try
{
    await collections.InitializeTransactionAsync(request);
}
catch (MonnifyApiException ex)
{
    logger.LogError("Monnify rejected the request: {Code} - {Message}", ex.ResponseCode, ex.ResponseMessage);
}
```

## Full runnable samples

- [samples/Monnify.Samples.ConsoleApp](samples/Monnify.Samples.ConsoleApp) — DI
  via the generic host outside ASP.NET Core, listing banks and initializing a
  checkout.
- [samples/Monnify.Samples.WebApi](samples/Monnify.Samples.WebApi) — a minimal
  API webhook receiver with signature verification and event dispatch.

Both read credentials from environment variables; see the comment at the top
of each `Program.cs` for which ones to set.

## Documentation

- [docs/COMPATIBILITY.md](docs/COMPATIBILITY.md) — every SDK method against
  the Monnify endpoint it calls, and how it was verified
- [CHANGELOG.md](CHANGELOG.md)

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) (also covers versioning and the
release process). Security issues: see [SECURITY.md](SECURITY.md).

## License

[MIT](LICENSE)
