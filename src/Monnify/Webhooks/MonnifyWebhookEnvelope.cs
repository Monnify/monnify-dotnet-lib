using System.Text.Json;
using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

/// <summary>
/// The outer shape of every Monnify webhook request: an <see cref="EventType"/> identifying what
/// happened, and <see cref="EventData"/> holding the event-specific payload. Use
/// <see cref="MonnifyWebhookParser.ParseEventData{TEventData}"/> to deserialize <see cref="EventData"/>
/// into the typed event class matching <see cref="EventType"/> (see <see cref="MonnifyWebhookEventTypes"/>).
/// </summary>
public sealed class MonnifyWebhookEnvelope
{
    [JsonPropertyName("eventType")]
    public string EventType { get; set; } = string.Empty;

    [JsonPropertyName("eventData")]
    public JsonElement EventData { get; set; }

    /// <summary>
    /// Present only on some event types (currently only <see cref="MonnifyWebhookEventTypes.AccountActivity"/>),
    /// where Monnify puts this field next to <see cref="EventData"/> rather than inside it.
    /// </summary>
    [JsonPropertyName("metaData")]
    public JsonElement? MetaData { get; set; }
}
