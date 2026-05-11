using Microsoft.EntityFrameworkCore;
using Repair.Persistence.Core;

namespace Repair.Persistence;

public class RepairDatabaseContext : BaseDatabaseContext<RepairDatabaseContext>
{

    /// <inheritdoc />
    public RepairDatabaseContext(DbContextOptions<RepairDatabaseContext> options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
