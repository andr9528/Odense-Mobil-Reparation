using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrderGrid : Border
{
    internal OrderGridViewModel ViewModel => (OrderGridViewModel) DataContext;

    public OrderGrid(OrderGridArguments arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);

        DataContext = new OrderGridViewModel(arguments);

        var logic = new OrderGridLogic(ViewModel);
        var ui = new OrderGridUi(logic, ViewModel);

        Child = ui.CreateContentGrid();

        _ = logic.RefreshOrders();
    }

    internal record OrderGridArguments(
        IEntityQueryService<Order, SearchableOrder> OrderQueryService,
        DispatcherQueue DispatcherQueue,
        ILoggerFactory LoggerFactory,
        int CustomerId = 0)
    {
    }
}
