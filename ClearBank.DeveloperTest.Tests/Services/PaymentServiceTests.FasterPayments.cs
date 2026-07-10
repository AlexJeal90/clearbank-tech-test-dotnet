using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services;

public partial class PaymentServiceTests
{
    [Fact]
    public void MakePayment_ForFasterPayments_WhenRequestIsValid_UpdatesAccountAndReturnsSuccess()
    {
        var accountStartingBalance = 10;
        var requestAmount = 1;

        // Balance - Request Amount
        var expectedUpdateBalance = 9;

        var account = new Account
        {
            Balance = accountStartingBalance,
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
        };

        var request = new MakePaymentRequest
        {
            PaymentScheme = PaymentScheme.FasterPayments,
            Amount = requestAmount
        };

        _accountDataStoreMock.GetAccount(Arg.Any<string>()).Returns(account);
        _accountDataStoreFactoryMock.Create().Returns(_accountDataStoreMock);

        var sut = new PaymentService(_accountDataStoreFactoryMock);

        var result = sut.MakePayment(request);
        result.Success.ShouldBeTrue();

        // UpdateAccount should have been called with the expectedUpdateBalance
        _accountDataStoreMock.Received(1).UpdateAccount(Arg.Is<Account>(x => x.Balance == expectedUpdateBalance));
    }

    [Fact]
    public void MakePayment_ForFasterPayments_WhenRequestIsInvalid_DoesNotUpdateAndReturnsFalse()
    {
        var accountStartingBalance = 10;
        // Request amount higher than the balance will fail validation
        var requestAmount = 11;

        var account = new Account
        {
            Balance = accountStartingBalance,
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
        };

        var request = new MakePaymentRequest
        {
            PaymentScheme = PaymentScheme.FasterPayments,
            Amount = requestAmount
        };

        _accountDataStoreMock.GetAccount(Arg.Any<string>()).Returns(account);
        _accountDataStoreFactoryMock.Create().Returns(_accountDataStoreMock);

        var sut = new PaymentService(_accountDataStoreFactoryMock);

        var result = sut.MakePayment(request);
        result.Success.ShouldBeFalse();

        // UpdateAccount should not be called for a failed update
        _accountDataStoreMock.Received(0).UpdateAccount(Arg.Any<Account>());
    }


}
