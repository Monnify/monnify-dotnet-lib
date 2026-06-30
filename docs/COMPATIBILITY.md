# Endpoint Compatibility Ledger

Tracks each SDK method against the Monnify endpoint it calls, for maintainers
adding or changing a client (see [CONTRIBUTING.md](../CONTRIBUTING.md)).

Status legend:
- **Implemented** — shipped, with the path/payload sourced from a live
  sandbox call or our official API reference.
- **Planned** — designed in the implementation plan, not yet built.

| SDK Method | Monnify Endpoint | Status | Notes |
|---|---|---|---|
| `MonnifyTokenProvider.LoginAsync` | `POST /api/v1/auth/login` | **Implemented** | Basic auth (`base64(apiKey:secretKey)`) |
| `IMonnifyCollectionsClient.InitializeTransactionAsync` | `POST /api/v1/merchant/transactions/init-transaction` | **Implemented** | Returns a hosted `checkoutUrl`; `contractCode` is required |
| `IMonnifyCollectionsClient.CreateReservedAccountAsync` | `POST /api/v2/bank-transfer/reserved-accounts` | **Implemented** | `getAllAvailableBanks` is required (bool); if `false`, `preferredBanks` must be supplied |
| `IMonnifyCollectionsClient.GetReservedAccountAsync` | `GET /api/v2/bank-transfer/reserved-accounts/{accountReference}` | **Implemented** | |
| `IMonnifyCollectionsClient.GetReservedAccountTransactionsAsync` | `GET /api/v1/bank-transfer/reserved-accounts/transactions?accountReference=&page=&size=` | **Implemented** | Item fields modeled from general conventions, not a populated real example |
| `IMonnifyCollectionsClient.DeleteReservedAccountAsync` | `DELETE /api/v1/bank-transfer/reserved-accounts/reference/{accountReference}` | **Implemented** | Note the `/reference/` path segment — easy to miss |
| `IMonnifyCollectionsClient.CreateInvoiceAsync` | `POST /api/v1/invoice/create` | **Implemented** | Sandbox rejects `expiryDate` too far in the future |
| `IMonnifyCollectionsClient.GetInvoiceAsync` | `GET /api/v1/invoice/{invoiceReference}/details` | **Implemented** | |
| `IMonnifyCollectionsClient.GetInvoicesAsync` | `GET /api/v1/invoice/all?page=&size=` | **Implemented** | |
| `IMonnifyCollectionsClient.CancelInvoiceAsync` | `DELETE /api/v1/invoice/{invoiceReference}/cancel` | **Implemented** | |
| `IMonnifyCollectionsClient.InitiateBankTransferAsync` | `POST /api/v1/merchant/bank-transfer/init-payment` | **Implemented** | Generates a one-time dynamic account for an already-initialized transaction |
| `IMonnifyCollectionsClient.SearchTransactionsAsync` | `GET /api/v1/transactions/search?page=&size=&...` | **Implemented** | Many optional filters (reference, amount range, customer, status, date range) |
| `IMonnifyCollectionsClient.GetTransactionAsync` | `GET /api/v2/transactions/{transactionReference}` | **Implemented** | Amount fields are quoted strings; `decimal` via `JsonNumberHandling.AllowReadingFromString` |
| `IMonnifyCollectionsClient.QueryTransactionAsync` | `GET /api/v2/merchant/transactions/query?transactionReference=&paymentReference=` | **Implemented** | Same response shape as `GetTransactionAsync`; requires at least one query parameter |
| *(Collections — sub-accounts/splitting)* | TBD | Planned | |
| *(Collections — direct debit/mandates)* | TBD | Planned | |
| *(Collections — card tokenization)* | TBD | Planned | |
| *(Collections — Payment Links)* | N/A | Out of scope | Dashboard-only feature, no API |
| `IMonnifyDisbursementsClient.InitiateSingleTransferAsync` | `POST /api/v2/disbursements/single` | **Implemented** | Requires Transfer feature activation (sales@monnify.com); automatic retry disabled |
| `IMonnifyDisbursementsClient.AuthorizeSingleTransferAsync` | `POST /api/v2/disbursements/single/validate-otp` | **Implemented** | For a transfer with status `PENDING_AUTHORIZATION` |
| `IMonnifyDisbursementsClient.ResendSingleTransferOtpAsync` | `POST /api/v2/disbursements/single/resend-otp` | **Implemented** | |
| `IMonnifyDisbursementsClient.GetSingleTransferAsync` | `GET /api/v2/disbursements/single/summary?reference=` | **Implemented** | |
| `IMonnifyDisbursementsClient.GetSingleTransfersAsync` | `GET /api/v2/disbursements/single/transactions?pageNo=&pageSize=` | **Implemented** | |
| `IMonnifyDisbursementsClient.InitiateBulkTransferAsync` | `POST /api/v2/disbursements/batch` | **Implemented** | Response includes an undocumented `transactionBatchReference` field; automatic retry disabled |
| `IMonnifyDisbursementsClient.AuthorizeBulkTransferAsync` | `POST /api/v2/disbursements/batch/validate-otp` | **Implemented** | Body field is `reference`, not `batchReference`, despite this being the batch endpoint |
| `IMonnifyDisbursementsClient.ResendBulkTransferOtpAsync` | `POST /api/v2/disbursements/batch/resend-otp` | **Implemented** | Body field is `batchReference` here (unlike validate-otp above) |
| `IMonnifyDisbursementsClient.GetBulkTransferSummaryAsync` | `GET /api/v2/disbursements/batch/summary?reference=` | **Implemented** | |
| `IMonnifyDisbursementsClient.GetBulkTransferTransactionsAsync` | `GET /api/v2/disbursements/bulk/{batchReference}/transactions?pageSize=&pageNo=` | **Implemented** | |
| `IMonnifyDisbursementsClient.GetBulkTransfersAsync` | `GET /api/v2/disbursements/bulk?sourceAccountNumber=&pageNo=&pageSize=` | **Implemented** | Docs show a `/transactions` suffix on this path; that 404s — omit it |
| `IMonnifyDisbursementsClient.SearchTransactionsAsync` | `GET /api/v2/disbursements/search-transactions?sourceAccountNumber=&...` | **Implemented** | |
| `IMonnifyDisbursementsClient.GetWalletBalanceAsync` | `GET /api/v2/disbursements/wallet-balance?accountNumber=` | **Implemented** | Accepts both numeric and quoted-string balances via `AllowReadingFromString` |
| `IMonnifyVerificationClient.ValidateAccountNumberAsync` | `GET /api/v1/disbursements/account/validate?accountNumber=&bankCode=` | **Implemented** | Free on both sandbox and live |
| `IMonnifyVerificationClient.MatchBvnDetailsAsync` | `POST /api/v1/vas/bvn-details-match` | **Implemented** | Bills the merchant wallet per request |
| `IMonnifyVerificationClient.MatchBvnToAccountAsync` | `POST /api/v1/vas/bvn-account-match` | **Implemented** | Bills the merchant wallet per request |
| `IMonnifyVerificationClient.VerifyNinAsync` | `POST /api/v1/vas/nin-details` | **Implemented** | Live environment only; bills the merchant wallet per request |
| `IMonnifyBanksClient.GetBanksAsync` | `GET /api/v1/banks` | **Implemented** | Most entries have null USSD fields |
| `IMonnifyBanksClient.GetUssdEnabledBanksAsync` | `GET /api/v1/sdk/transactions/banks` | **Implemented** | Not every entry has a populated `ussdTemplate` (e.g. Suntrust Bank) |
| `MonnifyWebhookValidator.IsValid` / `ComputeSignature` | `monnify-signature` header | **Implemented** | `HMAC-SHA512(key: secretKey, message: rawRequestBody)`, lowercase hex, over the *compact* (not pretty-printed) JSON body. Our docs prose describes plain SHA-512 — HMAC is correct. Sandbox sends no signature header at all, so `IsValid` is always `false` there by design |
| `MonnifyWebhookParser.Parse` / `ParseEventData` / `ParseMetaData` | n/a (parses the request body) | **Implemented** | 11 classes cover 12 event types (disbursement and refund status events share one class each, differentiated by status). Handles real shape inconsistencies: `paymentSourceInformation` as array/empty-object/single-object (`SingleOrArrayJsonConverter`); amount fields as string or number; rejected-payment's `created_on` (snake_case); wallet-activity's `metaData` as an envelope sibling, not nested |
| `HttpRequest.ValidateMonnifyWebhookAsync` | n/a (ASP.NET Core convenience) | **Implemented** | Reads the body once and doesn't reset its position — for a dedicated webhook endpoint only |
| `IMonnifyBillsClient.GetBillerCategoriesAsync` | `GET /api/v1/vas/bills-payment/biller-categories?page=&size=` | **Implemented** | Paging is 0-based; our docs incorrectly show a 1-based default of `page=1`. Real default `size` is `20`, not `10` |
| `IMonnifyBillsClient.GetBillersAsync` | `GET /api/v1/vas/bills-payment/billers?page=&size=&category_code=` | **Implemented** | Same 0-based paging note as above |
| `IMonnifyBillsClient.GetBillerProductsAsync` | `GET /api/v1/vas/bills-payment/biller-products?biller_code=&page=&size=` | **Implemented** | `category`/`biller` are singular objects, not the arrays our docs show. `biller_code` accepts either a plain code (`AIRTEL`) or a `biller-`-prefixed slug (`biller-dstv`) |
| `IMonnifyBillsClient.ValidateCustomerAsync` | `POST /api/v1/vas/bills-payment/validate-customer` | **Implemented** | `validationReference` lives inside `vendInstruction`, not top-level as our docs prose implies. `minAmount`/`maxAmount` can appear top-level for amount-constrained products |
| `IMonnifyBillsClient.VendAsync` | `POST /api/v1/vas/bills-payment/vend` | **Implemented** | Automatic retry disabled, same reasoning as Disbursements |
| `IMonnifyBillsClient.RequeryAsync` | `GET /api/v1/vas/bills-payment/requery?reference=` | **Implemented** | Same response shape as `VendAsync` |
| `IMonnifyCardsClient.ChargeAsync` | `POST /api/v1/merchant/cards/charge` | **Implemented** | Requires a `transactionReference` from `InitializeTransactionAsync` first. Automatic retry disabled - this directly debits a card |
| `IMonnifyCardsClient.AuthorizeOtpAsync` | `POST /api/v1/merchant/cards/otp/authorize` | **Implemented** | Not sandbox-verified - blocked on the `ChargeAsync` bin issue above, since this needs a real `tokenId` from a successful OTP-required charge |
| `IMonnifyCardsClient.Authorize3dsAsync` | `POST /api/v1/sdk/cards/secure-3d/authorize` | **Implemented** | Restricted to PCI DSS-certified merchants, requires separate activation. Calling it without a prior successful charge correctly returned `"No Card Payment found for this transaction."` rather than an auth/activation error, so the endpoint itself is reachable on this account; the OTP-required response shape is otherwise unverified for the same reason as `AuthorizeOtpAsync` |

Update this table whenever a method moves from Planned to Implemented, or its
endpoint changes.
