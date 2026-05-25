using Repair.Frontend.Presentation.Core;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderDetailsPage
{
    private sealed class OrderDetailsPageLogic(OrderDetailsPageViewModel viewModel)
        : BaseLogic<OrderDetailsPageViewModel>(viewModel)
    {
        internal void EditToggleChanged(object sender, RoutedEventArgs e)
        {
            // TODO: Enable / disable editing order details.
        }

        internal void CustomerClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Navigate to CustomerDetailsPage for the order's customer.
        }

        internal void SaveClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Save order changes.
        }
    }
}