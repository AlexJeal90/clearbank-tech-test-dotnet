using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Extensions;

public static class AccountExtensions
{
    public static bool IsValidPaymentRequest(this Account? account, MakePaymentRequest request)
    {
        // Suggested enhancement - Also add validation for negative payment amounts
        return request.PaymentScheme switch
        {
            _ when account is null => false,
            PaymentScheme.FasterPayments when account.IsValidForFasterPayments(request) => true,
            PaymentScheme.Bacs when account.IsValidForBacs() => true,
            PaymentScheme.Chaps when account.IsValidForChaps() => true,
            _ => false
        };
    }

    private static bool IsValidForFasterPayments(this Account account, MakePaymentRequest request)
    {
        return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments)
            && account.Balance >= request.Amount;
    }
    
    private static bool IsValidForChaps(this Account account)
    {
        return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) 
            && account.Status == AccountStatus.Live;
    }

    private static bool IsValidForBacs(this Account account) => account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
}
