using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Extensions;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Types.Config;
using Microsoft.Extensions.Options;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService(IOptionsMonitor<DataStoreConfig> dataStoreConfig) : IPaymentService
    {
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            // OptionsMonitor allows for hot-reload of the dataStore type on invocation
            var dataStoreType = dataStoreConfig.CurrentValue.DataStoreType;

            Account account = null;

            if (dataStoreType == DataStoreType.Backup)
            {
                var accountDataStore = new BackupAccountDataStore();
                account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            }
            else
            {
                var accountDataStore = new AccountDataStore();
                account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            }

            // Implemented IsValidPaymentRequest Extension method to replace switch statement and enable better testing
            if (!account.IsValidPaymentRequest(request))
            {
                return new MakePaymentResult
                {
                    Success = false
                };
            }

            account = account with
            {
                Balance = account.Balance - request.Amount,
            };

            if (dataStoreType == DataStoreType.Backup)
            {
                var accountDataStore = new BackupAccountDataStore();
                accountDataStore.UpdateAccount(account);
            }
            else
            {
                var accountDataStore = new AccountDataStore();
                accountDataStore.UpdateAccount(account);
            }

            return new MakePaymentResult
            {
                Success = true
            };
        }
    }
}
