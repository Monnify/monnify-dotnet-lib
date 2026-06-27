using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class InitiateBankTransferRequest
{
    /// <summary>The Monnify transaction reference returned from <see cref="IMonnifyCollectionsClient.InitializeTransactionAsync"/>.</summary>
    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    /// <summary>Restricts the generated account to a single bank, for USSD string generation.</summary>
    [JsonPropertyName("bankCode")]
    public string? BankCode { get; set; }
}
