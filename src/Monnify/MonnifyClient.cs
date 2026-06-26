using Monnify.Banks;
using Monnify.Verification;

namespace Monnify;

/// <summary>
/// Convenience facade aggregating every Monnify typed client. Resolve this from DI if you want
/// one entry point; or inject an individual client interface (e.g. <see cref="IMonnifyBanksClient"/>)
/// directly if you only need one category — both are registered and work independently.
/// </summary>
public sealed class MonnifyClient
{
    public MonnifyClient(IMonnifyBanksClient banks, IMonnifyVerificationClient verification)
    {
        Banks = banks;
        Verification = verification;
    }

    public IMonnifyBanksClient Banks { get; }

    public IMonnifyVerificationClient Verification { get; }
}
