using Repair.Frontend.Abstraction;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Pages;

namespace Repair.Frontend.NavigationRegions;

public class CreateOrderPageRegionDefinition : IPageRegion
{
    private readonly ILogger<CreateOrderPageRegionDefinition> logger;

    public CreateOrderPageRegionDefinition(ILogger<CreateOrderPageRegionDefinition> logger)
    {
        this.logger = logger;
    }

    public string DisplayName => "Create Order";

    public UIElement Icon => CreateIcon();

    public UIElement CreateControl(IServiceProvider services)
    {
        logger.LogInformation($"Changing page to: {nameof(OrderCreationPage)}");

        return ActivatorUtilities.CreateInstance<OrderCreationPage>(services);
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
