using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Repair.Persistence;


namespace Repair.Tests.Core
{
    public abstract class BaseDatabaseTest : IDisposable
    {
        private readonly SqliteConnection connection;
        private readonly IDbContextFactory<RepairDatabaseContext> contextFactory;
        private bool disposed;

        protected BaseDatabaseTest()
        {
            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var optionsBuilder = new DbContextOptionsBuilder<RepairDatabaseContext>().UseSqlite(connection);

            contextFactory = new PooledDbContextFactory<RepairDatabaseContext>(optionsBuilder.Options);

            using RepairDatabaseContext context = CreateContext();

            context.Database.Migrate();
        }

        protected RepairDatabaseContext CreateContext()
        {
            return contextFactory.CreateDbContext();
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            if (contextFactory is IDisposable disposableFactory)
                disposableFactory.Dispose();

            connection.Dispose();

            disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
