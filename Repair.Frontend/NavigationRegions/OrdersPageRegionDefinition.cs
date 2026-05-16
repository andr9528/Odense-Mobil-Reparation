using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Pages;

namespace Repair.Frontend.NavigationRegions;

public class OrdersPageRegionDefinition : IPageRegion
{
    private readonly ILogger<OrdersPageRegionDefinition> logger;

    public OrdersPageRegionDefinition(ILogger<OrdersPageRegionDefinition> logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc />
    public string DisplayName => "Orders";

    /// <inheritdoc />
    public IconElement Icon => new SymbolIcon(Symbol.Page);

    /// <inheritdoc />
    public UIElement CreateControl(IServiceProvider services)
    {
        logger.LogInformation($"Changing page to: {nameof(OrdersPage)}");

        return ActivatorUtilities.CreateInstance<OrdersPage>(services);
    }
}
