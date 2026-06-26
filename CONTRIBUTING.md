# Contributing

## Adding or fixing an API client

1. Confirm the literal endpoint path, HTTP method, and request/response shape
   against Monnify's sandbox (or their Postman collection) — don't trust the
   public docs alone, they're known to drift and were unreliable when this
   SDK was bootstrapped. Update [docs/COMPATIBILITY.md](docs/COMPATIBILITY.md)
   with the confirmed status.
2. Add the endpoint path as a named constant in `Monnify.Http.MonnifyApiPaths`,
   scoped to its own category — never assume a single global API version
   prefix, since Monnify versions endpoints independently.
3. Add or extend the relevant typed client interface + implementation under
   `src/Monnify/<Category>/`, with request/response DTOs in `Models/`.
4. Add unit tests under `tests/Monnify.Tests/<Category>/` using
   `FakeHttpMessageHandler` — assert the request path/method/body and the
   response mapping, including at least one error-envelope case.
5. If you have sandbox credentials, add a corresponding
   `[Trait("Category", "Sandbox")]` test under `tests/Monnify.IntegrationTests/`.
6. Update `CHANGELOG.md` under `[Unreleased]`.

## Code style

- Nullable reference types and `CancellationToken` parameters are required on
  all public async APIs.
- Public members need XML doc comments (enforced via `GenerateDocumentationFile`).
- Run `dotnet format` before submitting a PR; CI verifies formatting.

## Pull request checklist

- [ ] Tests added/updated and passing (`dotnet test`)
- [ ] `CHANGELOG.md` updated
- [ ] `docs/COMPATIBILITY.md` updated if an endpoint's confirmed status changed
- [ ] No secrets, tokens, or real API keys committed
