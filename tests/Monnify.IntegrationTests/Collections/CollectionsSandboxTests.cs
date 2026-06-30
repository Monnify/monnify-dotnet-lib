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

    [SkippableFact]
    public async Task LimitProfileLifecycle_AgainstRealSandbox_CreateListUpdateAndApplyToReservedAccount()
    {
        Skip.IfNot(CanRun, "Sandbox credentials/contract code are not set.");

        var client = _fixture.Provider.GetRequiredService<IMonnifyCollectionsClient>();
        var profileName = $"it-profile-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

        var created = await client.CreateLimitProfileAsync(new CreateLimitProfileRequest
        {
            LimitProfileName = profileName,
            SingleTransactionValue = 5000m,
            DailyTransactionValue = 50000m,
            DailyTransactionVolume = 10,
        });
        Assert.False(string.IsNullOrWhiteSpace(created.LimitProfileCode));
        Assert.Equal(profileName, created.LimitProfileName);
        var profileCode = created.LimitProfileCode;

        var page = await client.GetLimitProfilesAsync(page: 0, size: 50);
        Assert.Contains(page.Content, p => p.LimitProfileCode == profileCode);

        var updated = await client.UpdateLimitProfileAsync(profileCode, new UpdateLimitProfileRequest
        {
            LimitProfileName = profileName,
            SingleTransactionValue = 10000m,
            DailyTransactionValue = 100000m,
            DailyTransactionVolume = 20,
        });
        Assert.Equal(profileCode, updated.LimitProfileCode);

        var accountReference = $"it-limit-acct-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        var reservedWithLimit = await client.CreateReservedAccountWithLimitAsync(new CreateReservedAccountWithLimitRequest
        {
            ContractCode = SandboxContractCode.Value!,
            AccountName = "Integration Test Limit Account",
            AccountReference = accountReference,
            CustomerEmail = "integration-test@example.com",
            CustomerName = "Integration Test",
            GetAllAvailableBanks = true,
            LimitProfileCode = profileCode,
        });
        Assert.Equal(accountReference, reservedWithLimit.AccountReference);

        var updatedLimit = await client.UpdateReservedAccountLimitAsync(new UpdateReservedAccountLimitRequest
        {
            AccountReference = accountReference,
            LimitProfileCode = profileCode,
        });
        Assert.Equal(accountReference, updatedLimit.AccountReference);

        await client.DeleteReservedAccountAsync(accountReference);
    }

    [SkippableFact]
    public async Task SubAccountLifecycle_AgainstRealSandbox_CreateGetUpdateDelete()
    {
        Skip.IfNot(CanRun, "Sandbox credentials/contract code are not set.");
        Skip.IfNot(SandboxSubAccountDestination.IsAvailable,
            "MONNIFY_SANDBOX_SUBACCOUNT_BANK_CODE / MONNIFY_SANDBOX_SUBACCOUNT_ACCOUNT are not set.");

        var client = _fixture.Provider.GetRequiredService<IMonnifyCollectionsClient>();

        var created = await client.CreateSubAccountsAsync(
        [
            new CreateSubAccountRequest
            {
                CurrencyCode = "NGN",
                AccountNumber = SandboxSubAccountDestination.AccountNumber!,
                BankCode = SandboxSubAccountDestination.BankCode!,
                Email = "integration-test-subaccount@example.com",
                DefaultSplitPercentage = 10m,
            },
        ]);
        Assert.NotEmpty(created);
        var subAccountCode = created[0].SubAccountCode;
        Assert.False(string.IsNullOrWhiteSpace(subAccountCode));

        var all = await client.GetSubAccountsAsync();
        Assert.Contains(all, s => s.SubAccountCode == subAccountCode);

        var updated = await client.UpdateSubAccountAsync(new UpdateSubAccountRequest
        {
            SubAccountCode = subAccountCode,
            CurrencyCode = "NGN",
            AccountNumber = SandboxSubAccountDestination.AccountNumber!,
            BankCode = SandboxSubAccountDestination.BankCode!,
            Email = "integration-test-subaccount-updated@example.com",
            DefaultSplitPercentage = 15m,
        });
        Assert.Equal(subAccountCode, updated.SubAccountCode);

        await client.DeleteSubAccountAsync(subAccountCode);

        var afterDelete = await client.GetSubAccountsAsync();
        Assert.DoesNotContain(afterDelete, s => s.SubAccountCode == subAccountCode);
    }

    [SkippableFact]
    public async Task TransactionLookup_AgainstRealSandbox_InitiateBankTransferSearchGetAndQuery()
    {
        Skip.IfNot(CanRun, "Sandbox credentials/contract code are not set.");

        var client = _fixture.Provider.GetRequiredService<IMonnifyCollectionsClient>();
        var reference = $"it-lookup-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

        var initialized = await client.InitializeTransactionAsync(new InitializeTransactionRequest
        {
            Amount = 100,
            CustomerName = "Ada Lovelace",
            CustomerEmail = "ada@example.com",
            PaymentReference = reference,
            PaymentDescription = "Integration test lookup",
            ContractCode = SandboxContractCode.Value!,
            RedirectUrl = "https://example.com/callback",
        });

        var bankTransfer = await client.InitiateBankTransferAsync(new InitiateBankTransferRequest
        {
            TransactionReference = initialized.TransactionReference,
        });
        Assert.False(string.IsNullOrWhiteSpace(bankTransfer.AccountNumber));

        var searchResults = await client.SearchTransactionsAsync(
            new SearchTransactionsRequest { PaymentReference = reference }, page: 0, size: 5);
        Assert.NotEmpty(searchResults.Content);

        var fetched = await client.GetTransactionAsync(initialized.TransactionReference);
        Assert.Equal(initialized.TransactionReference, fetched.TransactionReference);

        var queried = await client.QueryTransactionAsync(paymentReference: reference);
        Assert.Equal(reference, queried.PaymentReference);
    }
}
