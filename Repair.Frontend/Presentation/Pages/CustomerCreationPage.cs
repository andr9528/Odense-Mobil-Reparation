using Repair.Abstractions.Persistence;
using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Factory;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

/// <summary>
/// Creates a new customer. Editing is always enabled.
/// </summary>
internal sealed partial class CustomerCreationPage : Border
{
    public CustomerCreationPage(CustomerCreationPageArguments arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);
        this.ConfigurePageBorder();

        DataContext = new CustomerCreationPageViewModel(arguments);

        var viewModel = (CustomerCreationPageViewModel) DataContext;
        var logic = new CustomerCreationPageLogic(viewModel);
        var ui = new CustomerCreationPageUi(logic, viewModel);

        Child = ui.CreateContentGrid();
    }

    internal sealed record CustomerCreationPageArguments(
        IEntityQueryService<Customer, SearchableCustomer> CustomerQueryService,
        INavigationService NavigationService);
}
