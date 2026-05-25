using Repair.Abstractions.Persistence;
using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Pieces;
using Repair.Frontend.Services;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerCreationPage
{
    private sealed class CustomerCreationPageLogic : BaseLogic<CustomerCreationPageViewModel>
    {
        private readonly IEntityQueryService<Customer, SearchableCustomer> queryService;
        private readonly INavigationService navigationService;

        public CustomerCreationPageLogic(CustomerCreationPageViewModel viewModel) : base(viewModel)
        {
            queryService = ViewModel.Arguments.CustomerQueryService;
            navigationService = ViewModel.Arguments.NavigationService;
        }

        internal async Task SaveClicked(object sender, RoutedEventArgs e)
        {
            if (!IsUserInputValid())
            {
                return;
            }

            Customer newCustomer = BuildNewCustomer();
            await queryService.AddEntity(newCustomer);
            CustomerDetailsPage.CustomerDetailsPageArguments arguments = App.Startup.ServiceProvider
                .GetRequiredService<ArgumentsFactory>().CreateCustomerDetailsPageArguments(newCustomer.Id);

            var details = new CustomerDetailsPage(arguments);
            navigationService.NavigateTo(details);
        }

        private bool IsUserInputValid()
        {
            CustomerEditor.CustomerEditorViewModel customer = ViewModel.CustomerEditor.ViewModel;

            return !string.IsNullOrWhiteSpace(customer.Name) && !string.IsNullOrWhiteSpace(customer.Phone) &&
                   !string.IsNullOrWhiteSpace(customer.Email) && customer.Email.Count(x => x == '@') == 1;
        }

        private Customer BuildNewCustomer()
        {
            return new Customer
            {
                Name = ViewModel.CustomerEditor.ViewModel.Name,
                Phone = ViewModel.CustomerEditor.ViewModel.Phone,
                Email = ViewModel.CustomerEditor.ViewModel.Email,
            };
        }

        internal void CancelClicked(object sender, RoutedEventArgs e)
        {
            navigationService.NavigateBack();
        }
    }
}
