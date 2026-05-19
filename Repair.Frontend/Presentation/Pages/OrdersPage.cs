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
    public OrdersPage(
        IEntityQueryService<Order, SearchableOrder> orderQueryService, DispatcherQueue dispatcherQueue,
        ILoggerFactory loggerFactory)
    {
        DataContext = new OrdersPageViewModel();
        Margin = new Thickness(0);

        var viewModel = (OrdersPageViewModel) DataContext;
        var logic = new OrdersPageLogic(viewModel);
        var ui = new OrdersPageUi(logic, viewModel, orderQueryService, dispatcherQueue, loggerFactory);

        Child = ui.CreateContentGrid();
    }
}
