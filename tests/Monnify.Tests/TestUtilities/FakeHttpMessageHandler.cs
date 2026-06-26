namespace Monnify.Tests.TestUtilities;

/// <summary>
/// A scriptable <see cref="HttpMessageHandler"/> for unit tests: each call to <c>Enqueue</c>
/// queues one response (or a delegate that inspects the request and returns one), consumed in order.
/// Records every request it sees so tests can assert on path/method/headers/body.
/// </summary>
internal sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<Func<HttpRequestMessage, HttpResponseMessage>> _responses = new();

    public List<HttpRequestMessage> Requests { get; } = new();

    /// <summary>
    /// The request body captured at the moment each request arrived. Handlers (like
    /// <c>MonnifyAuthHandler</c>) may dispose a request's content once they're done with it, so
    /// asserting on <see cref="Requests"/>[i].Content after the fact can throw
    /// <see cref="ObjectDisposedException"/> — read it from here instead.
    /// </summary>
    public List<string?> RequestBodies { get; } = new();

    public void Enqueue(HttpResponseMessage response) => Enqueue(_ => response);

    public void Enqueue(Func<HttpRequestMessage, HttpResponseMessage> responseFactory) => _responses.Enqueue(responseFactory);

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Requests.Add(request);
        RequestBodies.Add(request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));

        if (_responses.Count == 0)
        {
            throw new InvalidOperationException("FakeHttpMessageHandler received a request with no queued response.");
        }

        return _responses.Dequeue().Invoke(request);
    }
}
