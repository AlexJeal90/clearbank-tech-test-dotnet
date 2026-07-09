using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Types.Config;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Linq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services;

public class PaymentServiceTests
{
    private readonly IOptionsMonitor<DataStoreConfig> _dataStoreConfigMock;

    public PaymentServiceTests()
    {
        _dataStoreConfigMock = Substitute.For<IOptionsMonitor<DataStoreConfig>>();
    }

    /// <summary>
    /// Flaky test to check that the IOptionsMonitor is called - best we can do until the remaining refactor is complete
    /// </summary>
    [Fact]
    public void MakePayment_RetrievesConfigFromOptionsMonitor()
    {
        _dataStoreConfigMock.CurrentValue.Returns(
            new DataStoreConfig
            {
                DataStoreType = DataStoreType.Backup
            });

        var sut = new PaymentService(_dataStoreConfigMock);

        var request = new MakePaymentRequest
        {
            PaymentScheme = PaymentScheme.Bacs
        };

        _dataStoreConfigMock.ClearReceivedCalls();
        sut.MakePayment(request);

        _ = _dataStoreConfigMock.Received(1).CurrentValue;
    }
}
