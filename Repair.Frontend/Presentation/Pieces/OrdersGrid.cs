using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrdersGrid : Border
{
    internal OrdersGridViewModel ViewModel => (OrdersGridViewModel) DataContext;

    public OrdersGrid(OrdersGridArguments arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);

        DataContext = new OrdersGridViewModel(arguments);

        var logic = new OrdersGridLogic(ViewModel);
        var ui = new OrdersGridUi(logic, ViewModel);

        Child = ui.CreateContentGrid();

        _ = logic.RefreshOrders();
    }

    internal record OrdersGridArguments(
        IEntityQueryService<Order, SearchableOrder> OrderQueryService,
        DispatcherQueue DispatcherQueue,
        ILoggerFactory LoggerFactory,
        int CustomerId = 0)
    {
    }
}
