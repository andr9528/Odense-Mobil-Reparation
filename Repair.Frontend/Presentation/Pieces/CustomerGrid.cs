using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class CustomersGrid : Border
{
    internal CustomersGridViewModel ViewModel => (CustomersGridViewModel) DataContext;

    public CustomersGrid(CustomersGridArguments arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);

        DataContext = new CustomersGridViewModel(arguments);

        var logic = new CustomersGridLogic(ViewModel);
        var ui = new CustomersGridUi(logic, ViewModel);

        Child = ui.CreateContentGrid();

        _ = logic.RefreshCustomers();
    }

    internal record CustomersGridArguments(
        IEntityQueryService<Customer, SearchableCustomer> QueryService,
        DispatcherQueue DispatcherQueue,
        ILoggerFactory LoggerFactory,
        int SelectedCustomerId = 0)
    {
    }
}
