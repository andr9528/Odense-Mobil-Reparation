using Repair.Abstractions.Persistence;
using Repair.Frontend.Abstraction;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

/// <summary>
/// Creates a new order. Editing is always enabled.
/// </summary>
internal sealed partial class OrderCreationPage : Border
{
    public OrderCreationPage(OrderCreationPageArguments arguments)
    {
        DataContext = new OrderCreationPageViewModel(arguments);
        Margin = new Thickness(0);

        var viewModel = (OrderCreationPageViewModel) DataContext;
        var logic = new OrderCreationPageLogic(viewModel);
        var ui = new OrderCreationPageUi(logic, viewModel);

        Child = ui.CreateContentGrid();
    }

    internal sealed record OrderCreationPageArguments(
        IEntityQueryService<Order, SearchableOrder> OrderQueryService,
        INavigationService NavigationService);
}
