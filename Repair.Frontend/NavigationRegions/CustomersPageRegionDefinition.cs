using System.Diagnostics;
using Repair.Frontend.Abstraction;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Pages;
using Repair.Frontend.Services;

namespace Repair.Frontend.NavigationRegions;

public class CustomersPageRegionDefinition(ILogger<CustomersPageRegionDefinition> logger) : IPageRegion
{
    /// <inheritdoc />
    public string DisplayName => "Customers";

    /// <inheritdoc />
    public UIElement Icon => CreateIcon();

    /// <inheritdoc />
    public UIElement CreateControl(IServiceProvider services)
    {
        Stopwatch? sw = null;

        try
        {
            sw = Stopwatch.StartNew();
            logger.LogInformation($"Changing page to: {nameof(CustomersPage)}");
            CustomersPage.CustomersPageArguments arguments =
                services.GetRequiredService<ArgumentsFactory>().CreateCustomersPageArguments();

            return new CustomersPage(arguments);
        }
        finally
        {
            sw?.Stop();
            logger.LogDebug("Constructed {PageName} in {Elapsed} ms.", nameof(CustomersPage), sw?.Elapsed.Milliseconds);
        }
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
