using Monnify.Webhooks;

namespace Monnify.Tests.Webhooks;

// Every payload below is Monnify's own documented sample for that event type, copied verbatim
// (not reformatted), so these tests prove the models match real shapes rather than ones we guessed.
public class MonnifyWebhookEventDataTests
{
    [Fact]
    public void SuccessfulCollection_ParsesIntoCollectionTransactionEventData()
    {
        var envelope = MonnifyWebhookParser.Parse("""
            {
              "eventType": "SUCCESSFUL_TRANSACTION",
              "eventData": {
                "product": {
                  "reference": "1636106097661",
                  "type": "RESERVED_ACCOUNT"
                },
                "transactionReference": "MNFY|04|20211117112842|000170",
                "paymentReference": "MNFY|04|20211117112842|000170",
                "paidOn": "2021-11-17 11:28:42.615",
                "paymentDescription": "Adm",
                "metaData": {},
                "paymentSourceInformation": [
                  {
                    "bankCode": "",
                    "amountPaid": 3000,
                    "accountName": "Monnify Limited",
                    "sessionId": "e6cV1smlpkwG38Cg6d5F9B2PRnIq5FqA",
                    "accountNumber": "0065432190"
                  }
                ],
                "destinationAccountInformation": {
                  "bankCode": "232",
                  "bankName": "Sterling bank",
                  "accountNumber": "6000140770"
                },
                "amountPaid": 3000,
                "totalPayable": 3000,
                "cardDetails": {},
                "paymentMethod": "ACCOUNT_TRANSFER",
                "currency": "NGN",
                "settlementAmount": "2990.00",
                "paymentStatus": "PAID",
                "customer": {
                  "name": "John Doe",
                  "email": "test@tester.com"
                }
              }
            }
            """);

        Assert.Equal(MonnifyWebhookEventTypes.SuccessfulTransaction, envelope.EventType);
        var data = MonnifyWebhookParser.ParseEventData<CollectionTransactionEventData>(envelope);

        Assert.Equal("RESERVED_ACCOUNT", data.Product!.Type);
        Assert.Equal(3000m, data.AmountPaid);
        Assert.Equal(2990.00m, data.SettlementAmount);
        Assert.Equal("PAID", data.PaymentStatus);
        Assert.Single(data.PaymentSourceInformation);
        Assert.Equal("Monnify Limited", data.PaymentSourceInformation[0].AccountName);
        Assert.Equal("Sterling bank", data.DestinationAccountInformation!.BankName);
        Assert.Equal("John Doe", data.Customer!.Name);
        Assert.Null(data.InvoiceReference);
        Assert.Null(data.OfflineProductInformation);
    }

    [Fact]
    public void CompletedOfflinePayment_ParsesIntoCollectionTransactionEventData_WithEmptyObjectFields()
    {
        // Same eventType as a regular collection, but paymentSourceInformation/destinationAccountInformation
        // are empty objects instead of an array/populated object here.
        var envelope = MonnifyWebhookParser.Parse("""
            {
              "eventType": "SUCCESSFUL_TRANSACTION",
              "eventData": {
                "product": {
                  "reference": "MNF-Tl9Noo0G48000890",
                  "type": "OFFLINE_PAYMENT_AGENT"
                },
                "transactionReference": "MNFY|76|20230830171357|000252",
                "invoiceReference": "MNF-Tl9Noo0G48000890",
                "paymentReference": "MNF-Tl9Noo0G48000890",
                "paidOn": "30/08/2023 5:13:57 PM",
                "paymentDescription": "adron",
                "metaData":{
                  "phoneNumber":"08088523241",
                  "name":"Khalid"
                },
                "destinationAccountInformation": {},
                "paymentSourceInformation": {},
                "amountPaid": 15000,
                "totalPayable": 15000,
                "offlineProductInformation": {
                  "amount": 15000,
                  "code": "56417",
                  "type": "INVOICE"
                },
                "cardDetails": {},
                "paymentMethod": "CASH",
                "currency": "NGN",
                "settlementAmount": 14990,
                "paymentStatus": "PAID",
                "customer": {
                  "name": "David Customer",
                  "email": "mayluv55@hotmail.co.uk"
                }
              }
            }
            """);

        var data = MonnifyWebhookParser.ParseEventData<CollectionTransactionEventData>(envelope);

        Assert.Equal("MNF-Tl9Noo0G48000890", data.InvoiceReference);
        Assert.Empty(data.PaymentSourceInformation);
        Assert.NotNull(data.DestinationAccountInformation);
        Assert.Null(data.DestinationAccountInformation!.BankCode);
        Assert.Equal("INVOICE", data.OfflineProductInformation!.Type);
        Assert.Equal(15000m, data.OfflineProductInformation!.Amount);
        Assert.Equal(14990m, data.SettlementAmount);
        Assert.Equal("Khalid", data.MetaData!["name"].GetString());
    }

