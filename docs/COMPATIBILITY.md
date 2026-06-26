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
| `MonnifyTokenProvider.LoginAsync` | `POST /api/v1/auth/login` | **Confirmed** (live sandbox call, 2026-06-26) | Basic auth (`base64(apiKey:secretKey)`); envelope is exactly `{requestSuccessful, responseMessage, responseCode, responseBody:{accessToken, expiresIn}}`; observed `expiresIn` was 3588s (~3600s) |
| *(Collections — to be added in Phase 3)* | `POST /api/v1/transactions/init` | Confirmed | Checkout/init transaction |
| *(Collections — reserved accounts)* | `POST /api/v2/bank-transfer/reserved-accounts` (assumed v2) | Partial | Not independently reconfirmed this session |
| *(Collections — sub-accounts/splitting)* | unknown | Unconfirmed | Docs site 502'd before path was observed |
| *(Collections — direct debit/mandates)* | unknown | Unconfirmed | Docs site 502'd before path was observed |
| *(Collections — card tokenization)* | unknown | Unconfirmed | Docs site 502'd before path was observed |
| *(Disbursements — single transfer)* | `POST /api/v2/disbursements/single` (assumed v2) | Partial | Operation name confirmed via anchor, literal path not directly observed |
| *(Disbursements — bulk transfer)* | `POST /api/v2/disbursements/batch` (assumed v2) | Partial | Same as above |
| *(Disbursements — wallet balance)* | `GET /api/v2/disbursements/wallet-balance` (assumed) | Partial | |
| `IMonnifyVerificationClient.ValidateAccountNumberAsync` | `GET /api/v1/disbursements/account/validate?accountNumber=&bankCode=` | **Confirmed** (live sandbox call, 2026-06-26) | Free on both sandbox and live per Monnify's docs; envelope `responseBody: {accountNumber, accountName, bankCode, currencyCode}` |
| *(Verification — BVN/NIN)* | unknown | **Not implemented** | Confirmed live-only by the user (2026-06-26) and billed against the merchant wallet per request — can never be sandbox-verified, and wasn't probed against live without explicit authorization to spend real money. Revisit once prod verification is authorized. |
| `IMonnifyBanksClient.GetBanksAsync` | `GET /api/v1/banks` | **Confirmed** (live sandbox call, 2026-06-26) | Returns all banks; `responseBody` is a JSON array of `{name, code, nipBankCode, ussdTemplate, baseUssdCode, transferUssdTemplate}` — most entries have null USSD fields |
| `IMonnifyBanksClient.GetUssdEnabledBanksAsync` | `GET /api/v1/sdk/transactions/banks` | **Confirmed** (live sandbox call, 2026-06-26) | Subset of banks; even here, at least one entry (Suntrust Bank) was observed with a null `ussdTemplate` — don't assume every item has one populated |
| *(Webhooks — signature)* | `monnify-signature` header, `SHA-512(secretKey + rawBody)` | Confirmed (algorithm) / Unconfirmed (hex casing) | Hex casing of the computed digest not yet checked against a real captured webhook |
| *(Bills payment)* | unknown | Unconfirmed | Category barely explored; scheduled last |

Update this table every time an endpoint moves between statuses, per
[CONTRIBUTING.md](../CONTRIBUTING.md).
