using System.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Dispatching;
using Repair.Abstractions.Entity.Model;
using Repair.Abstractions.Persistence;
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

        public CustomersPageLogic(
            IEntityQueryService<Customer, SearchableCustomer> customerQueryService, CustomersPageViewModel viewModel,
            DispatcherQueue dispatcherQueue, ILogger<CustomersPageLogic> logger) : base(viewModel)
        {
            this.customerQueryService = customerQueryService;
            this.dispatcherQueue = dispatcherQueue;
            this.logger = logger;

            ViewModel.PropertyChanged += HandleViewModelPropertyChanged;
        }

        private async void HandleViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName is nameof(CustomersPageViewModel.NameSearchText)
                    or nameof(CustomersPageViewModel.PhoneSearchText) or nameof(CustomersPageViewModel.EmailSearchText)
                    or nameof(CustomersPageViewModel.UseFuzzySearch))
                {
                    await RefreshCustomers();
                }
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
                complex.Name = ViewModel.NameSearchText;
                complex.Phone = ViewModel.PhoneSearchText;
                complex.Email = ViewModel.EmailSearchText;
            }
            else
            {
                complex.Searchable.Name = ViewModel.NameSearchText;
                complex.Searchable.Phone = ViewModel.PhoneSearchText;
                complex.Searchable.Email = ViewModel.EmailSearchText;
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

            // TODO: Navigate to CustomerDetailsPage for the selected Customer.
            // customer.Id can be used here.
        }
    }
}
