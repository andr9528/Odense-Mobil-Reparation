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

    public CustomersPage(CustomersPageArguments arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);

        DataContext = new CustomersPageViewModel(arguments);
        Margin = new Thickness(0);

        var viewModel = (CustomersPageViewModel) DataContext;
        var logic = new CustomersPageLogic(viewModel);
        var ui = new CustomersPageUi(logic, viewModel);

        Child = ui.CreateContentGrid();
    }

    internal sealed record CustomersPageArguments(
        IEntityQueryService<Customer, SearchableCustomer> CustomerQueryService,
        DispatcherQueue DispatcherQueue,
        ILoggerFactory LoggerFactory,
        INavigationService NavigationService);
}
