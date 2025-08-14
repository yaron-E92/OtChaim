using Microsoft.Extensions.DependencyInjection;
using OtChaim.Persistence;
using OtChaim.Application;

namespace OtChaim.IntegrationTests;

public abstract class IntegrationTestBase
{
    protected ServiceProvider? Provider { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var services = new ServiceCollection();
        services.AddPersistence();
        services.AddEventAggregator();
        Provider = services.BuildServiceProvider();
        ApplicationDI.SubscribeEventHandlers(Provider);
    }
}
