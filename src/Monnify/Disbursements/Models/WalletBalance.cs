using System.Text.Json.Serialization;

namespace Monnify.Disbursements;

public sealed class WalletBalance
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("availableBalance")]
    public decimal AvailableBalance { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("ledgerBalance")]
    public decimal LedgerBalance { get; set; }
}
