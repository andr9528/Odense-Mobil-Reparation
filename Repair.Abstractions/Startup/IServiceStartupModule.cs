using Microsoft.Extensions.DependencyInjection;

namespace Repair.Abstractions.Startup;

public interface IServiceStartupModule
{
    void ConfigureServices(IServiceCollection services);
    string Name { get; }
}
