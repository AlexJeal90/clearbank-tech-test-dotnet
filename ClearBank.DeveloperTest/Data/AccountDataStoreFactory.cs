using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Types.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ClearBank.DeveloperTest.Data;

public sealed class AccountDataStoreFactory(IServiceProvider serviceProvider, IOptionsMonitor<DataStoreConfig> dataStoreConfig) : IAccountDataStoreFactory
{
    public IAccountDataStore Create()
    {
        return serviceProvider.GetRequiredKeyedService<IAccountDataStore>(dataStoreConfig.CurrentValue.DataStoreType)
            // Enhancement - throw here instead of defaulting to Primary Datastore
            ?? serviceProvider.GetRequiredKeyedService<IAccountDataStore>(DataStoreType.Primary);
    }
}
