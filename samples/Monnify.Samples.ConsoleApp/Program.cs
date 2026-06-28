// Demonstrates wiring up the Monnify SDK outside ASP.NET Core (a worker service, a CLI tool,
// a console job, etc.) via the generic host, then calling a couple of typed clients.
//
// Before running, set these environment variables to your own sandbox credentials:
//   MONNIFY_API_KEY, MONNIFY_SECRET_KEY, MONNIFY_CONTRACT_CODE
// (find ApiKey/SecretKey/ContractCode on your Monnify dashboard under Settings > API Keys)

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Monnify;
using Monnify.Banks;
using Monnify.Collections;
using Monnify.Exceptions;

var builder = Host.CreateApplicationBuilder(args);

var apiKey = Environment.GetEnvironmentVariable("MONNIFY_API_KEY")
    ?? throw new InvalidOperationException("Set the MONNIFY_API_KEY environment variable.");
var secretKey = Environment.GetEnvironmentVariable("MONNIFY_SECRET_KEY")
    ?? throw new InvalidOperationException("Set the MONNIFY_SECRET_KEY environment variable.");
var contractCode = Environment.GetEnvironmentVariable("MONNIFY_CONTRACT_CODE")
    ?? throw new InvalidOperationException("Set the MONNIFY_CONTRACT_CODE environment variable.");

builder.Services.AddMonnify(options =>
{
    options.ApiKey = apiKey;
    options.SecretKey = secretKey;
    options.Environment = MonnifyEnvironment.Sandbox;
});

using var host = builder.Build();

var banks = host.Services.GetRequiredService<IMonnifyBanksClient>();
var collections = host.Services.GetRequiredService<IMonnifyCollectionsClient>();

try
{
    var allBanks = await banks.GetBanksAsync();
    Console.WriteLine($"Monnify knows about {allBanks.Count} banks. First five:");
    foreach (var bank in allBanks.Take(5))
    {
        Console.WriteLine($"  {bank.Code} - {bank.Name}");
    }

    var checkout = await collections.InitializeTransactionAsync(new InitializeTransactionRequest
    {
        Amount = 1000,
        CustomerName = "Jane Doe",
        CustomerEmail = "jane.doe@example.com",
        PaymentReference = $"sample-{Guid.NewGuid():N}",
        PaymentDescription = "Sample checkout from Monnify.Samples.ConsoleApp",
        ContractCode = contractCode,
    });

    Console.WriteLine();
    Console.WriteLine($"Checkout initialized: {checkout.TransactionReference}");
    Console.WriteLine($"Redirect the customer to: {checkout.CheckoutUrl}");
}
catch (MonnifyApiException ex)
{
    // Loud by default: Monnify rejected the request, and ex carries enough to act on it
    // (ResponseCode/ResponseMessage for logic, RawResponseBody if you need to see everything).
    Console.WriteLine($"Monnify rejected the request: {ex.ResponseCode} - {ex.ResponseMessage}");
}
