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

## Commit messages / PR titles

This repo follows [Conventional Commits](https://www.conventionalcommits.org/)
(`feat: ...`, `fix: ...`, `chore: ...`, etc., with a `BREAKING CHANGE:` footer
when needed). This isn't just a style preference — `release-please` (see
below) parses these to decide the next version number and to write the
changelog automatically, so it needs to be the PR title if you squash-merge,
or every commit message if you merge/rebase.

## Pull request checklist

- [ ] Tests added/updated and passing (`dotnet test`)
- [ ] `CHANGELOG.md` updated
- [ ] `docs/COMPATIBILITY.md` updated for any endpoint you implemented
- [ ] No secrets, tokens, or real API keys committed

## Versioning and releasing

The version number and the release tag are both decided automatically by
[release-please](.github/workflows/release-please.yml), not by hand:

1. Every PR merged to `main` is analyzed by `release-please` for Conventional
   Commits since the last release. It maintains a single standing **Release
   PR** that accumulates these, with the computed next version
   (`fix:` → patch, `feat:` → minor, `BREAKING CHANGE:` → major) and an
   auto-written `CHANGELOG.md` section.
2. Merging that Release PR creates and pushes a `vX.Y.Z` tag.
3. That tag push is what triggers [release.yml](.github/workflows/release.yml),
   which builds, tests, and packs, then **requires manual approval** before
   publishing: the publish job runs under a `nuget-release` GitHub Environment
   (already configured with required reviewers).

Don't create or push a `vX.Y.Z` tag by hand — that races with
`release-please`'s own bookkeeping in `.release-please-manifest.json` and can
leave it confused about what's already shipped.

`release-please` pushes its tag using a fine-grained PAT in the
`RELEASE_PLEASE_TOKEN` repo secret, not the default `GITHUB_TOKEN` - GitHub
blocks the default token's pushes from triggering other workflows (its
built-in loop-prevention), which would otherwise silently break the chain
right before it reaches `release.yml`. `skip-github-release: true` in
`release-please-config.json` stops `release-please` from also creating a
GitHub Release itself - `release.yml`'s own publish job already creates one
after a successful NuGet push, and having both would create two Releases for
the same tag.

Nerdbank.GitVersioning still computes the actual assembly/package version
from `version.json` and git history at build time, the same as before -
`release-please` only decides *which* tag to create; once that tag exists and
matches `version.json`'s `publicReleaseRefSpec`, NBGV treats it as the public
release version directly.

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
