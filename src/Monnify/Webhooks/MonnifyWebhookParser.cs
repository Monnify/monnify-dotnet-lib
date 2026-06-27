using System.Text.Json;
using Monnify.Exceptions;
using Monnify.Http;

namespace Monnify.Webhooks;

/// <summary>Parses a Monnify webhook request body into its envelope and typed event data.</summary>
public static class MonnifyWebhookParser
{
    /// <summary>
    /// Parses the envelope out of a webhook request body. Validate the signature first with
    /// <see cref="MonnifyWebhookValidator"/> - this does not verify authenticity on its own.
    /// </summary>
    public static MonnifyWebhookEnvelope Parse(string rawRequestBody)
    {
        if (string.IsNullOrWhiteSpace(rawRequestBody))
        {
            throw new ArgumentException("Value must be provided.", nameof(rawRequestBody));
        }

        try
        {
            return JsonSerializer.Deserialize<MonnifyWebhookEnvelope>(rawRequestBody, MonnifyJsonOptions.Default)
                   ?? throw new MonnifyDeserializationException(
                       "Monnify webhook body parsed to null.", rawRequestBody, new InvalidOperationException());
        }
        catch (JsonException ex)
        {
            throw new MonnifyDeserializationException("Failed to parse the Monnify webhook request body.", rawRequestBody, ex);
        }
    }

    /// <summary>
    /// Deserializes <see cref="MonnifyWebhookEnvelope.EventData"/> into the typed event class
    /// matching <see cref="MonnifyWebhookEnvelope.EventType"/> (see <see cref="MonnifyWebhookEventTypes"/>
    /// for which class goes with which event type).
    /// </summary>
    public static TEventData ParseEventData<TEventData>(MonnifyWebhookEnvelope envelope)
    {
        if (envelope is null)
        {
            throw new ArgumentNullException(nameof(envelope));
        }

        try
        {
            return envelope.EventData.Deserialize<TEventData>(MonnifyJsonOptions.Default)
                   ?? throw new MonnifyDeserializationException(
                       $"Monnify webhook eventData parsed to null for {typeof(TEventData).Name}.",
                       envelope.EventData.GetRawText(), new InvalidOperationException());
        }
        catch (JsonException ex)
        {
            throw new MonnifyDeserializationException(
                $"Failed to parse Monnify webhook eventData as {typeof(TEventData).Name}.", envelope.EventData.GetRawText(), ex);
        }
    }

    /// <summary>
    /// Deserializes <see cref="MonnifyWebhookEnvelope.MetaData"/>, present only on some event types
    /// (e.g. <see cref="MonnifyWebhookEventTypes.AccountActivity"/>). Returns <see langword="null"/>
    /// if the envelope had no <c>metaData</c> field.
    /// </summary>
    public static TMetaData? ParseMetaData<TMetaData>(MonnifyWebhookEnvelope envelope)
    {
        if (envelope is null)
        {
            throw new ArgumentNullException(nameof(envelope));
        }

        if (envelope.MetaData is null)
        {
            return default;
        }

        try
        {
            return envelope.MetaData.Value.Deserialize<TMetaData>(MonnifyJsonOptions.Default);
        }
        catch (JsonException ex)
        {
            throw new MonnifyDeserializationException(
                $"Failed to parse Monnify webhook metaData as {typeof(TMetaData).Name}.", envelope.MetaData.Value.GetRawText(), ex);
        }
    }
}
