using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Frontend.Presentation.Core;
using Repair.Models.Entity.ComplexSearchable;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class CustomersGrid
{
    internal sealed partial class CustomersGridLogic : BaseLogic<CustomersGridViewModel>
    {
        private readonly IEntityQueryService<Customer, SearchableCustomer> queryService;
        private readonly DispatcherQueue dispatcherQueue;
        private readonly ILogger<CustomersGridLogic> logger;

        public CustomersGridLogic(CustomersGridViewModel viewModel) : base(viewModel)
        {
            queryService = ViewModel.Arguments.QueryService;
            dispatcherQueue = ViewModel.Arguments.DispatcherQueue;
            logger = ViewModel.Arguments.LoggerFactory.CreateLogger<CustomersGridLogic>();

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
                logger.LogError(exe, "Exception caught during refresh of Customers");
            }
        }

        public async Task RefreshCustomers()
        {
            RememberSelectedCustomer();
            var customers = await queryService.GetEntitiesComplex(CreateSearchableCustomer());

            dispatcherQueue.TryEnqueue(() =>
            {
                ViewModel.Customers.Clear();

                foreach (Customer customer in customers)
                {
                    ViewModel.Customers.Add(customer);
                }

                RestoreSelectedCustomer();
            });
        }

        private void RememberSelectedCustomer()
        {
            if (ViewModel.DataGrid.SelectedItem is not Customer customer)
            {
                return;
            }

            ViewModel.SelectedCustomerId = customer.Id;
        }

        private void RestoreSelectedCustomer()
        {
            if (ViewModel.SelectedCustomerId <= 0)
            {
                return;
            }

            Customer? selectedCustomer = ViewModel.Customers.FirstOrDefault(x => x.Id == ViewModel.SelectedCustomerId);

            if (selectedCustomer is null)
            {
                ViewModel.DataGrid.SelectedItem = null;
                return;
            }

            ViewModel.DataGrid.SelectedItem = selectedCustomer;
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
    }
}
