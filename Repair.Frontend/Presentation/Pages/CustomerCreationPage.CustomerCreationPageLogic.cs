using Repair.Frontend.Presentation.Core;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerCreationPage
{
    private sealed class CustomerCreationPageLogic : BaseLogic<CustomerCreationPageViewModel>
    {
        public CustomerCreationPageLogic(CustomerCreationPageViewModel viewModel) : base(viewModel)
        {
        }

        internal void SaveClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Create customer and navigate to CustomerDetailsPage.
        }

        internal void CancelClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Navigate back without creating the customer.
        }
    }
}
