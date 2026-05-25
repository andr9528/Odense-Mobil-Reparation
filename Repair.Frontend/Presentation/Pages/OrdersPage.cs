using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

/// <summary>
/// Shows all orders and allows narrowing the list through search.
/// </summary>
internal sealed partial class OrdersPage : Border
{
    public OrdersPage(OrdersPageArguments arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);

        DataContext = new OrdersPageViewModel(arguments);
        Margin = new Thickness(0);

        var viewModel = (OrdersPageViewModel) DataContext;
        var logic = new OrdersPageLogic(viewModel);
        var ui = new OrdersPageUi(logic, viewModel);

        Child = ui.CreateContentGrid();
    }

    internal sealed record OrdersPageArguments(
        IEntityQueryService<Order, SearchableOrder> OrderQueryService,
        DispatcherQueue DispatcherQueue,
        ILoggerFactory LoggerFactory);
}
