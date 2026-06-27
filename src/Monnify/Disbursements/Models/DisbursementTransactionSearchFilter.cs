namespace Monnify.Disbursements;

/// <summary>Optional filters for <see cref="IMonnifyDisbursementsClient.SearchTransactionsAsync"/>. All fields are optional.</summary>
public sealed class DisbursementTransactionSearchFilter
{
    /// <summary>Start of the date range, as a Unix timestamp.</summary>
    public long? StartDate { get; set; }

    /// <summary>End of the date range, as a Unix timestamp.</summary>
    public long? EndDate { get; set; }

    public decimal? AmountFrom { get; set; }

    public decimal? AmountTo { get; set; }
}
