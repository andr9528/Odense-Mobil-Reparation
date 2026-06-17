using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repair.Services;

namespace Repair.Persistence;

public sealed class RepairDbDesignTimeContextFactory : IDesignTimeDbContextFactory<RepairDatabaseContext>
{
    public RepairDatabaseContext CreateDbContext(string[] args)
    {
        var configurationService = new ConfigurationService(GetType().Assembly);

        var optionsBuilder = new DbContextOptionsBuilder<RepairDatabaseContext>();

        configurationService.ConfigureDatabaseOptions(optionsBuilder);

        return new RepairDatabaseContext(optionsBuilder.Options);
    }
}
