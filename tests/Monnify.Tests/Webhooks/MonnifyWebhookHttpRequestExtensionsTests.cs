using System.Text;
using Microsoft.AspNetCore.Http;
using Monnify.Webhooks;

namespace Monnify.Tests.Webhooks;

public class MonnifyWebhookHttpRequestExtensionsTests
{
    private const string SecretKey = "91MUDL9N6U3BQRXBQ2PJ9M0PW4J22M1Y";
    private const string Body = """{"eventType":"SUCCESSFUL_TRANSACTION","eventData":{"transactionReference":"abc-123"}}""";

    private static DefaultHttpContext BuildContext(string body, string? signatureHeader)
    {
        var context = new DefaultHttpContext();
        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
        if (signatureHeader is not null)
        {
            context.Request.Headers["monnify-signature"] = signatureHeader;
        }

        return context;
    }

    [Fact]
    public async Task ValidateMonnifyWebhookAsync_CorrectSignature_ReturnsValidResult()
    {
        var signature = MonnifyWebhookValidator.ComputeSignature(Body, SecretKey);
        var context = BuildContext(Body, signature);

        var result = await context.Request.ValidateMonnifyWebhookAsync(SecretKey);

        Assert.True(result.IsValid);
        Assert.Equal(Body, result.RawBody);
        Assert.Equal("SUCCESSFUL_TRANSACTION", result.GetEnvelope().EventType);
    }

    [Fact]
    public async Task ValidateMonnifyWebhookAsync_MissingSignatureHeader_ReturnsInvalidResult_NotThrow()
    {
        var context = BuildContext(Body, signatureHeader: null);

        var result = await context.Request.ValidateMonnifyWebhookAsync(SecretKey);

        Assert.False(result.IsValid);
        Assert.Equal(Body, result.RawBody);
    }

    [Fact]
    public async Task ValidateMonnifyWebhookAsync_WrongSignature_ReturnsInvalidResult()
    {
        var context = BuildContext(Body, signatureHeader: "not-the-real-signature");

        var result = await context.Request.ValidateMonnifyWebhookAsync(SecretKey);

        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task ValidateMonnifyWebhookAsync_NullSecretKey_Throws()
    {
        var context = BuildContext(Body, signatureHeader: null);

        await Assert.ThrowsAsync<ArgumentException>(() => context.Request.ValidateMonnifyWebhookAsync(""));
    }
}
