using Repair.Frontend.Abstraction;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Pages;
using Repair.Frontend.Services;

namespace Repair.Frontend.NavigationRegions;

public class OrdersPageRegionDefinition(ILogger<OrdersPageRegionDefinition> logger) : IPageRegion
{
    /// <inheritdoc />
    public string DisplayName => "Orders";

    /// <inheritdoc />
    public UIElement Icon => CreateIcon();

    /// <inheritdoc />
    public UIElement CreateControl(IServiceProvider services)
    {
        logger.LogInformation($"Changing page to: {nameof(OrdersPage)}");
        OrdersPage.OrdersPageArguments arguments =
            services.GetRequiredService<ArgumentsFactory>().CreateOrdersPageArguments();

        return new OrdersPage(arguments);
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
