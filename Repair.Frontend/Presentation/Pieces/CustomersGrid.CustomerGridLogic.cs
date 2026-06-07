using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;
using Repair.Models.Entity.ComplexSearchable;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;
using Repair.Models.Extensions;

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

            ComplexSearchableCustomer searchable = CreateSearchableCustomer();

            List<Customer> customers = (await queryService.GetEntitiesComplex(searchable)).ToList();
            customers = ViewModel.DataGrid.ApplyCurrentSort(customers).ToList();

            logger.LogDebug("Customers query returned {CustomerCount} customers.", customers.Count);

            dispatcherQueue.TryEnqueue(() =>
            {
                logger.LogDebug("Updating Customers collection. Existing count: {ExistingCount}",
                    ViewModel.Customers.Count);

                ViewModel.Customers.ReplaceItems(customers);
                ViewModel.DataGrid.Refresh();

                RestoreSelectedCustomer();

                logger.LogDebug(
                    "Customers collection updated. New count: {NewCount}, SelectedCustomerId: {SelectedCustomerId}, SelectedCustomer: '{SelectedCustomerName}'",
                    ViewModel.Customers.Count, ViewModel.SelectedCustomerId, ViewModel.SelectedCustomer?.Name);
            });
        }

        private void RememberSelectedCustomer()
        {
            if (ViewModel.SelectedCustomer is null)
            {
                return;
            }

            ViewModel.SelectedCustomerId = ViewModel.SelectedCustomer.Id;
        }

        private void RestoreSelectedCustomer()
        {
            ViewModel.SelectedCustomer = ViewModel.Customers.FirstOrDefault(x => x.Id == ViewModel.SelectedCustomerId);
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
