using Microsoft.Extensions.DependencyInjection;
using Monnify.Collections;

namespace Monnify.IntegrationTests.Collections;

[Trait("Category", "Sandbox")]
public class CollectionsSandboxTests : IClassFixture<SandboxClientFixture>
{
    private readonly SandboxClientFixture _fixture;

    public CollectionsSandboxTests(SandboxClientFixture fixture) => _fixture = fixture;

    private static bool CanRun => SandboxCredentials.IsAvailable && SandboxContractCode.IsAvailable;

    [SkippableFact]
    public async Task InitializeTransactionAsync_AgainstRealSandbox_ReturnsCheckoutUrl()
    {
        Skip.IfNot(CanRun, "Sandbox credentials/contract code are not set.");

        var client = _fixture.Provider.GetRequiredService<IMonnifyCollectionsClient>();
        var reference = $"it-txn-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

        var result = await client.InitializeTransactionAsync(new InitializeTransactionRequest
        {
            Amount = 100,
            CustomerName = "Ada Lovelace",
            CustomerEmail = "ada@example.com",
            PaymentReference = reference,
            PaymentDescription = "Integration test payment",
            ContractCode = SandboxContractCode.Value!,
            RedirectUrl = "https://example.com/callback",
        });

        Assert.False(string.IsNullOrWhiteSpace(result.CheckoutUrl));
        Assert.False(string.IsNullOrWhiteSpace(result.TransactionReference));
    }

    [SkippableFact]
    public async Task ReservedAccountLifecycle_AgainstRealSandbox_CreateGetListDelete()
    {
        Skip.IfNot(CanRun, "Sandbox credentials/contract code are not set.");

        var client = _fixture.Provider.GetRequiredService<IMonnifyCollectionsClient>();
        var reference = $"it-acct-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

        var created = await client.CreateReservedAccountAsync(new CreateReservedAccountRequest
        {
            AccountReference = reference,
            AccountName = "Integration Test Account",
            ContractCode = SandboxContractCode.Value!,
            CustomerEmail = "integration-test@example.com",
            CustomerName = "Integration Test",
            GetAllAvailableBanks = true,
        });
        Assert.NotEmpty(created.Accounts);

        var fetched = await client.GetReservedAccountAsync(reference);
        Assert.Equal(reference, fetched.AccountReference);

        var transactions = await client.GetReservedAccountTransactionsAsync(reference);
        Assert.NotNull(transactions.Content);

        var deleted = await client.DeleteReservedAccountAsync(reference);
        Assert.Equal(reference, deleted.AccountReference);
    }

    [SkippableFact]
    public async Task InvoiceLifecycle_AgainstRealSandbox_CreateGetListCancel()
    {
        Skip.IfNot(CanRun, "Sandbox credentials/contract code are not set.");

        var client = _fixture.Provider.GetRequiredService<IMonnifyCollectionsClient>();
        var reference = $"it-inv-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        var expiry = DateTimeOffset.UtcNow.AddDays(7).ToString("yyyy-MM-dd HH:mm:ss");

        var created = await client.CreateInvoiceAsync(new CreateInvoiceRequest
        {
            Amount = 999,
            InvoiceReference = reference,
            CustomerName = "Integration Test",
            CustomerEmail = "integration-test@example.com",
            ContractCode = SandboxContractCode.Value!,
            Description = "Integration test invoice",
            ExpiryDate = expiry,
        });
        Assert.Equal("PENDING", created.InvoiceStatus);

        var fetched = await client.GetInvoiceAsync(reference);
        Assert.Equal(reference, fetched.InvoiceReference);

        var page = await client.GetInvoicesAsync(page: 0, size: 5);
        Assert.NotEmpty(page.Content);

        var cancelled = await client.CancelInvoiceAsync(reference);
        Assert.Equal("CANCELLED", cancelled.InvoiceStatus);
    }
}
