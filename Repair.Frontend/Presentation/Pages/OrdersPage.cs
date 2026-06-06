using CommunityToolkit.WinUI.UI.Controls;
using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Factory;

namespace Repair.Frontend.Presentation.Pages;

/// <summary>
/// Shows all orders and allows narrowing the list through search.
/// </summary>
internal sealed partial class OrdersPage : Border, INavigationRefreshable
{
    private OrdersPageViewModel ViewModel => (OrdersPageViewModel) DataContext;

    public OrdersPage(OrdersPageArguments arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);
        this.ConfigurePageBorder();

        DataContext = new OrdersPageViewModel(arguments);
        var logic = new OrdersPageLogic(ViewModel);
        var ui = new OrdersPageUi(logic, ViewModel);

        Child = ui.CreateContentGrid();
    }

    internal sealed record OrdersPageArguments(INavigationService NavigationService);

    /// <inheritdoc />
    public void RefreshAfterNavigation()
    {
        ViewModel.OrdersGrid.RefreshAfterNavigation();
    }
}
