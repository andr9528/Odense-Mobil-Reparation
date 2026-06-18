using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Factory;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class CustomersGrid : Border, INavigationRefreshable
{
    internal CustomersGridViewModel ViewModel => (CustomersGridViewModel) DataContext;

    public CustomersGrid(CustomersGridArguments arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);
        this.ConfigurePieceBorder();

        DataContext = new CustomersGridViewModel(arguments);

        Logic = new CustomersGridLogic(ViewModel);
        var ui = new CustomersGridUi(Logic, ViewModel);

        Child = ui.CreateContentGrid();

        _ = Logic.RefreshCustomers();
    }

    private CustomersGridLogic Logic { get; set; }

    internal record CustomersGridArguments(
        IEntityQueryService<Customer, SearchableCustomer> QueryService,
        IUiDispatcher UiDispatcher,
        ILoggerFactory LoggerFactory,
        int SelectedCustomerId = 0)
    {
    }

    /// <inheritdoc />
    public void RefreshAfterNavigation()
    {
        var logger = ViewModel.Arguments.LoggerFactory.CreateLogger<CustomersGrid>();
        logger.LogInformation($"Refreshing Customers after Navigation");

        _ = Logic.RefreshCustomers();
    }
}
