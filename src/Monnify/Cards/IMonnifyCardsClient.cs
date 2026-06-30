using Monnify.Http;

namespace Monnify.Cards;

/// <summary>
/// Direct card charges (raw PAN/CVV/PIN) against an already-initialized transaction, as an
/// alternative to the hosted checkout flow in <see cref="Collections.IMonnifyCollectionsClient"/>.
/// </summary>
/// <remarks>
/// This client is registered with automatic HTTP retry disabled, for the same reason as
/// <see cref="Disbursements.IMonnifyDisbursementsClient"/> and <see cref="Bills.IMonnifyBillsClient"/>:
/// <see cref="ChargeAsync"/> directly attempts to debit a card, so an ambiguous failure (timeout,
/// network error, 5xx) must not be blindly resent with the same details - that risks a double
/// charge. Query the transaction's status via
/// <see cref="Collections.IMonnifyCollectionsClient.GetTransactionAsync"/> with the same
/// <c>transactionReference</c> instead.
/// </remarks>
public interface IMonnifyCardsClient
{
    /// <summary>
    /// Charges a card against a transaction reference obtained from
    /// <see cref="Collections.IMonnifyCollectionsClient.InitializeTransactionAsync"/>. Depending on
    /// the card, the result is either immediately final, or requires a follow-up call to
    /// <see cref="AuthorizeOtpAsync"/> (OTP) or <see cref="Authorize3dsAsync"/> (3DS) to complete.
    /// </summary>
    Task<ChargeCardResult> ChargeAsync(ChargeCardRequest request, CancellationToken cancellationToken = default);

    /// <summary>Completes a charge that required an OTP, using the token ID from <see cref="ChargeAsync"/>'s response.</summary>
    Task<AuthorizeCardOtpResult> AuthorizeOtpAsync(AuthorizeCardOtpRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Completes a charge on a card that uses 3D Secure authentication. This is not active by
    /// default - it's restricted to PCI DSS-certified merchants and requires it to be enabled for
    /// your account first.
    /// </summary>
    Task<AuthorizeCardOtpResult> Authorize3dsAsync(Authorize3dsCardRequest request, CancellationToken cancellationToken = default);
}