    [Theory]
    [InlineData(MonnifyWebhookEventTypes.SuccessfulDisbursement, "SUCCESS")]
    [InlineData(MonnifyWebhookEventTypes.FailedDisbursement, "FAILED")]
    public void DisbursementStatusChange_ParsesIntoDisbursementStatusEventData(string eventType, string expectedStatus)
    {
        var envelope = MonnifyWebhookParser.Parse($$"""
            {
              "eventType": "{{eventType}}",
              "eventData": {
                "amount": 10,
                "transactionReference": "MFDS|20210317032332|002431",
                "fee": 8,
                "transactionDescription": "Approved or completed successfully",
                "destinationAccountNumber": "0068687503",
                "sessionId": "090405210317032336726272971260",
                "createdOn": "17/03/2021 3:23:32 AM",
                "destinationAccountName": "DAMILARE SAMUEL OGUNNAIKE",
                "reference": "ref1615947809303",
                "destinationBankCode": "232",
                "completedOn": "17/03/2021 3:23:38 AM",
                "narration": "This is a quite long narration",
                "currency": "NGN",
                "destinationBankName": "Sterling bank",
                "status": "{{expectedStatus}}"
              }
            }
            """);

        var data = MonnifyWebhookParser.ParseEventData<DisbursementStatusEventData>(envelope);

        Assert.Equal(expectedStatus, data.Status);
        Assert.Equal(10m, data.Amount);
        Assert.Equal("ref1615947809303", data.Reference);
        Assert.Equal("Sterling bank", data.DestinationBankName);
    }

    [Fact]
    public void ReversedDisbursement_ParsesIntoDisbursementStatusEventData()
    {
        var envelope = MonnifyWebhookParser.Parse("""
            {
              "eventType": "REVERSED_DISBURSEMENT",
              "eventData": {
                "transactionReference": "MFDS33920240513211815009133P47MKU",
                "reference": "662d2dcf22132ea227db164e-1715631494637",
                "narration": "Fund Transfer",
                "currency": "NGN",
                "amount": 145708,
                "status": "REVERSED",
                "fee": 8,
                "destinationAccountNumber": "8088523251",
                "destinationAccountName": "Marvelous Benji",
                "destinationBankCode": "305",
                "sessionId": "090405240513211816637369129129",
                "createdOn": "13/05/2023 9:18:16 PM",
                "completedOn": "13/05/2023 9:18:19 PM"
              }
            }
            """);

        var data = MonnifyWebhookParser.ParseEventData<DisbursementStatusEventData>(envelope);

        Assert.Equal("REVERSED", data.Status);
        Assert.Equal(145708m, data.Amount);
    }

    [Theory]
    [InlineData(MonnifyWebhookEventTypes.SuccessfulRefund, "COMPLETED")]
    [InlineData(MonnifyWebhookEventTypes.FailedRefund, "FAILED")]
    public void RefundStatusChange_ParsesIntoRefundEventData(string eventType, string expectedStatus)
    {
        var envelope = MonnifyWebhookParser.Parse($$"""
            {
              "eventType": "{{eventType}}",
              "eventData": {
                "merchantReason":"defective goods",
                "transactionReference":"MNFY|20190816083102|000021",
                "completedOn":"14/04/2021 4:24:05 PM",
                "refundStatus":"{{expectedStatus}}",
                "customerNote":"defects",
                "createdOn":"14/04/2021 4:23:37 PM",
                "refundReference":"ref001",
                "refundAmount":10.00
              }
            }
            """);

        var data = MonnifyWebhookParser.ParseEventData<RefundEventData>(envelope);

        Assert.Equal(expectedStatus, data.RefundStatus);
        Assert.Equal(10.00m, data.RefundAmount);
        Assert.Equal("ref001", data.RefundReference);
    }

