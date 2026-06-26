using Monnify.Authentication;

namespace Monnify.Tests.TestUtilities;

/// <summary>Hands out tokens from a fixed queue, one per call, repeating the last once exhausted.</summary>
internal sealed class SequentialFakeTokenProvider : IMonnifyTokenProvider
{
    private readonly Queue<string> _tokens;
    private string _last = string.Empty;

    public SequentialFakeTokenProvider(params string[] tokens) => _tokens = new Queue<string>(tokens);

    public int InvalidateCount { get; private set; }

    public ValueTask<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        _last = _tokens.Count > 0 ? _tokens.Dequeue() : _last;
        return new ValueTask<string>(_last);
    }

    public void Invalidate() => InvalidateCount++;
}
