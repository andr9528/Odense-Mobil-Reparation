using Microsoft.Extensions.DependencyInjection;
using Repair.Abstractions.Persistence;
using Repair.Abstractions.Startup;

namespace Repair.Startup.Modules;

public class EntityQueryServiceStartupModule<TQuery, TEntity, TSearchable> : IServiceStartupModule
    where TQuery : class, IEntityQueryService<TEntity, TSearchable>
    where TEntity : class, IEntity
    where TSearchable : class, ISearchable, new()
{
    /// <inheritdoc />
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IEntityQueryService<TEntity, TSearchable>, TQuery>();
    }

    /// <inheritdoc />
    public string Name => $"Entity Query Service Module - {typeof(TQuery).Name}";
}
