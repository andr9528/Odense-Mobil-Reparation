using Repair.Frontend.Abstraction;
using Repair.Frontend.Extensions;
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
    public UIElement Icon => CreateIcon();

    /// <inheritdoc />
    public UIElement CreateControl(IServiceProvider services)
    {
        logger.LogInformation($"Changing page to: {nameof(OrdersPage)}");

        return ActivatorUtilities.CreateInstance<OrdersPage>(services);
    }

    private Grid CreateIcon()
    {
        Grid grid = new()
        {
            Height = 24,
        };
        grid.DefineColumns(GridUnitType.Auto, [1, 1,]);

        grid.Children.Add(new SymbolIcon(Symbol.Repair)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        }.SetColumn(0));

        grid.Children.Add(new SymbolIcon(Symbol.Repair)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        }.SetColumn(0, 2));

        grid.Children.Add(new SymbolIcon(Symbol.Repair)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        }.SetColumn(1));

        return grid;
    }
}
