# Endpoint Compatibility Ledger

Tracks each SDK method against the Monnify endpoint it calls, and whether the
literal path/method/payload has been independently confirmed against
Monnify's sandbox (not just the public docs, which were unreliable — several
pages 502'd — when this SDK was bootstrapped in 2026-06).

Status legend:
- **Confirmed** — verified against a live sandbox call.
- **Partial** — operation/shape known, literal path inferred from docs nav or
  partial fetch, not yet sandbox-verified.
- **Unconfirmed** — category known to exist; path/payload not yet seen at all.

| SDK Method | Monnify Endpoint | Status | Notes |
|---|---|---|---|
| `MonnifyTokenProvider.LoginAsync` | `POST /api/v1/auth/login` | Confirmed | Basic auth, returns bearer token, 3600s expiry |
| *(Collections — to be added in Phase 3)* | `POST /api/v1/transactions/init` | Confirmed | Checkout/init transaction |
| *(Collections — reserved accounts)* | `POST /api/v2/bank-transfer/reserved-accounts` (assumed v2) | Partial | Not independently reconfirmed this session |
| *(Collections — sub-accounts/splitting)* | unknown | Unconfirmed | Docs site 502'd before path was observed |
| *(Collections — direct debit/mandates)* | unknown | Unconfirmed | Docs site 502'd before path was observed |
| *(Collections — card tokenization)* | unknown | Unconfirmed | Docs site 502'd before path was observed |
| *(Disbursements — single transfer)* | `POST /api/v2/disbursements/single` (assumed v2) | Partial | Operation name confirmed via anchor, literal path not directly observed |
| *(Disbursements — bulk transfer)* | `POST /api/v2/disbursements/batch` (assumed v2) | Partial | Same as above |
| *(Disbursements — wallet balance)* | `GET /api/v2/disbursements/wallet-balance` (assumed) | Partial | |
| *(Verification — BVN)* | unknown | Unconfirmed | |
| *(Banks — list)* | unknown | Unconfirmed | Supported-banks table confirmed to exist (~361 entries with USSD codes), API path not observed |
| *(Webhooks — signature)* | `monnify-signature` header, `SHA-512(secretKey + rawBody)` | Confirmed (algorithm) / Unconfirmed (hex casing) | Hex casing of the computed digest not yet checked against a real captured webhook |
| *(Bills payment)* | unknown | Unconfirmed | Category barely explored; scheduled last |

Update this table every time an endpoint moves between statuses, per
[CONTRIBUTING.md](../CONTRIBUTING.md).
