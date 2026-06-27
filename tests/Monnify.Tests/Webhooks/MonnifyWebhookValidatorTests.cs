using Monnify.Webhooks;

namespace Monnify.Tests.Webhooks;

public class MonnifyWebhookValidatorTests
{
    // From our own webhooks documentation sample (secret key, body, and "Hashed Value").
    // Note: our docs pretty-print the sample body with JSON.stringify(obj, null, 2) before
    // hashing it, but that pretty-printed form does NOT reproduce our own published hash -
    // only the compact form below does. Confirmed by running our exact sample code in Node.js.
    private const string SecretKey = "91MUDL9N6U3BQRXBQ2PJ9M0PW4J22M1Y";

    private const string Body = """
        {"eventData":{"product":{"reference":"111222333","type":"OFFLINE_PAYMENT_AGENT"},"transactionReference":"MNFY|76|20211117154810|000001","paymentReference":"0.01462001097368737","paidOn":"17/11/2021 3:48:10 PM","paymentDescription":"Mockaroo Jesse","metaData":{},"destinationAccountInformation":{},"paymentSourceInformation":{},"amountPaid":78000,"totalPayable":78000,"offlineProductInformation":{"code":"41470","type":"DYNAMIC"},"cardDetails":{},"paymentMethod":"CASH","currency":"NGN","settlementAmount":77600,"paymentStatus":"PAID","customer":{"name":"Mockaroo Jesse","email":"111222333@ZZAMZ4WT4Y3E.monnify"}},"eventType":"SUCCESSFUL_TRANSACTION"}
        """;

    private const string ExpectedSignature =
        "f04fb635e04d71648bd3cc7999003da6861483342c856d05ddfa9b2dafacb873b0de1d0f8f67405d0010b4348b721c49fa171d317972618debba6b638aedcd3c";

    [Fact]
    public void ComputeSignature_MatchesMonnifysDocumentedSample()
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

    [Fact]
    public void IsValid_PrettyPrintedBody_DoesNotMatch_OnlyExactRawBodyDoes()
    {
        // Guards against re-serializing/re-formatting the body before validating - even though
        // it's semantically the same JSON, a different byte representation produces a different
        // signature. This is exactly the mistake in our own docs sample.
        var prettyPrinted = """
            {
              "eventData": {
                "product": { "reference": "111222333", "type": "OFFLINE_PAYMENT_AGENT" }
              },
              "eventType": "SUCCESSFUL_TRANSACTION"
            }
            """;

        Assert.False(MonnifyWebhookValidator.IsValid(prettyPrinted, ExpectedSignature, SecretKey));
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
