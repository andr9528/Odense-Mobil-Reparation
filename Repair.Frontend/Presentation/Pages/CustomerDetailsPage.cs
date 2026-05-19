using Repair.Abstractions.Persistence;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

/// <summary>
/// Shows customer details and the customer's orders.
/// </summary>
internal sealed partial class CustomerDetailsPage : Border
{
    public CustomerDetailsPage(int customerId, IEntityQueryService<Customer, SearchableCustomer> queryService)
    {
        DataContext = new CustomerDetailsPageViewModel();
        Margin = new Thickness(0);

        var viewModel = (CustomerDetailsPageViewModel) DataContext;
        var logic = new CustomerDetailsPageLogic(viewModel);
        var ui = new CustomerDetailsPageUi(logic, viewModel);

        Child = ui.CreateContentGrid();
    }
}
