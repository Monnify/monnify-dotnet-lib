namespace Monnify.Webhooks;

/// <summary>
/// The <c>eventType</c> values Monnify sends in a webhook envelope. Compare
/// <see cref="MonnifyWebhookEnvelope.EventType"/> against these instead of hardcoding the strings.
/// </summary>
public static class MonnifyWebhookEventTypes
{
    /// <summary>A successful collection payment. Use <see cref="CollectionTransactionEventData"/>.</summary>
    public const string SuccessfulTransaction = "SUCCESSFUL_TRANSACTION";

    /// <summary>A disbursement with a successful definite status. Use <see cref="DisbursementStatusEventData"/>.</summary>
    public const string SuccessfulDisbursement = "SUCCESSFUL_DISBURSEMENT";

    /// <summary>A failed disbursement. Use <see cref="DisbursementStatusEventData"/>.</summary>
    public const string FailedDisbursement = "FAILED_DISBURSEMENT";

    /// <summary>A reversed disbursement. Use <see cref="DisbursementStatusEventData"/>.</summary>
    public const string ReversedDisbursement = "REVERSED_DISBURSEMENT";

    /// <summary>A successfully processed refund. Use <see cref="RefundEventData"/>.</summary>
    public const string SuccessfulRefund = "SUCCESSFUL_REFUND";

    /// <summary>A failed refund. Use <see cref="RefundEventData"/>.</summary>
    public const string FailedRefund = "FAILED_REFUND";

    /// <summary>A completed settlement to your bank account or wallet. Use <see cref="SettlementEventData"/>.</summary>
    public const string Settlement = "SETTLEMENT";

    /// <summary>A mandate (direct debit) status change. Use <see cref="MandateStatusEventData"/>.</summary>
    public const string MandateUpdate = "MANDATE_UPDATE";

    /// <summary>A credit or debit on your Monnify wallet. Use <see cref="WalletActivityEventData"/>.</summary>
    public const string AccountActivity = "ACCOUNT_ACTIVITY";

    /// <summary>Your wallet balance dropped below a configured threshold. Use <see cref="LowBalanceAlertEventData"/>.</summary>
    public const string LowBalanceAlert = "LOW_BALANCE_ALERT";

    /// <summary>A payment that was rejected (e.g. an under-payment). Use <see cref="RejectedPaymentEventData"/>.</summary>
    public const string RejectedPayment = "REJECTED_PAYMENT";
}