    [Fact]
    public void Settlement_ParsesIntoSettlementEventData_WithNestedTransactions()
    {
        var envelope = MonnifyWebhookParser.Parse("""
            {
              "eventType": "SETTLEMENT",
              "eventData": {
                "amount": "1199.00",
                "settlementTime": "11/11/2021 02:29:00 PM",
                "settlementReference": "LB8HG1PNZT4ATJGZXQBY",
                "destinationAccountNumber": "6000000249",
                "destinationBankName": "Fidelity Bank",
                "destinationAccountName": "Teamapt Limited234",
                "transactionsCount": 1,
                "transactions": [
                  {
                    "product": {
                      "reference": "2134565wda",
                      "type": "2134565wda"
                    },
                    "transactionReference": "MNFY|26|20211111142601|000001",
                    "paymentReference": "MNFY|26|20211111142601|000001",
                    "paidOn": "11/11/2021 02:26:02 PM",
                    "paymentDescription": "Seg",
                    "accountPayments": [
                      {
                        "bankCode": "000014",
                        "amountPaid": "1234.00",
                        "accountName": "Okeke Chimezie",
                        "accountNumber": "******1070"
                      }
                    ],
                    "amountPaid": "1234.00",
                    "totalPayable": "1234.00",
                    "accountDetails": {
                      "bankCode": "000014",
                      "amountPaid": "1234.00",
                      "accountName": "Okeke Chimezie",
                      "accountNumber": "******1070"
                    },
                    "cardDetails": {},
                    "paymentMethod": "ACCOUNT_TRANSFER",
                    "currency": "NGN",
                    "paymentStatus": "PAID",
                    "customer": {
                      "name": "Segun Adeponle",
                      "email": "segunadeponle@gmail.com"
                    }
                  }
                ]
              }
            }
            """);

        var data = MonnifyWebhookParser.ParseEventData<SettlementEventData>(envelope);

        Assert.Equal(1199.00m, data.Amount);
        Assert.Equal(1, data.TransactionsCount);
        Assert.Single(data.Transactions);
        var transaction = data.Transactions[0];
        Assert.Equal(1234.00m, transaction.AmountPaid);
        Assert.Equal("Segun Adeponle", transaction.Customer!.Name);
        Assert.Single(transaction.AccountPayments);
        Assert.Equal("Okeke Chimezie", transaction.AccountPayments[0].AccountName);
        Assert.Equal("Okeke Chimezie", transaction.AccountDetails!.AccountName);
    }

    [Fact]
    public void MandateStatusChange_ParsesIntoMandateStatusEventData()
    {
        var envelope = MonnifyWebhookParser.Parse("""
            {
              "eventType": "MANDATE_UPDATE",
              "eventData": {
                "customerAddress": "Everywhere is an address",
                "endDate": "2024-12-31 08:00:00.0",
                "customerEmailAddress": "ogunnaike.damilare@gmail.com",
                "customerAccountName": "SAMUEL DAMILARE OGUNNAIKE",
                "customerAccountNumber": "2191406799",
                "customerAccountBankCode": "057",
                "customerName": "Damilare Ogunnaike",
                "mandateDescription": "Testing Monnify Mandate",
                "externalMandateReference": "mfy-mandate-102",
                "mandateStatus": "CANCELLED",
                "mandateAmount": 100000,
                "autoRenew": false,
                "mandateCode": "MTDD|01J3GRJH8D58B20VNX1E6GSY1N",
                "contractCode": "626689863141",
                "customerPhoneNumber": "08166189142",
                "startDate": "2024-07-24 08:00:00.0"
              }
            }
            """);

        var data = MonnifyWebhookParser.ParseEventData<MandateStatusEventData>(envelope);

        Assert.Equal("CANCELLED", data.MandateStatus);
        Assert.Equal(100000m, data.MandateAmount);
        Assert.False(data.AutoRenew);
        Assert.Equal("MTDD|01J3GRJH8D58B20VNX1E6GSY1N", data.MandateCode);
    }

