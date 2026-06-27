using Monnify.Webhooks;

namespace Monnify.Tests.Webhooks;

public class MonnifyWebhookValidatorTests
{
    private const string SecretKey = "test-secret-key";
    private const string Body = """{"eventType":"SUCCESSFUL_TRANSACTION","eventData":{}}""";

    // Computed independently via Python's hashlib (sha512(secretKey + body)), not via this SDK,
    // so this test actually proves the implementation matches the documented scheme.
    private const string ExpectedSignature =
        "3a3f96ea7088a4297e04ba3f80f7e03c7aef24d998b033862100c8c9b5d2961166050a400929a1f1e87619db9060b0ef702d8810623e7050584dc7c082ca49e4";

    [Fact]
    public void ComputeSignature_MatchesIndependentlyComputedSha512()
    {
        var signature = MonnifyWebhookValidator.ComputeSignature(Body, SecretKey);

        Assert.Equal(ExpectedSignature, signature);
    }

    [Fact]
    public void IsValid_CorrectSignature_ReturnsTrue()
    {
        Assert.True(MonnifyWebhookValidator.IsValid(Body, ExpectedSignature, SecretKey));
    }

    [Fact]
    public void IsValid_SignatureInDifferentCase_StillReturnsTrue()
    {
        Assert.True(MonnifyWebhookValidator.IsValid(Body, ExpectedSignature.ToUpperInvariant(), SecretKey));
    }

    [Fact]
    public void IsValid_WrongSecretKey_ReturnsFalse()
    {
        Assert.False(MonnifyWebhookValidator.IsValid(Body, ExpectedSignature, "wrong-secret-key"));
    }

    [Fact]
    public void IsValid_TamperedBody_ReturnsFalse()
    {
        var tamperedBody = Body.Replace("SUCCESSFUL_TRANSACTION", "FAILED_TRANSACTION");

        Assert.False(MonnifyWebhookValidator.IsValid(tamperedBody, ExpectedSignature, SecretKey));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValid_MissingSignatureHeader_ReturnsFalse(string? signatureHeader)
    {
        Assert.False(MonnifyWebhookValidator.IsValid(Body, signatureHeader, SecretKey));
    }

    [Fact]
    public void IsValid_SignatureWrongLength_ReturnsFalse()
    {
        Assert.False(MonnifyWebhookValidator.IsValid(Body, "abc123", SecretKey));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IsValid_MissingRawBody_Throws(string? body)
    {
        Assert.Throws<ArgumentException>(() => MonnifyWebhookValidator.IsValid(body!, ExpectedSignature, SecretKey));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IsValid_MissingSecretKey_Throws(string? secretKey)
    {
        Assert.Throws<ArgumentException>(() => MonnifyWebhookValidator.IsValid(Body, ExpectedSignature, secretKey!));
    }
}
