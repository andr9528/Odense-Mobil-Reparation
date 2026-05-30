using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class CustomerGrid : Border
{
    internal CustomerGridViewModel ViewModel => (CustomerGridViewModel) DataContext;

    public CustomerGrid(CustomerGridArguments arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);

        DataContext = new CustomerGridViewModel(arguments);

        var logic = new CustomerGridLogic(ViewModel);
        var ui = new CustomerGridUi(logic, ViewModel);

        Child = ui.CreateContentGrid();

        _ = logic.RefreshCustomers();
    }

    internal record CustomerGridArguments(
        IEntityQueryService<Customer, SearchableCustomer> QueryService,
        DispatcherQueue DispatcherQueue,
        ILoggerFactory LoggerFactory,
        int SelectedCustomerId = 0)
    {
    }
}
