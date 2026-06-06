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
        private readonly ILogger<CustomerCreationPageLogic> logger;

        public CustomerCreationPageLogic(CustomerCreationPageViewModel viewModel) : base(viewModel)
        {
            queryService = ViewModel.Arguments.CustomerQueryService;
            navigationService = ViewModel.Arguments.NavigationService;
            logger = ViewModel.Arguments.LoggerFactory.CreateLogger<CustomerCreationPageLogic>();
        }

        internal async Task SaveClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsUserInputValid())
                {
                    return;
                }

                Customer newCustomer = BuildNewCustomer();
                await queryService.AddEntity(newCustomer);
                CustomerDetailsPage.CustomerDetailsPageArguments arguments =
                    GetArgumentsFactory().CreateCustomerDetailsPageArguments(newCustomer.Id);

                var details = new CustomerDetailsPage(arguments);
                navigationService.NavigateTo(details, "Customer Details Page");
            }
            catch (Exception exe)
            {
                logger.LogError(exe, $"Caught exception while trying to create a Customer.");
            }
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
