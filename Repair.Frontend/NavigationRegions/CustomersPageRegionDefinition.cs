using Repair.Frontend.Abstraction;
using Repair.Frontend.Extensions;
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
    public UIElement Icon => CreateIcon();

    /// <inheritdoc />
    public UIElement CreateControl(IServiceProvider services)
    {
        logger.LogInformation($"Changing page to: {nameof(CustomersPage)}");

        return ActivatorUtilities.CreateInstance<CustomersPage>(services);
    }

    private Grid CreateIcon()
    {
        Grid grid = new()
        {
            Height = 24,
        };
        grid.DefineColumns(GridUnitType.Auto, [1, 1,]);

        grid.Children.Add(new SymbolIcon(Symbol.People)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        }.SetColumn(0));

        grid.Children.Add(new SymbolIcon(Symbol.People)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        }.SetColumn(1));

        return grid;
    }
}
