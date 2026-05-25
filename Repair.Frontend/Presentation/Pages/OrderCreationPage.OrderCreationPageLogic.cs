using Repair.Frontend.Presentation.Core;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderCreationPage
{
    private sealed class OrderCreationPageLogic(OrderCreationPageViewModel viewModel)
        : BaseLogic<OrderCreationPageViewModel>(viewModel)
    {
        internal void CustomerSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO: Update search text and refresh selectable customers.
        }

        internal void CustomerClicked(object sender, ItemClickEventArgs e)
        {
            // TODO: Select the customer for the order.
        }

        internal void SaveClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Create order and navigate to OrderDetailsPage.
        }

        internal void CancelClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Navigate back without creating the order.
        }
    }
}
