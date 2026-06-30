using Microsoft.Extensions.DependencyInjection;
using Monnify.Disbursements;
using Monnify.Verification;

namespace Monnify.IntegrationTests.Disbursements;

[Trait("Category", "Sandbox")]
public class DisbursementsSandboxTests : IClassFixture<SandboxClientFixture>
{
    private readonly SandboxClientFixture _fixture;

    public DisbursementsSandboxTests(SandboxClientFixture fixture) => _fixture = fixture;

    private static bool CanRun =>
        SandboxCredentials.IsAvailable && SandboxDisbursementWallet.IsAvailable && SandboxDisbursementDestination.IsAvailable;

    private async Task<string> ResolveDestinationAccountNameAsync()
    {
        var verification = _fixture.Provider.GetRequiredService<IMonnifyVerificationClient>();
        var result = await verification.ValidateAccountNumberAsync(
            SandboxDisbursementDestination.AccountNumber!, SandboxDisbursementDestination.BankCode!);
        return result.AccountName;
    }

    [SkippableFact]
    public async Task SingleTransferLifecycle_AgainstRealSandbox_InitiateThenGetStatus()
    {
        Skip.IfNot(CanRun, "Sandbox credentials/disbursement wallet/destination are not set.");

        var client = _fixture.Provider.GetRequiredService<IMonnifyDisbursementsClient>();
        var reference = $"it-disb-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        var destinationAccountName = await ResolveDestinationAccountNameAsync();

        var initiated = await client.InitiateSingleTransferAsync(new SingleTransferRequest
        {
            Amount = 100,
            Reference = reference,
            Narration = "Integration test disbursement",
            DestinationBankCode = SandboxDisbursementDestination.BankCode!,
            DestinationAccountNumber = SandboxDisbursementDestination.AccountNumber!,
            DestinationAccountName = destinationAccountName,
            SourceAccountNumber = SandboxDisbursementWallet.Value!,
        });
        Assert.Equal(reference, initiated.Reference);
        Assert.False(string.IsNullOrWhiteSpace(initiated.Status));

        var fetched = await client.GetSingleTransferAsync(reference);
        Assert.Equal(reference, fetched.Reference);

        var page = await client.GetSingleTransfersAsync(pageNo: 0, pageSize: 3);
        Assert.NotEmpty(page.Content);
    }

    [SkippableFact]
    public async Task BulkTransferLifecycle_AgainstRealSandbox_InitiateThenGetSummaryAndTransactions()
    {
        Skip.IfNot(CanRun, "Sandbox credentials/disbursement wallet/destination are not set.");

        var client = _fixture.Provider.GetRequiredService<IMonnifyDisbursementsClient>();
        var batchReference = $"it-disb-batch-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        var destinationAccountName = await ResolveDestinationAccountNameAsync();

        var initiated = await client.InitiateBulkTransferAsync(new BulkTransferRequest
        {
            Title = "Integration test batch",
            BatchReference = batchReference,
            Narration = "Integration test batch",
            SourceAccountNumber = SandboxDisbursementWallet.Value!,
            OnValidationFailure = "CONTINUE",
            NotificationInterval = 25,
            TransactionList = new[]
            {
                new BulkTransferItem
                {
                    Amount = 50,
                    Reference = $"{batchReference}-item-1",
                    Narration = "Integration test batch item",
                    DestinationBankCode = SandboxDisbursementDestination.BankCode!,
                    DestinationAccountNumber = SandboxDisbursementDestination.AccountNumber!,
                    DestinationAccountName = destinationAccountName,
                },
            },
        });
        Assert.Equal(batchReference, initiated.BatchReference);
        Assert.Equal(1, initiated.TotalTransactionsCount);

        var summary = await client.GetBulkTransferSummaryAsync(batchReference);
        Assert.Equal(batchReference, summary.BatchReference);

        var transactions = await client.GetBulkTransferTransactionsAsync(batchReference);
        Assert.NotEmpty(transactions.Content);

        var allBatches = await client.GetBulkTransfersAsync(SandboxDisbursementWallet.Value, pageNo: 0, pageSize: 3);
        Assert.NotEmpty(allBatches.Content);
    }

    [SkippableFact]
    public async Task SharedEndpoints_AgainstRealSandbox_WalletBalanceAndSearch()
    {
        Skip.IfNot(SandboxCredentials.IsAvailable && SandboxDisbursementWallet.IsAvailable, "Sandbox credentials/disbursement wallet are not set.");

        var client = _fixture.Provider.GetRequiredService<IMonnifyDisbursementsClient>();

        var balance = await client.GetWalletBalanceAsync(SandboxDisbursementWallet.Value!);
        Assert.True(balance.AvailableBalance >= 0);

        var page = await client.SearchTransactionsAsync(SandboxDisbursementWallet.Value!, pageNo: 0, pageSize: 3);
        Assert.NotNull(page.Content);
    }

    [SkippableFact]
    public async Task CustomerWallets_AgainstRealSandbox_ListGetBalanceAndTransactions()
    {
        Skip.IfNot(SandboxCredentials.IsAvailable, "Sandbox credentials are not set.");

        var client = _fixture.Provider.GetRequiredService<IMonnifyDisbursementsClient>();

        var page = await client.GetWalletsAsync(pageNo: 0, pageSize: 5);
        Assert.NotNull(page.Content);
        Assert.True(page.TotalElements > 0, "Expected at least one customer wallet in the sandbox.");

        var wallet = page.Content[0];
        Assert.False(string.IsNullOrWhiteSpace(wallet.WalletReference));
        Assert.False(string.IsNullOrWhiteSpace(wallet.AccountNumber));

        var balance = await client.GetCustomerWalletBalanceAsync(wallet.AccountNumber);
        Assert.True(balance.AvailableBalance >= 0);

        var transactions = await client.GetWalletTransactionsAsync(wallet.AccountNumber, pageNo: 0, pageSize: 5);
        Assert.NotNull(transactions.Content);
    }
}
