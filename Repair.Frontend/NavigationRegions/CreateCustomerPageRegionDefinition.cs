using System.Diagnostics;
using Repair.Frontend.Abstraction;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Pages;
using Repair.Frontend.Services;

namespace Repair.Frontend.NavigationRegions;

public class CreateCustomerPageRegionDefinition(ILogger<CreateCustomerPageRegionDefinition> logger) : IPageRegion
{
    public string DisplayName => "Create Customer";

    public UIElement Icon => CreateIcon();

    public UIElement CreateControl(IServiceProvider services)
    {
        Stopwatch? sw = null;

        try
        {
            sw = Stopwatch.StartNew();
            logger.LogInformation($"Changing page to: {nameof(CustomerCreationPage)}");
            CustomerCreationPage.CustomerCreationPageArguments arguments =
                services.GetRequiredService<ArgumentsFactory>().CreateCustomerCreationPageArguments();

            return new CustomerCreationPage(arguments);
        }
        finally
        {
            sw?.Stop();
            logger.LogDebug("Constructed {PageName} in {Elapsed} ms.", nameof(CustomerCreationPage),
                sw?.Elapsed.Milliseconds);
        }
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

        grid.Children.Add(new SymbolIcon(Symbol.Contact)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        }.SetColumn(1));

        return grid;
    }
}
