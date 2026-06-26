namespace Monnify;

/// <summary>
/// Selects which Monnify base URL a <see cref="MonnifyClientOptions"/> resolves to by default.
/// </summary>
public enum MonnifyEnvironment
{
    /// <summary>Monnify's sandbox environment (https://sandbox.monnify.com).</summary>
    Sandbox,

    /// <summary>Monnify's production environment (https://api.monnify.com).</summary>
    Live
}
