using Microsoft.EntityFrameworkCore;
using Repair.Models.Entity.Model;
using Repair.Persistence.Configuration;
using Repair.Persistence.Core;
using Repair.Persistence.Core.Abstraction;

namespace Repair.Persistence;

public class RepairDatabaseContext : BaseDatabaseContext<RepairDatabaseContext>
{

    /// <inheritdoc />
    public RepairDatabaseContext(DbContextOptions<RepairDatabaseContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Order> Orders { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var databaseType = DatabaseType.SQL_LITE;

        modelBuilder.ApplyConfiguration(new CustomerConfiguration(databaseType));
        modelBuilder.ApplyConfiguration(new OrderConfiguration(databaseType));
    }
}
