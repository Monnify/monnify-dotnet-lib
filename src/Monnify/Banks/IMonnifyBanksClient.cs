namespace Monnify.Banks;

public interface IMonnifyBanksClient
{
    /// <summary>Returns every bank Monnify knows about. Most entries have no USSD info populated.</summary>
    Task<IReadOnlyList<Bank>> GetBanksAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns only banks that support USSD, with their USSD templates populated.</summary>
    Task<IReadOnlyList<Bank>> GetUssdEnabledBanksAsync(CancellationToken cancellationToken = default);
}
