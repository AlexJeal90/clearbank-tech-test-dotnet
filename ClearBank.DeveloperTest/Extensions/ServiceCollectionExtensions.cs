using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClearBank.DeveloperTest.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPaymentServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register the DataStoreConfigOptions
        services.Configure<DataStoreConfig>(configuration.GetSection(DataStoreConfig.ConfigSection));

        // Register the AccountDataStores as Keyed Scoped
        // Selected scoped for hypothetical per-request datastore connections
        services.AddKeyedScoped<IAccountDataStore, AccountDataStore>(DataStoreType.Primary);
        services.AddKeyedScoped<IAccountDataStore, BackupAccountDataStore>(DataStoreType.Backup);

        services.AddScoped<IAccountDataStoreFactory, AccountDataStoreFactory>();

        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }
}
