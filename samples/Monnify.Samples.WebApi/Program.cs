// Demonstrates the webhook receiver pattern: verify the signature, parse the envelope, dispatch
// on event type, and respond 200 immediately - matching our own best-practice guidance to
// acknowledge fast and do any heavy processing afterward rather than making Monnify wait on it.
//
// Before running, set these (e.g. via user secrets or the Monnify__ApiKey/Monnify__SecretKey
// environment variables): Monnify:ApiKey, Monnify:SecretKey.

using Monnify;
using Monnify.Webhooks;

var builder = WebApplication.CreateBuilder(args);

var apiKey = builder.Configuration["Monnify:ApiKey"]
    ?? throw new InvalidOperationException("Set Monnify:ApiKey (e.g. via the Monnify__ApiKey environment variable).");
var secretKey = builder.Configuration["Monnify:SecretKey"]
    ?? throw new InvalidOperationException("Set Monnify:SecretKey (e.g. via the Monnify__SecretKey environment variable).");

builder.Services.AddMonnify(options =>
{
    options.ApiKey = apiKey;
    options.SecretKey = secretKey;
    options.Environment = MonnifyEnvironment.Sandbox;
});

var app = builder.Build();

app.MapGet("/", () => "Monnify.Samples.WebApi is running.");

app.MapPost("/webhooks/monnify", async (HttpRequest request, ILogger<Program> logger) =>
{
    var validation = await request.ValidateMonnifyWebhookAsync(secretKey);
    if (!validation.IsValid)
    {
        // Always the case against sandbox, which sends no monnify-signature header at all - see
        // the remarks on MonnifyWebhookValidator. Reject in production; if you want to exercise
        // this handler against sandbox, branch on your own environment in your own app instead of
        // expecting the validator to skip itself.
        logger.LogWarning("Rejected a webhook with an invalid or missing signature.");
        return Results.Unauthorized();
    }

    var envelope = validation.GetEnvelope();

    switch (envelope.EventType)
    {
        case MonnifyWebhookEventTypes.SuccessfulTransaction:
            var collection = MonnifyWebhookParser.ParseEventData<CollectionTransactionEventData>(envelope);
            logger.LogInformation("Collection {Reference} paid: {Amount} {Currency}",
                collection.TransactionReference, collection.AmountPaid, collection.Currency);
            break;

        case MonnifyWebhookEventTypes.SuccessfulDisbursement:
        case MonnifyWebhookEventTypes.FailedDisbursement:
        case MonnifyWebhookEventTypes.ReversedDisbursement:
            var disbursement = MonnifyWebhookParser.ParseEventData<DisbursementStatusEventData>(envelope);
            logger.LogInformation("Disbursement {Reference} status: {Status}",
                disbursement.Reference, disbursement.Status);
            break;

        default:
            logger.LogInformation("Received an unhandled Monnify webhook event type: {EventType}", envelope.EventType);
            break;
    }

    // Acknowledge immediately; do any slow follow-up work (database writes, notifications,
    // re-vending, etc.) out of band rather than in this handler.
    return Results.Ok();
});

app.Run();
