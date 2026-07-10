using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data.Interfaces;

public interface IAccountDataStore
{
    Account? GetAccount(string accountNumber);
    void UpdateAccount(Account account);
}