    [Fact]
    public void WalletActivityNotification_ParsesIntoWalletActivityEventData_AndSiblingMetaData()
    {
        var envelope = MonnifyWebhookParser.Parse("""
            {
              "eventType": "ACCOUNT_ACTIVITY",
              "eventData": {
                "accountType": "MAIN",
                "accountName": "Test01",
                "accountNumber": "8016472829",
                "accountNuban": null,
                "activityType": "TRANSACTION",
                "amount": 100,
                "currency": "566",
                "balanceBefore": 862.68,
                "balanceAfter": 962.68,
                "reference": "MFY_WTP_TRF_2MPT61CFP_1896839989128998912_CBA_CREDIT_0_CREDIT_0",
                "narration": " MFY-WT/#/TRF|2MPT61cfp|1896839989128998912_CBA_CREDIT_0/#/2025-03-04/#/VA-6927004623/#/From-Moniepoint Microfinance Bank/#/Test User/#/5744000051",
                "activityTime": "2025-03-04 10:27:AM"
              },
              "metaData": {
                "senderAccount": "Monnify Service",
                "sourceAccountName": null,
                "sourceAccountNumber": null,
                "sourceBankCode": null,
                "sourceBankName": null
              }
            }
            """);

        var data = MonnifyWebhookParser.ParseEventData<WalletActivityEventData>(envelope);
        var metaData = MonnifyWebhookParser.ParseMetaData<WalletActivityMetaData>(envelope);

        Assert.Equal(100m, data.Amount);
        Assert.Equal(862.68m, data.BalanceBefore);
        Assert.Equal(962.68m, data.BalanceAfter);
        Assert.Null(data.AccountNuban);
        Assert.Equal("Monnify Service", metaData!.SenderAccount);
        Assert.Null(metaData.SourceAccountNumber);
    }

    [Fact]
    public void LowBalanceAlert_ParsesIntoLowBalanceAlertEventData()
    {
        var envelope = MonnifyWebhookParser.Parse("""
            {
            "eventType": "LOW_BALANCE_ALERT",
            "eventData": {
              "transactionTime": "2025-09-01T23:13:19Z",
              "merchantCode": "99ZYAFM0F3CY",
              "walletAccountNumber": "8023759978",
              "walletBalance": 0,
              "lowBalanceThreshold": 2000,
              "currency": "NGN",
              "description": "Your wallet balance has dropped below the configured threshold. Please fund your account."
              }
            }
            """);

        var data = MonnifyWebhookParser.ParseEventData<LowBalanceAlertEventData>(envelope);

        Assert.Equal(0m, data.WalletBalance);
        Assert.Equal(2000m, data.LowBalanceThreshold);
        Assert.Equal("99ZYAFM0F3CY", data.MerchantCode);
    }

    [Fact]
    public void RejectedPayment_ParsesIntoRejectedPaymentEventData_WithSingleObjectPaymentSource()
    {
        // paymentSourceInformation is a single populated object here (not an array, not empty) -
        // a third shape variant for this field, on top of the array/empty-object ones seen elsewhere.
        var envelope = MonnifyWebhookParser.Parse("""
            {
              "eventType": "REJECTED_PAYMENT",
              "eventData": {
                "product": {
                  "reference": "MNFY|PAYREF|GENERATED|1687798434397393735",
                  "type": "WEB_SDK"
                },
                "amount": 100,
                "paymentSourceInformation": {
                  "bankCode": "50515",
                  "amountPaid": 40,
                  "accountName": "MARVELOUS BENJI",
                  "sessionId": "090405230626180003067844645188",
                  "accountNumber": "5141901487"
                },
                "transactionReference": "MNFY|85|20230626175354|041855",
                "created_on": "2023-06-26 17:53:55.0",
                "paymentReference": "MNFY|PAYREF|GENERATED|1687798434397393735",
                "paymentRejectionInformation": {
                  "bankCode": "035",
                  "destinationAccountNumber": "7023576853",
                  "bankName": "Wema bank",
                  "rejectionReason": "UNDER_PAYMENT",
                  "expectedAmount": 100
                },
                "paymentDescription": "lets pay",
                "customer": {
                  "name": "Marvelous Benji",
                  "email": "benji71@gmail.com"
                }
              }
            }
            """);

        var data = MonnifyWebhookParser.ParseEventData<RejectedPaymentEventData>(envelope);

        Assert.Equal(100m, data.Amount);
        Assert.Equal("2023-06-26 17:53:55.0", data.CreatedOn);
        Assert.Single(data.PaymentSourceInformation);
        Assert.Equal("MARVELOUS BENJI", data.PaymentSourceInformation[0].AccountName);
        Assert.Equal("UNDER_PAYMENT", data.PaymentRejectionInformation!.RejectionReason);
        Assert.Equal(100m, data.PaymentRejectionInformation!.ExpectedAmount);
    }
}
