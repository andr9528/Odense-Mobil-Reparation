using Repair.Frontend.Abstraction;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Pages;
using Repair.Frontend.Services;

namespace Repair.Frontend.NavigationRegions;

public class CreateOrderPageRegionDefinition(ILogger<CreateOrderPageRegionDefinition> logger) : IPageRegion
{
    public string DisplayName => "Create Order";

    public UIElement Icon => CreateIcon();

    public UIElement CreateControl(IServiceProvider services)
    {
        logger.LogInformation($"Changing page to: {nameof(OrderCreationPage)}");
        OrderCreationPage.OrderCreationPageArguments arguments =
            services.GetRequiredService<ArgumentsFactory>().CreateOrderCreationPageArguments();

        return new OrderCreationPage(arguments);
    }

    private Grid CreateIcon()
    {
        Grid grid = new()
        {
            Height = 24,
        };
        grid.DefineColumns(GridUnitType.Auto, [1, 1,]);

        grid.Children.Add(new SymbolIcon(Symbol.Add)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        }.SetColumn(0));

        grid.Children.Add(new SymbolIcon(Symbol.Repair)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        }.SetColumn(1));

        return grid;
    }
}
