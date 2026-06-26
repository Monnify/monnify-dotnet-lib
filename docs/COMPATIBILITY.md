# Endpoint Compatibility Ledger

Tracks each SDK method against the Monnify endpoint it calls, for maintainers
adding or changing a client (see [CONTRIBUTING.md](../CONTRIBUTING.md)).

Status legend:
- **Implemented** — shipped, with the path/payload sourced from a live
  sandbox call or Monnify's official API reference.
- **Planned** — designed in the implementation plan, not yet built.

| SDK Method | Monnify Endpoint | Status | Notes |
|---|---|---|---|
| `MonnifyTokenProvider.LoginAsync` | `POST /api/v1/auth/login` | **Implemented** | Basic auth (`base64(apiKey:secretKey)`); envelope `{requestSuccessful, responseMessage, responseCode, responseBody:{accessToken, expiresIn}}` |
| *(Collections — checkout/init)* | `POST /api/v1/transactions/init` | Planned | Phase 3 |
| *(Collections — reserved accounts)* | `POST /api/v2/bank-transfer/reserved-accounts` | Planned | Phase 3 |
| *(Collections — sub-accounts/splitting)* | TBD | Planned | Phase 3 |
| *(Collections — direct debit/mandates)* | TBD | Planned | Phase 3 |
| *(Collections — card tokenization)* | TBD | Planned | Phase 3 |
| *(Disbursements — single transfer)* | `POST /api/v2/disbursements/single` | Planned | Phase 4 |
| *(Disbursements — bulk transfer)* | `POST /api/v2/disbursements/batch` | Planned | Phase 4 |
| *(Disbursements — wallet balance)* | `GET /api/v2/disbursements/wallet-balance` | Planned | Phase 4 |
| `IMonnifyVerificationClient.ValidateAccountNumberAsync` | `GET /api/v1/disbursements/account/validate?accountNumber=&bankCode=` | **Implemented** | Free on both sandbox and live; `responseBody: {accountNumber, accountName, bankCode, currencyCode}` |
| `IMonnifyVerificationClient.MatchBvnDetailsAsync` | `POST /api/v1/vas/bvn-details-match` | **Implemented** | Bills the merchant wallet per request; response shape mixes a structured match object (`name`) with plain match-status strings (`dateOfBirth`, `mobileNo`) |
| `IMonnifyVerificationClient.MatchBvnToAccountAsync` | `POST /api/v1/vas/bvn-account-match` | **Implemented** | Bills the merchant wallet per request |
| `IMonnifyVerificationClient.VerifyNinAsync` | `POST /api/v1/vas/nin-details` | **Implemented** | Live environment only; bills the merchant wallet per request |
| `IMonnifyBanksClient.GetBanksAsync` | `GET /api/v1/banks` | **Implemented** | `responseBody` is a JSON array of `{name, code, nipBankCode, ussdTemplate, baseUssdCode, transferUssdTemplate}`; most entries have null USSD fields |
| `IMonnifyBanksClient.GetUssdEnabledBanksAsync` | `GET /api/v1/sdk/transactions/banks` | **Implemented** | Subset of banks with USSD info; not every entry on this list has a populated `ussdTemplate` (e.g. Suntrust Bank) |
| *(Webhooks — signature)* | `monnify-signature` header, `SHA-512(secretKey + rawBody)` | Planned | Phase 5 |
| *(Bills payment)* | TBD | Planned | Phase 6 |

Update this table whenever a method moves from Planned to Implemented, or its
endpoint changes.
