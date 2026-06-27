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
| `IMonnifyCollectionsClient.InitializeTransactionAsync` | `POST /api/v1/merchant/transactions/init-transaction` | **Implemented** | Returns a hosted `checkoutUrl`; `contractCode` is required |
| `IMonnifyCollectionsClient.CreateReservedAccountAsync` | `POST /api/v2/bank-transfer/reserved-accounts` | **Implemented** | `getAllAvailableBanks` is required (bool); if `false`, `preferredBanks` must be supplied |
| `IMonnifyCollectionsClient.GetReservedAccountAsync` | `GET /api/v2/bank-transfer/reserved-accounts/{accountReference}` | **Implemented** | |
| `IMonnifyCollectionsClient.GetReservedAccountTransactionsAsync` | `GET /api/v1/bank-transfer/reserved-accounts/transactions?accountReference=&page=&size=` | **Implemented** | Per-transaction item fields modeled from general Monnify conventions; no real example with content was observed (test account had zero transactions) |
| `IMonnifyCollectionsClient.DeleteReservedAccountAsync` | `DELETE /api/v1/bank-transfer/reserved-accounts/reference/{accountReference}` | **Implemented** | Note the `/reference/` path segment — easy to miss |
| `IMonnifyCollectionsClient.CreateInvoiceAsync` | `POST /api/v1/invoice/create` | **Implemented** | Sandbox rejects `expiryDate` too far in the future |
| `IMonnifyCollectionsClient.GetInvoiceAsync` | `GET /api/v1/invoice/{invoiceReference}/details` | **Implemented** | |
| `IMonnifyCollectionsClient.GetInvoicesAsync` | `GET /api/v1/invoice/all?page=&size=` | **Implemented** | |
| `IMonnifyCollectionsClient.CancelInvoiceAsync` | `DELETE /api/v1/invoice/{invoiceReference}/cancel` | **Implemented** | |
| `IMonnifyCollectionsClient.InitiateBankTransferAsync` | `POST /api/v1/merchant/bank-transfer/init-payment` | **Implemented** | Generates a one-time dynamic account for an already-initialized transaction |
| `IMonnifyCollectionsClient.SearchTransactionsAsync` | `GET /api/v1/transactions/search?page=&size=&...` | **Implemented** | Many optional filters (reference, amount range, customer, status, date range) |
| `IMonnifyCollectionsClient.GetTransactionAsync` | `GET /api/v2/transactions/{transactionReference}` | **Implemented** | `amountPaid`/`totalPayable`/`settlementAmount` are quoted strings in this response; deserialized into `decimal` via `JsonNumberHandling.AllowReadingFromString` |
| `IMonnifyCollectionsClient.QueryTransactionAsync` | `GET /api/v2/merchant/transactions/query?transactionReference=&paymentReference=` | **Implemented** | Same response shape as `GetTransactionAsync`; requires at least one of the two query parameters |
| *(Collections — sub-accounts/splitting)* | TBD | Planned | Phase 3 follow-up |
| *(Collections — direct debit/mandates)* | TBD | Planned | Phase 3 follow-up |
| *(Collections — card tokenization)* | TBD | Planned | Phase 3 follow-up |
| *(Collections — Payment Links)* | N/A | Out of scope | Dashboard-only feature, no API |
| `IMonnifyDisbursementsClient.InitiateSingleTransferAsync` | `POST /api/v2/disbursements/single` | **Implemented** | Requires Monnify to have activated the Transfer feature for the merchant (contact sales@monnify.com); registered with automatic retry disabled |
| `IMonnifyDisbursementsClient.AuthorizeSingleTransferAsync` | `POST /api/v2/disbursements/single/validate-otp` | **Implemented** | Authorizes a transfer with status `PENDING_AUTHORIZATION` |
| `IMonnifyDisbursementsClient.ResendSingleTransferOtpAsync` | `POST /api/v2/disbursements/single/resend-otp` | **Implemented** | |
| `IMonnifyDisbursementsClient.GetSingleTransferAsync` | `GET /api/v2/disbursements/single/summary?reference=` | **Implemented** | |
| `IMonnifyDisbursementsClient.GetSingleTransfersAsync` | `GET /api/v2/disbursements/single/transactions?pageNo=&pageSize=` | **Implemented** | |
| `IMonnifyDisbursementsClient.InitiateBulkTransferAsync` | `POST /api/v2/disbursements/batch` | **Implemented** | Response includes an extra `transactionBatchReference` (Monnify's own internal batch id) not shown in the official docs sample; registered with automatic retry disabled |
| `IMonnifyDisbursementsClient.AuthorizeBulkTransferAsync` | `POST /api/v2/disbursements/batch/validate-otp` | **Implemented** | Despite being the *batch* endpoint, the body field is `reference`, not `batchReference` |
| `IMonnifyDisbursementsClient.ResendBulkTransferOtpAsync` | `POST /api/v2/disbursements/batch/resend-otp` | **Implemented** | Body field is `batchReference` here (unlike the validate-otp endpoint above) |
| `IMonnifyDisbursementsClient.GetBulkTransferSummaryAsync` | `GET /api/v2/disbursements/batch/summary?reference=` | **Implemented** | |
| `IMonnifyDisbursementsClient.GetBulkTransferTransactionsAsync` | `GET /api/v2/disbursements/bulk/{batchReference}/transactions?pageSize=&pageNo=` | **Implemented** | |
| `IMonnifyDisbursementsClient.GetBulkTransfersAsync` | `GET /api/v2/disbursements/bulk?sourceAccountNumber=&pageNo=&pageSize=` | **Implemented** | Monnify's docs say `GET /api/v2/disbursements/bulk/transactions`, which 404s in the sandbox — the correct path has no `/transactions` suffix |
| `IMonnifyDisbursementsClient.SearchTransactionsAsync` | `GET /api/v2/disbursements/search-transactions?sourceAccountNumber=&...` | **Implemented** | |
| `IMonnifyDisbursementsClient.GetWalletBalanceAsync` | `GET /api/v2/disbursements/wallet-balance?accountNumber=` | **Implemented** | Sandbox returns plain numeric balances; Monnify's docs show them as quoted strings — `decimal` properties accept either via `JsonNumberHandling.AllowReadingFromString` |
| `IMonnifyVerificationClient.ValidateAccountNumberAsync` | `GET /api/v1/disbursements/account/validate?accountNumber=&bankCode=` | **Implemented** | Free on both sandbox and live; `responseBody: {accountNumber, accountName, bankCode, currencyCode}` |
| `IMonnifyVerificationClient.MatchBvnDetailsAsync` | `POST /api/v1/vas/bvn-details-match` | **Implemented** | Bills the merchant wallet per request; response shape mixes a structured match object (`name`) with plain match-status strings (`dateOfBirth`, `mobileNo`) |
| `IMonnifyVerificationClient.MatchBvnToAccountAsync` | `POST /api/v1/vas/bvn-account-match` | **Implemented** | Bills the merchant wallet per request |
| `IMonnifyVerificationClient.VerifyNinAsync` | `POST /api/v1/vas/nin-details` | **Implemented** | Live environment only; bills the merchant wallet per request |
| `IMonnifyBanksClient.GetBanksAsync` | `GET /api/v1/banks` | **Implemented** | `responseBody` is a JSON array of `{name, code, nipBankCode, ussdTemplate, baseUssdCode, transferUssdTemplate}`; most entries have null USSD fields |
| `IMonnifyBanksClient.GetUssdEnabledBanksAsync` | `GET /api/v1/sdk/transactions/banks` | **Implemented** | Subset of banks with USSD info; not every entry on this list has a populated `ussdTemplate` (e.g. Suntrust Bank) |
| `MonnifyWebhookValidator.IsValid` / `ComputeSignature` | `monnify-signature` header | **Implemented** | `HMAC-SHA512(key: secretKey, message: rawRequestBody)`, lowercase hex. Monnify's docs prose says "SHA-512(secretKey + body)" (no HMAC), and their own JS sample pretty-prints the body before hashing — neither reproduces the "Hashed Value" the same docs page publishes. Verified against Monnify's documented sample secret/body/hash using the *compact* JSON form, which matches; their Java sample (unlike the JS one) hashes the compact form too. Monnify's sandbox sends no `monnify-signature` header at all, so `IsValid` always returns `false` there by design — there is deliberately no environment-aware bypass |
| `MonnifyWebhookParser.Parse` / `ParseEventData` / `ParseMetaData` | n/a (parses the request body, not an HTTP call) | **Implemented** | 11 event-data classes cover the 12 documented `eventType` values — `SUCCESSFUL_TRANSACTION`, `SUCCESSFUL/FAILED/REVERSED_DISBURSEMENT`, and `SUCCESSFUL/FAILED_REFUND` each share one class per family since their `eventData` shapes are identical, differentiated by a `Status`/`PaymentStatus`/`RefundStatus` field. All verified against Monnify's own documented sample payloads (see `MonnifyWebhookEventDataTests`), which surfaced several real inconsistencies: `paymentSourceInformation` comes back as an array, an empty object, *or* a single populated object depending on event variant (handled by `SingleOrArrayJsonConverter`); `settlementAmount`/`refundAmount`/etc. are sometimes quoted strings and sometimes numbers (`JsonNumberHandling.AllowReadingFromString`); the rejected-payment sample uses `created_on` (snake_case) instead of every other event's `createdOn`; the wallet-activity (`ACCOUNT_ACTIVITY`) event puts `metaData` as a sibling of `eventData` at the envelope root instead of nesting it inside, unlike every other event type |
| `HttpRequest.ValidateMonnifyWebhookAsync` | n/a (ASP.NET Core convenience) | **Implemented** | Reads `HttpRequest.Body` to completion once and does not reset its position — intended for a dedicated webhook endpoint, not one that also relies on framework model binding for the same request |
| `IMonnifyBillsClient.GetBillerCategoriesAsync` | `GET /api/v1/vas/bills-payment/biller-categories?page=&size=` | **Implemented (unverified against sandbox)** | Bills payment is **not active by default** for any merchant — email integration-support@monnify.com to activate it. Built directly from Monnify's documented samples; not yet exercised against a real sandbox call. Paging here is 1-based (`page` defaults to `1`), unlike every other paginated list in this SDK, which is 0-based — that's Monnify's own documented default, not a typo |
| `IMonnifyBillsClient.GetBillersAsync` | `GET /api/v1/vas/bills-payment/billers?page=&size=&category_code=` | **Implemented (unverified against sandbox)** | See the Bills activation note above |
| `IMonnifyBillsClient.GetBillerProductsAsync` | `GET /api/v1/vas/bills-payment/biller-products?biller_code=&page=&size=` | **Implemented (unverified against sandbox)** | `biller_code` isn't explicitly marked required in the docs' parameter table, but the endpoint description is "products for a specific biller" — implemented as a required parameter pending sandbox verification |
| `IMonnifyBillsClient.ValidateCustomerAsync` | `POST /api/v1/vas/bills-payment/validate-customer` | **Implemented (unverified against sandbox)** | The only response sample available used the "without a required reference" example variant; the "with a required reference" variant (which should include a `validationReference` field) was never seen, so `ValidateBillCustomerResult.ValidationReference` is inferred from the request-side docs prose, not a confirmed response sample |
| `IMonnifyBillsClient.VendAsync` | `POST /api/v1/vas/bills-payment/vend` | **Implemented (unverified against sandbox)** | Registered with automatic retry disabled, same reasoning as Disbursements — an ambiguous failure must be resolved via `RequeryAsync` with the same reference, not retried with the same one |
| `IMonnifyBillsClient.RequeryAsync` | `GET /api/v1/vas/bills-payment/requery?reference=` | **Implemented (unverified against sandbox)** | Returns the same response shape as `VendAsync`, per Monnify's docs |

Update this table whenever a method moves from Planned to Implemented, or its
endpoint changes.
