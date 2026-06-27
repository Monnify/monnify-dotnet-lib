using Monnify.Exceptions;
using Monnify.Webhooks;

namespace Monnify.Tests.Webhooks;

public class MonnifyWebhookParserTests
{
    [Fact]
    public void Parse_ValidEnvelope_ReturnsEventTypeAndRawEventData()
    {
        var envelope = MonnifyWebhookParser.Parse("""
            { "eventType": "SUCCESSFUL_TRANSACTION", "eventData": { "transactionReference": "abc-123" } }
            """);

        Assert.Equal("SUCCESSFUL_TRANSACTION", envelope.EventType);
        Assert.Equal("abc-123", envelope.EventData.GetProperty("transactionReference").GetString());
        Assert.Null(envelope.MetaData);
    }

    [Fact]
    public void Parse_EnvelopeWithMetaData_ReturnsMetaData()
    {
        var envelope = MonnifyWebhookParser.Parse("""
            { "eventType": "ACCOUNT_ACTIVITY", "eventData": {}, "metaData": { "senderAccount": "Monnify Service" } }
            """);

        Assert.NotNull(envelope.MetaData);
        Assert.Equal("Monnify Service", envelope.MetaData!.Value.GetProperty("senderAccount").GetString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Parse_MissingBody_Throws(string? body)
    {
        Assert.Throws<ArgumentException>(() => MonnifyWebhookParser.Parse(body!));
    }

    [Fact]
    public void Parse_MalformedJson_ThrowsMonnifyDeserializationException_PreservingRawBody()
    {
        var ex = Assert.Throws<MonnifyDeserializationException>(() => MonnifyWebhookParser.Parse("not json"));

        Assert.Equal("not json", ex.RawResponseBody);
    }

    private sealed class SimpleEventData
    {
        public string TransactionReference { get; set; } = string.Empty;
    }

    [Fact]
    public void ParseEventData_ValidShape_Deserializes()
    {
        var envelope = MonnifyWebhookParser.Parse("""
            { "eventType": "SUCCESSFUL_TRANSACTION", "eventData": { "transactionReference": "abc-123" } }
            """);

        var data = MonnifyWebhookParser.ParseEventData<SimpleEventData>(envelope);

        Assert.Equal("abc-123", data.TransactionReference);
    }

    [Fact]
    public void ParseEventData_NullEnvelope_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => MonnifyWebhookParser.ParseEventData<SimpleEventData>(null!));
    }

    private sealed class SimpleMetaData
    {
        public string SenderAccount { get; set; } = string.Empty;
    }

    [Fact]
    public void ParseMetaData_PresentMetaData_Deserializes()
    {
        var envelope = MonnifyWebhookParser.Parse("""
            { "eventType": "ACCOUNT_ACTIVITY", "eventData": {}, "metaData": { "senderAccount": "Monnify Service" } }
            """);

        var metaData = MonnifyWebhookParser.ParseMetaData<SimpleMetaData>(envelope);

        Assert.Equal("Monnify Service", metaData!.SenderAccount);
    }

    [Fact]
    public void ParseMetaData_AbsentMetaData_ReturnsNull()
    {
        var envelope = MonnifyWebhookParser.Parse("""
            { "eventType": "SUCCESSFUL_TRANSACTION", "eventData": {} }
            """);

        var metaData = MonnifyWebhookParser.ParseMetaData<SimpleMetaData>(envelope);

        Assert.Null(metaData);
    }

    [Fact]
    public void ParseMetaData_NullEnvelope_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => MonnifyWebhookParser.ParseMetaData<SimpleMetaData>(null!));
    }
}
