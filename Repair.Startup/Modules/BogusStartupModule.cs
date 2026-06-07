using Microsoft.Extensions.DependencyInjection;
using Repair.Abstractions.Persistence;
using Repair.Abstractions.Startup;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;
using Repair.Services;

namespace Repair.Startup.Modules;

public class BogusStartupModule(int minimumCustomersTarget) : IServiceStartupModule
{
    /// <inheritdoc />
    public void ConfigureServices(IServiceCollection services)
    {
        _ = AddCustomerIfNeeded(services);
    }

    private async Task AddCustomerIfNeeded(IServiceCollection services)
    {
        ServiceProvider provider = services.BuildServiceProvider();
        var queryService = provider.GetRequiredService<IEntityQueryService<Customer, SearchableCustomer>>();

        var currentCustomers = (await queryService.GetAllEntities()).ToList();
        if (currentCustomers.Count >= minimumCustomersTarget)
        {
            return;
        }

        var bogusService = new BogusService();
        int customersToCreate = minimumCustomersTarget - currentCustomers.Count + Random.Shared.Next(1, 6);

        var newBogusCustomers = bogusService.CreateCustomers(customersToCreate);
        await queryService.AddEntities(newBogusCustomers);
    }

    /// <inheritdoc />
    public string Name => "Bogus Module";
}
