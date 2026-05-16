using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Pages;

namespace Repair.Frontend.NavigationRegions;

public class CustomersPageRegionDefinition : IPageRegion
{
    private readonly ILogger<CustomersPageRegionDefinition> logger;

    public CustomersPageRegionDefinition(ILogger<CustomersPageRegionDefinition> logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc />
    public string DisplayName => "Customers";

    /// <inheritdoc />
    public IconElement Icon => new SymbolIcon(Symbol.Character);

    /// <inheritdoc />
    public UIElement CreateControl(IServiceProvider services)
    {
        logger.LogInformation($"Changing page to: {nameof(CustomersPage)}");

        return ActivatorUtilities.CreateInstance<CustomersPage>(services);
    }
}
