using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Frontend.Presentation.Core;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerDetailsPage
{
    private sealed class CustomerDetailsPageLogic : BaseLogic<CustomerDetailsPageViewModel>
    {
        private readonly IEntityQueryService<Customer, SearchableCustomer> queryService;
        private readonly DispatcherQueue dispatcherQueue;

        public CustomerDetailsPageLogic(CustomerDetailsPageViewModel viewModel) : base(viewModel)
        {
            queryService = ViewModel.Arguments.CustomerQueryService;
            dispatcherQueue = ViewModel.Arguments.DispatcherQueue;

            ViewModel.CustomerChanged += OnCustomerChanged;
        }

        private void OnCustomerChanged(object? sender, EventArgs e)
        {
            ViewModel.CustomerEditor.ViewModel.Name = ViewModel.Customer.Name;
            ViewModel.CustomerEditor.ViewModel.Email = ViewModel.Customer.Email;
            ViewModel.CustomerEditor.ViewModel.Phone = ViewModel.Customer.Phone;
        }

        internal async Task RefreshCustomer()
        {
            Customer? customer = await queryService.GetEntity(CreateSearchableCustomer());

            if (customer is null)
            {
                return;
            }

            dispatcherQueue.TryEnqueue(() => { ViewModel.Customer = customer; });
        }

        private SearchableCustomer CreateSearchableCustomer()
        {
            return new SearchableCustomer {Id = ViewModel.Arguments.CustomerId,};
        }

        internal void EditToggleChanged(object sender, RoutedEventArgs e)
        {
            // TODO: Enable / disable editing customer details.
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
