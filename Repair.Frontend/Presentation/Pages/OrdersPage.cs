using CommunityToolkit.WinUI.UI.Controls;
using Repair.Frontend.Abstraction;

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

        DataContext = new OrdersPageViewModel(arguments);
        Margin = new Thickness(0);

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
