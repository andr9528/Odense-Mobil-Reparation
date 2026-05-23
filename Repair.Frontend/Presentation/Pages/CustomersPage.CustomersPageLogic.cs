using System.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Dispatching;
using Repair.Abstractions.Entity.Model;
using Repair.Abstractions.Persistence;
using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Core;
using Repair.Models.Entity.ComplexSearchable;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomersPage
{
    private sealed class CustomersPageLogic : BaseLogic<CustomersPageViewModel>
    {
        private readonly IEntityQueryService<Customer, SearchableCustomer> customerQueryService;
        private readonly DispatcherQueue dispatcherQueue;
        private readonly ILogger<CustomersPageLogic> logger;
        private readonly INavigationService navigationService;

        public CustomersPageLogic(
            IEntityQueryService<Customer, SearchableCustomer> customerQueryService, CustomersPageViewModel viewModel,
            DispatcherQueue dispatcherQueue, ILogger<CustomersPageLogic> logger,
            INavigationService navigationService) : base(viewModel)
        {
            this.customerQueryService = customerQueryService;
            this.dispatcherQueue = dispatcherQueue;
            this.logger = logger;
            this.navigationService = navigationService;

            ViewModel.SearchChanged += SearchChanged;
        }

        private async void SearchChanged(object? sender, EventArgs e)
        {
            try
            {
                await RefreshCustomers();
            }
            catch (Exception exe)
            {
                logger.LogError(exe, $"Exception caught during refresh of Orders");
            }
        }

        internal async Task RefreshCustomers()
        {
            var customers = await customerQueryService.GetEntitiesComplex(CreateSearchableCustomer());

            dispatcherQueue.TryEnqueue(() =>
            {
                ViewModel.Customers.Clear();

                foreach (Customer customer in customers)
                {
                    ViewModel.Customers.Add(customer);
                }
            });
        }

        private ComplexSearchableCustomer CreateSearchableCustomer()
        {
            var complex = new ComplexSearchableCustomer();

            if (ViewModel.UseFuzzySearch)
            {
                complex.Name = ViewModel.CustomerEditor.ViewModel.Name;
                complex.Phone = ViewModel.CustomerEditor.ViewModel.Phone;
                complex.Email = ViewModel.CustomerEditor.ViewModel.Email;
            }
            else
            {
                complex.Searchable.Name = ViewModel.CustomerEditor.ViewModel.Name;
                complex.Searchable.Phone = ViewModel.CustomerEditor.ViewModel.Phone;
                complex.Searchable.Email = ViewModel.CustomerEditor.ViewModel.Email;
            }

            return complex;
        }

        public void CreateCustomerClicked(object sender, RoutedEventArgs e)
        {
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

            var detailPage = new CustomerDetailsPage(customer.Id, customerQueryService);
            navigationService.NavigateTo(detailPage);
        }
    }
}
