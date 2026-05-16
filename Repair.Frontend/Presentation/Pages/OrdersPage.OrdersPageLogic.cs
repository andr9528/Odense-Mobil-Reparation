using Repair.Frontend.Presentation.Core;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrdersPage
{
    private sealed class OrdersPageLogic : BaseLogic<OrdersPageViewModel>
    {
        public OrdersPageLogic(OrdersPageViewModel viewModel) : base(viewModel)
        {
        }

        internal void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO: Update search text and refresh shown orders.
        }

        internal void OrderClicked(object sender, ItemClickEventArgs e)
        {
            // TODO: Navigate to OrderDetailsPage for the selected order.
        }

        internal void CreateOrderClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Navigate to OrderCreationPage.
        }
    }
}