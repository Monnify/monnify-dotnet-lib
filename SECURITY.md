# Security Policy

## Reporting a Vulnerability

This is a community-maintained, unofficial SDK for the Monnify payment gateway
API. If you discover a security vulnerability in this SDK (not in Monnify's
own platform), please report it privately via GitHub's
[private vulnerability reporting](https://docs.github.com/en/code-security/security-advisories/guidance-on-reporting-and-writing/privately-reporting-a-security-vulnerability)
feature on this repository rather than opening a public issue.

Vulnerabilities in Monnify's own API or infrastructure should be reported
directly to Monnify, not here.

## Handling Credentials

- Never commit a real Monnify API key, secret key, or webhook secret to this
  repository or to any project consuming this SDK. Use environment variables,
  `IConfiguration`, or a secrets manager.
- The SDK never logs the secret key, API key, or access token. If you observe
  a code path that does, treat it as a security bug and report it as above.

## Webhook Signature Verification

`MonnifyWebhookValidator` performs a constant-time comparison of the computed
`SHA-512(secretKey + rawBody)` signature against the `monnify-signature`
header to avoid timing attacks. Always validate the signature before trusting
or acting on a webhook payload. Monnify's documented sender IP
(`35.242.133.146`) can change over time — do not rely on IP allowlisting as
your only control; treat it as defense-in-depth at most, layered on top of
signature verification, not a replacement for it.

## Supported Versions

Security fixes are applied to the latest minor version on the current major
release line. See [CHANGELOG.md](CHANGELOG.md) for release history.
