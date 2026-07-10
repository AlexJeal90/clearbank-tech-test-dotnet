using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Types.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using System;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Data;

public class AccountDataStoreFactoryTests
{
    private readonly IKeyedServiceProvider _serviceProviderMock;
    private readonly IOptionsMonitor<DataStoreConfig> _dataStoreConfigMock;

    private readonly IAccountDataStore accountDataStore = new AccountDataStore();
    private readonly IAccountDataStore backupDataStore = new BackupAccountDataStore();

    public AccountDataStoreFactoryTests()
    {
        _serviceProviderMock = Substitute.For<IKeyedServiceProvider>();

        _serviceProviderMock.GetRequiredKeyedService<IAccountDataStore>(DataStoreType.Primary).Returns(accountDataStore);
        _serviceProviderMock.GetRequiredKeyedService<IAccountDataStore>(DataStoreType.Backup).Returns(backupDataStore);

        _dataStoreConfigMock = Substitute.For<IOptionsMonitor<DataStoreConfig>>();
    }

    [Theory]
    [InlineData(DataStoreType.Primary, typeof(AccountDataStore))]
    [InlineData(DataStoreType.Backup, typeof(BackupAccountDataStore))]
    [InlineData((DataStoreType)2,  typeof(AccountDataStore))] // Invalid, should fall back to AccountDataStore
    public void Create_ResolvesCorrectDataStoreTypeForConfig(DataStoreType configDataStoreType, Type expectedDataStoreType)
    {
        var config = new DataStoreConfig
        {
            DataStoreType = configDataStoreType
        };

        _dataStoreConfigMock.CurrentValue.Returns(config);

        var sut = new AccountDataStoreFactory(_serviceProviderMock, _dataStoreConfigMock);

        var dataStore = sut.Create();

        dataStore.ShouldBeOfType(expectedDataStoreType);
    }
}
