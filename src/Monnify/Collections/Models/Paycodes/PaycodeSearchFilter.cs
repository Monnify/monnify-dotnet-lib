namespace Monnify.Collections;

public sealed class PaycodeSearchFilter
{
    public string? TransactionReference { get; set; }
    public string? BeneficiaryName { get; set; }
    public string? TransactionStatus { get; set; }

    /// <summary>Unix timestamp (seconds) for the start of the date range.</summary>
    public long? From { get; set; }

    /// <summary>Unix timestamp (seconds) for the end of the date range.</summary>
    public long? To { get; set; }
}
