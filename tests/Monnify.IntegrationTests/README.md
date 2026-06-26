# Monnify.IntegrationTests

These tests hit Monnify's real **sandbox** API, validating end-to-end behavior
alongside the mocked unit tests in `Monnify.Tests` (see
[CONTRIBUTING.md](../../CONTRIBUTING.md) and
[docs/COMPATIBILITY.md](../../docs/COMPATIBILITY.md)).

They're tagged `[Trait("Category", "Sandbox")]` and use `[SkippableFact]`
(`Xunit.SkippableFact`) so they **skip silently rather than fail** when
credentials aren't available — CI never has sandbox credentials and excludes
this category entirely via `--filter "Category!=Sandbox"`.

## Running locally

1. Get a Monnify sandbox API key/secret key from your Monnify dashboard.
2. Create a file at the repo root named `.sandbox.env` (already gitignored —
   never commit real credentials) containing:

   ```
   export MONNIFY_SANDBOX_API_KEY=your-sandbox-api-key
   export MONNIFY_SANDBOX_SECRET_KEY=your-sandbox-secret-key
   ```

3. From the repo root:

   ```bash
   source .sandbox.env
   dotnet test tests/Monnify.IntegrationTests
   ```

Without that file sourced, these tests report as `Skipped`, not `Failed`.
