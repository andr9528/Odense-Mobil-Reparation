using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrderGrid : Border
{
    internal OrderGridViewModel ViewModel => (OrderGridViewModel) DataContext;

    public OrderGrid(
        IEntityQueryService<Order, SearchableOrder> orderQueryService, DispatcherQueue dispatcherQueue,
        int customerId = 0)
    {
        ArgumentNullException.ThrowIfNull(orderQueryService);

        DataContext = new OrderGridViewModel(customerId);

        var viewModel = (OrderGridViewModel) DataContext;
        var logic = new OrderGridLogic(orderQueryService, viewModel, dispatcherQueue);
        var ui = new OrderGridUi(logic, viewModel);

        Child = ui.CreateContentGrid();

        _ = logic.RefreshOrders();
    }
}
