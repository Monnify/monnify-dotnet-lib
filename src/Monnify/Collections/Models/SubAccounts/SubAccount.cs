using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class SubAccount
{
    [JsonPropertyName("subAccountCode")]
    public string SubAccountCode { get; set; } = string.Empty;

    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("bankCode")]
    public string BankCode { get; set; } = string.Empty;

    [JsonPropertyName("bankName")]
    public string BankName { get; set; } = string.Empty;

    [JsonPropertyName("defaultSplitPercentage")]
    public decimal DefaultSplitPercentage { get; set; }

    [JsonPropertyName("settlementProfileCode")]
    public string? SettlementProfileCode { get; set; }

    [JsonPropertyName("settlementReportEmails")]
    public IReadOnlyList<string>? SettlementReportEmails { get; set; }
}
