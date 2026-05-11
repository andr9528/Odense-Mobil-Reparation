using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Repair.Persistence;
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
    }
}
