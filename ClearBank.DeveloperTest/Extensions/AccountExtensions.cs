using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Extensions;

public static class AccountExtensions
{
    public static bool IsValidPaymentRequest(this Account? account, MakePaymentRequest request)
    {
        return request.PaymentScheme switch
        {
            _ when account is null => false,
            PaymentScheme.FasterPayments when account.IsFasterPaymentsPermitted(request) => true,
            PaymentScheme.Bacs when account.IsBacsPaymentPermitted() => true,
            PaymentScheme.Chaps when account.IsChapsPaymentPermitted() => true,
            _ => false
        };
    }

    private static bool IsFasterPaymentsPermitted(this Account account, MakePaymentRequest request)
    {
        return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments)
            && account.Balance >= request.Amount;
    }
    
    private static bool IsChapsPaymentPermitted(this Account account)
    {
        return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) 
            && account.Status == AccountStatus.Live;
    }

    private static bool IsBacsPaymentPermitted(this Account account) => account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
}
