using System.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Dispatching;
using Repair.Abstractions.Entity.Model;
using Repair.Abstractions.Persistence;
using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Services;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomersPage
{
    private sealed class CustomersPageLogic : BaseLogic<CustomersPageViewModel>
    {
        private readonly IEntityQueryService<Customer, SearchableCustomer> queryService;
        private readonly DispatcherQueue dispatcherQueue;
        private readonly ILogger<CustomersPageLogic> logger;
        private readonly INavigationService navigationService;

        public CustomersPageLogic(CustomersPageViewModel viewModel) : base(viewModel)
        {
            queryService = ViewModel.Arguments.CustomerQueryService;
            dispatcherQueue = ViewModel.Arguments.DispatcherQueue;
            logger = ViewModel.Arguments.LoggerFactory.CreateLogger<CustomersPageLogic>();
            navigationService = ViewModel.Arguments.NavigationService;
        }

        public void CreateCustomerClicked(object sender, RoutedEventArgs e)
        {
            CustomerCreationPage.CustomerCreationPageArguments arguments =
                GetArgumentsFactory().CreateCustomerCreationPageArguments();

            var creationPage = new CustomerCreationPage(arguments);
            navigationService.NavigateTo(creationPage, "Customer Creation Page");
        }

        public void CustomerClicked(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not DataGrid dataGrid)
            {
                return;
            }

            if (dataGrid.SelectedItem is not Customer customer)
            {
                return;
            }

            CustomerDetailsPage.CustomerDetailsPageArguments arguments =
                GetArgumentsFactory().CreateCustomerDetailsPageArguments(customer.Id);

            var detailPage = new CustomerDetailsPage(arguments);
            navigationService.NavigateTo(detailPage, "Customer Details Page");

            dataGrid.SelectedItem = null;
        }
    }
}
