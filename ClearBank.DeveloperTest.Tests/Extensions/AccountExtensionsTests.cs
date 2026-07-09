using ClearBank.DeveloperTest.Extensions;
using ClearBank.DeveloperTest.Types;
using Shouldly;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Extensions;

public class AccountExtensionsTests
{
    [Fact]
    public void IsPaymentPermitted_WhenAccountIsNull_ReturnsFalse()
    {
        Account account = null;

        var request = new MakePaymentRequest
        {
            Amount = 1
        };

        var result = account.IsValidPaymentRequest(request);
        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData(PaymentScheme.FasterPayments)]
    [InlineData(PaymentScheme.Bacs)]
    [InlineData(PaymentScheme.Chaps)]
    public void IsPaymentPermitted_WhenAllowedPaymentSchemeIsNotSet_ReturnsFalse(PaymentScheme scheme)
    {
        var account = new Account();

        var request = new MakePaymentRequest
        {
            PaymentScheme = scheme,
            Amount = 1
        };

        var result = account.IsValidPaymentRequest(request);
        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData(0, 1, false)]
    [InlineData(1, 1, true)]
    [InlineData(2, 1, true)]
    public void IsPaymentPermitted_FasterPayments_ReturnsCorrectly(int accountBalance, int requestAmount, bool expectedResult)
    {
        var account = new Account
        {
            Balance = accountBalance,
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
        };

        var request = new MakePaymentRequest
        {
            PaymentScheme = PaymentScheme.FasterPayments,
            Amount = requestAmount
        };

        var result = account.IsValidPaymentRequest(request);
        
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData(AccountStatus.Live, true)]
    [InlineData(AccountStatus.InboundPaymentsOnly, false)]
    [InlineData(AccountStatus.Disabled, false)]
    public void IsPaymentPermitted_Chaps_ReturnsCorrectly(AccountStatus accountStatus, bool expectedResult)
    {
        var account = new Account
        {
            Status = accountStatus,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps
        };

        var request = new MakePaymentRequest
        {
            PaymentScheme = PaymentScheme.Chaps,
            Amount = 1
        };

        var result = account.IsValidPaymentRequest(request);

        result.ShouldBe(expectedResult);
    }
}
