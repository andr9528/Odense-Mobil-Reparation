using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.UI.Dispatching;
using Repair.Frontend.Abstraction;
using Repair.Frontend.NavigationRegions;
using Repair.Frontend.Services;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;
using Repair.Persistence;
using Repair.Persistence.Services;
using Repair.Services;
using Repair.Startup;
using Repair.Startup.Modules;

namespace Repair.Frontend;

internal class UnoStartup : ModularStartup<IApplicationBuilder>
{
    public IHost? Host { get; private set; }

    private readonly IConfiguration configuration;
    private readonly ConfigurationService configurationService;

    public UnoStartup()
    {
        configurationService = new ConfigurationService();
        configuration = configurationService.BuildConfiguration();

        AddModule(new LoggingStartupModule(new[]
        {
            LogTarget.CONSOLE,
            LogTarget.FILE,
        }, configurationService.GetApplicationDataPath()));

        AddModule(
            new DatabaseContextStartupModule<RepairDatabaseContext>(configurationService.ConfigureDatabaseOptions));

        AddModule(new EntityQueryServiceStartupModule<CustomerQueryService, Customer, SearchableCustomer>());
        AddModule(new EntityQueryServiceStartupModule<OrderQueryService, Order, SearchableOrder>());
    }

    protected override void ConfigureApplication(IApplicationBuilder app)
    {
        app.Configure(host => host
#if DEBUG
            // Switch to Development environment when running in DEBUG
            .UseEnvironment(Environments.Development)
#endif
            .UseConfiguration(configure: ConfigureConfigurationSource).UseSerialization(ConfigureSerialization));

        base.ConfigureApplication(app);

        Host = app.Build();
    }

    private void ConfigureSerialization(HostBuilderContext builderContext, IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(new JsonSerializerOptions {IncludeFields = true,});
    }

    private IHostBuilder ConfigureConfigurationSource(IConfigBuilder configBuilder)
    {
        return configBuilder.EmbeddedSource<App>().Section<AppConfig>();
    }

    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.AddSingleton(configurationService);
        services.AddSingleton<INavigationService, NavigationService>();

        DispatcherQueue? uiDispatcherQueue = DispatcherQueue.GetForCurrentThread();
        services.AddSingleton(uiDispatcherQueue);

        services.AddSingleton<IPageRegion, CustomersPageRegionDefinition>();
        services.AddSingleton<IPageRegion, OrdersPageRegionDefinition>();
        services.AddSingleton<IPageRegion, CreateCustomerPageRegionDefinition>();
        services.AddSingleton<IPageRegion, CreateOrderPageRegionDefinition>();
    }
}
