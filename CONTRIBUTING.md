# Contributing

## Adding or fixing an API client

1. Confirm the literal endpoint path, HTTP method, and request/response shape
   against the sandbox (or our official API reference/Postman collection).
   Update [docs/COMPATIBILITY.md](docs/COMPATIBILITY.md) once it's
   implemented.
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
- [ ] `docs/COMPATIBILITY.md` updated for any endpoint you implemented
- [ ] No secrets, tokens, or real API keys committed

## Versioning and releasing

Versioning is computed by Nerdbank.GitVersioning from `version.json` and git
history — every commit gets a meaningful prerelease version
(`0.1.18-alpha.gb301a5cb61`-style) automatically; there's no manual version
bump.

Pushing a `vX.Y.Z` tag triggers [release.yml](.github/workflows/release.yml),
which builds, tests, and packs, then **requires manual approval** before
publishing: the publish job runs under a `nuget-release` GitHub Environment
(already configured with required reviewers).

Publishing itself uses [NuGet Trusted Publishing](https://learn.microsoft.com/en-us/nuget/nuget-org/trusted-publishing)
(OIDC) rather than a stored API key — the workflow exchanges a short-lived
GitHub token for a temporary (~1hr) NuGet API key at publish time, so there's
no long-lived secret to leak or rotate. The matching policy on nuget.org
(account → Trusted Publishing) has Repository Owner=`Monnify`,
Repository=`monnify-dotnet-lib`, Workflow File=`release.yml`,
Environment=`nuget-release` — the environment restriction means a token is
only trusted if it came from a run that passed through that approval gate.
The only secret involved is `NUGET_USER` (the nuget.org profile username,
not the account email), scoped to the `nuget-release` environment.

Both the `id-token: write` (OIDC) and `contents: write` (creating the GitHub
Release) permissions need to be listed explicitly on the `publish` job —
job-level `permissions:` replaces the workflow-level block entirely for that
job rather than adding to it, so listing only one silently drops the other.
