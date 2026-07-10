using ClearBank.DeveloperTest.Data.Interfaces;
using NSubstitute;

namespace ClearBank.DeveloperTest.Tests.Services;

public partial class PaymentServiceTests
{
    private readonly IAccountDataStoreFactory _accountDataStoreFactoryMock;
    private readonly IAccountDataStore _accountDataStoreMock;

    public PaymentServiceTests()
    {
        _accountDataStoreFactoryMock = Substitute.For<IAccountDataStoreFactory>();
        _accountDataStoreMock = Substitute.For<IAccountDataStore>();
    }    
}
