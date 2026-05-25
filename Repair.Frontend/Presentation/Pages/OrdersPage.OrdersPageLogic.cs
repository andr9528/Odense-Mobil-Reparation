using CommunityToolkit.WinUI.UI.Controls;
using Repair.Frontend.Presentation.Core;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrdersPage
{
    private sealed class OrdersPageLogic(OrdersPageViewModel viewModel) : BaseLogic<OrdersPageViewModel>(viewModel)
    {
        internal void OrderClicked(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not DataGrid dataGrid)
            {
                return;
            }

            if (dataGrid.SelectedItem is not Order order)
            {
                return;
            }

            // TODO: Navigate to OrderDetailsPage for the selected order.
            // order.Id can be used here.
        }

        internal void CreateOrderClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Navigate to OrderCreationPage.
        }
    }
}
