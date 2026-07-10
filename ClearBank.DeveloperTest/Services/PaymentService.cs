using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Extensions;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService(IAccountDataStoreFactory dataStoreFactory) : IPaymentService
    {
        // AccountDataStore is now Lazy-loaded if a payment is made
        private readonly Lazy<IAccountDataStore> accountDataStore = new(dataStoreFactory.Create);

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = accountDataStore.Value.GetAccount(request.DebtorAccountNumber);

            // Implemented IsValidPaymentRequest Extension method to replace switch statement and enable better testing
            // Enhancement - Keyed Validator services for each PaymentScheme, handled with DI
            var success = account.IsValidPaymentRequest(request)
                // Short-circuit means the update can be evaluated after the validation succeeds
                && UpdateAccountBalance(request, account!);

            return new MakePaymentResult
            {
                Success = success
            };
        }

        private bool UpdateAccountBalance(MakePaymentRequest request, Account account)
        {
            account = account with
            {
                Balance = account.Balance - request.Amount,
            };

            accountDataStore.Value.UpdateAccount(account);
            return true;
        }
    }
}
