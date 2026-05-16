using Repair.Frontend.Presentation.Core;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerDetailsPage
{
    private sealed class CustomerDetailsPageLogic : BaseLogic<CustomerDetailsPageViewModel>
    {
        public CustomerDetailsPageLogic(CustomerDetailsPageViewModel viewModel) : base(viewModel)
        {
        }

        internal void EditToggleChanged(object sender, RoutedEventArgs e)
        {
            // TODO: Enable / disable editing customer details.
        }

        internal void OrderSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO: Update search text and refresh shown customer orders.
        }

        internal void OrderClicked(object sender, ItemClickEventArgs e)
        {
            // TODO: Navigate to OrderDetailsPage for the selected order.
        }

        internal void SaveClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Save customer changes.
        }
    }
}
