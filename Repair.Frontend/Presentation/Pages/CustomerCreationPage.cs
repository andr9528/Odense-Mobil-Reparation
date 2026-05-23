using Repair.Abstractions.Persistence;
using Repair.Frontend.Abstraction;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

/// <summary>
/// Creates a new customer. Editing is always enabled.
/// </summary>
internal sealed partial class CustomerCreationPage : Border
{
    public CustomerCreationPage(
        IEntityQueryService<Customer, SearchableCustomer> queryService, INavigationService navigationService)
    {
        DataContext = new CustomerCreationPageViewModel();
        Margin = new Thickness(0);

        var viewModel = (CustomerCreationPageViewModel) DataContext;
        var logic = new CustomerCreationPageLogic(viewModel, queryService, navigationService);
        var ui = new CustomerCreationPageUi(logic, viewModel);

        Child = ui.CreateContentGrid();
    }
}
