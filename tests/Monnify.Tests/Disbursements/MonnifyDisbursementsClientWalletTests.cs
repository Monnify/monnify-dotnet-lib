using System.Net;
using Monnify.Disbursements;
using Monnify.Tests.TestUtilities;

namespace Monnify.Tests.Disbursements;

public class MonnifyDisbursementsClientWalletTests
{
    private static MonnifyDisbursementsClient CreateClient(FakeHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://sandbox.monnify.test") });

    private const string WalletJson = """
        {
          "walletName": "Staging Wallet - ref1684248425966",
          "walletReference": "ref1684248425966",
          "customerName": "John Doe",
          "customerEmail": "johndoe@example.com",
          "feeBearer": "SELF",
          "bvnDetails": { "bvn": "22222222226", "bvnDateOfBirth": "1994-09-07" },
          "accountNumber": "1234567890",
          "accountName": "John Doe",
          "topUpAccountDetails": {
            "accountNumber": "1234567890",
            "accountName": "John Doe",
            "bankCode": "50515",
            "bankName": "Moniepoint MFB",
            "createdOn": "2025-11-12T11:40:25.344+00:00"
          }
        }
        """;

    [Fact]
    public async Task CreateWalletAsync_SendsPostWithJsonBody_AndDeserializesResult()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK,
            $$"""{ "requestSuccessful": true, "responseMessage": "Success", "responseCode": "0", "responseBody": {{WalletJson}} }"""));
        var client = CreateClient(handler);

        var result = await client.CreateWalletAsync(new CreateWalletRequest
        {
            WalletReference = "ref1684248425966",
            WalletName = "Staging Wallet - ref1684248425966",
            CustomerName = "John Doe",
            CustomerEmail = "johndoe@example.com",
            BvnDetails = new BvnDetails { Bvn = "22222222226", BvnDateOfBirth = "1994-09-07" },
        });

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/api/v1/disbursements/wallet", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("\"walletReference\":\"ref1684248425966\"", handler.RequestBodies[0]);
        Assert.Contains("\"bvn\":\"22222222226\"", handler.RequestBodies[0]);
        Assert.Equal("ref1684248425966", result.WalletReference);
        Assert.Equal("SELF", result.FeeBearer);
        Assert.Equal("50515", result.TopUpAccountDetails!.BankCode);
    }

    [Fact]
    public async Task CreateWalletAsync_NullRequest_Throws()
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.CreateWalletAsync(null!));
    }

    [Fact]
    public async Task GetWalletsAsync_WithoutFilter_SendsGetWithOnlyPaging()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, $$"""
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [ {{WalletJson}} ],
                "last": true, "totalElements": 1, "totalPages": 1,
                "first": true, "numberOfElements": 1, "size": 10, "number": 0, "empty": false
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetWalletsAsync(pageNo: 0, pageSize: 10);

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/disbursements/wallet", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("pageNo=0&pageSize=10", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Single(result.Content);
        Assert.Equal("ref1684248425966", result.Content[0].WalletReference);
    }

    [Fact]
    public async Task GetWalletsAsync_WithWalletReference_IncludesFilterInQuery()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, $$"""
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [ {{WalletJson}} ],
                "last": true, "totalElements": 1, "totalPages": 1,
                "first": true, "numberOfElements": 1, "size": 10, "number": 0, "empty": false
              } }
            """));
        var client = CreateClient(handler);

        await client.GetWalletsAsync(walletReference: "ref1684248425966", pageNo: 0, pageSize: 10);

        Assert.Contains("walletReference=ref1684248425966", handler.Requests[0].RequestUri!.Query);
    }

    [Fact]
    public async Task GetCustomerWalletBalanceAsync_SendsGetWithAccountNumberParam()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": { "availableBalance": 5000000000, "ledgerBalance": 5000000000 } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetCustomerWalletBalanceAsync("2611645927");

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/disbursements/wallet/balance", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("accountNumber=2611645927", handler.Requests[0].RequestUri!.Query.TrimStart('?'));
        Assert.Equal(5000000000m, result.AvailableBalance);
        Assert.Equal(5000000000m, result.LedgerBalance);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetCustomerWalletBalanceAsync_MissingAccountNumber_Throws(string? accountNumber)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetCustomerWalletBalanceAsync(accountNumber!));
    }

    [Fact]
    public async Task GetWalletTransactionsAsync_SendsGetWithAccountNumberAndPaging()
    {
        var handler = new FakeHttpMessageHandler();
        handler.Enqueue(HttpResponseFactory.Json(HttpStatusCode.OK, """
            { "requestSuccessful": true, "responseMessage": "success", "responseCode": "0",
              "responseBody": {
                "content": [
                  {
                    "walletTransactionReference": "MFDS60520251111091433348001YWN0D9_DEBIT_0",
                    "monnifyTransactionReference": "MFDS60520251111091433348001YWN0D9_DEBIT_0",
                    "availableBalanceBefore": 5000000000,
                    "availableBalanceAfter": 4900000000,
                    "amount": 1000000000,
                    "transactionDate": "2025-11-11T08:14:33.000+00:00",
                    "transactionType": "DEBIT",
                    "message": null,
                    "narration": "test narration",
                    "status": "COMPLETED"
                  }
                ],
                "last": true, "totalElements": 1, "totalPages": 1,
                "first": true, "numberOfElements": 1, "size": 10, "number": 0, "empty": false
              } }
            """));
        var client = CreateClient(handler);

        var result = await client.GetWalletTransactionsAsync("1234567890", pageNo: 0, pageSize: 10);

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/api/v1/disbursements/wallet/transactions", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("accountNumber=1234567890", handler.Requests[0].RequestUri!.Query);
        Assert.Single(result.Content);
        Assert.Equal("DEBIT", result.Content[0].TransactionType);
        Assert.Equal(5000000000m, result.Content[0].AvailableBalanceBefore);
        Assert.Equal(1000000000m, result.Content[0].Amount);
        Assert.Null(result.Content[0].Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetWalletTransactionsAsync_MissingAccountNumber_Throws(string? accountNumber)
    {
        var client = CreateClient(new FakeHttpMessageHandler());
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetWalletTransactionsAsync(accountNumber!));
    }
}
