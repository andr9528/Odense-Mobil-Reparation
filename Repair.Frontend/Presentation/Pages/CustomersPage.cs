using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Frontend.Abstraction;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

/// <summary>
/// Shows all customers and allows narrowing the list through search.
/// </summary>
internal sealed partial class CustomersPage : Border
{
    internal CustomersPageViewModel ViewModel => (CustomersPageViewModel) DataContext;

    public CustomersPage(
        IEntityQueryService<Customer, SearchableCustomer> customerQueryService, DispatcherQueue dispatcherQueue,
        ILoggerFactory loggerFactory, INavigationService navigationService)
    {
        DataContext = new CustomersPageViewModel();

        Margin = new Thickness(0);

        var viewModel = (CustomersPageViewModel) DataContext;
        var logic = new CustomersPageLogic(customerQueryService, viewModel, dispatcherQueue,
            loggerFactory.CreateLogger<CustomersPageLogic>(), navigationService);
        var ui = new CustomersPageUi(logic, viewModel);

        Child = ui.CreateContentGrid();

        _ = logic.RefreshCustomers();
    }
}
