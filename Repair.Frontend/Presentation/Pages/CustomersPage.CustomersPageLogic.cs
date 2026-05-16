using Repair.Frontend.Presentation.Core;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomersPage
{
    private sealed class CustomersPageLogic : BaseLogic<CustomersPageViewModel>
    {
        public CustomersPageLogic(CustomersPageViewModel viewModel) : base(viewModel)
        {
        }

        internal void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO: Update search text and refresh shown customers.
        }

        internal void CustomerClicked(object sender, ItemClickEventArgs e)
        {
            // TODO: Navigate to CustomerDetailsPage for the selected customer.
        }

        internal void CreateCustomerClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Navigate to CustomerCreationPage.
        }
    }
}
