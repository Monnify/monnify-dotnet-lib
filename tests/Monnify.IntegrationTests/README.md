# Monnify.IntegrationTests

These tests hit Monnify's real **sandbox** API, validating end-to-end behavior
alongside the mocked unit tests in `Monnify.Tests` (see
[CONTRIBUTING.md](../../CONTRIBUTING.md) and
[docs/COMPATIBILITY.md](../../docs/COMPATIBILITY.md)).

They're tagged `[Trait("Category", "Sandbox")]` and use `[SkippableFact]`
(`Xunit.SkippableFact`) so they **skip silently rather than fail** when
credentials aren't available — CI runs `dotnet test` with no filter, but
since it never has sandbox credentials configured, `SandboxCredentials.IsAvailable`
is `false` and every test in this project reports as `Skipped`, not `Failed`.

## Running locally

1. Get a Monnify sandbox API key/secret key from your Monnify dashboard.
2. Create a file at the repo root named `.sandbox.env` (already gitignored —
   never commit real credentials) containing:

   ```
   export MONNIFY_SANDBOX_API_KEY=your-sandbox-api-key
   export MONNIFY_SANDBOX_SECRET_KEY=your-sandbox-secret-key
   export MONNIFY_SANDBOX_CONTRACT_CODE=your-contract-code
   export MONNIFY_SANDBOX_DISBURSEMENT_WALLET=your-wallet-account-number
   export MONNIFY_SANDBOX_DISBURSEMENT_DESTINATION_BANK_CODE=a-real-bank-code
   export MONNIFY_SANDBOX_DISBURSEMENT_DESTINATION_ACCOUNT=a-real-account-number
   ```

   The Disbursements tests need a real, name-enquiry-resolvable destination account — a made-up
   account number gets rejected by Monnify's own validation before a transfer is even created.

3. From the repo root:

   ```bash
   source .sandbox.env
   dotnet test tests/Monnify.IntegrationTests
   ```

Without that file sourced, these tests report as `Skipped`, not `Failed`.
