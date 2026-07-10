using ClearBank.DeveloperTest.Extensions;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Types.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Configuration;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Extensions;

public class ServiceCollectionExtensionTests
{
    /// <summary>
    /// Checks end-to-end dependency injection for the Payment Service
    /// </summary>
    [Fact]
    public void AddPaymentServices_ResolvesPaymentService()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new($"{DataStoreConfig.ConfigSection}.{nameof(DataStoreConfig.DataStoreType)}", PaymentScheme.FasterPayments.ToString())
            ])
            .Build();

        var services = new ServiceCollection()
            .AddPaymentServices(configuration);

        using var serviceProvider = services.BuildServiceProvider();

        var paymentService = serviceProvider.GetRequiredService<IPaymentService>();
        paymentService.ShouldNotBeNull();

        var result = paymentService.MakePayment(new());
        result.Success.ShouldBeFalse();
    }
}
