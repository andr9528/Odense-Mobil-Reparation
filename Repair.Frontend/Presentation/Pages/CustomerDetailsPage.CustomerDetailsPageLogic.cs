using CommunityToolkit.WinUI.UI.Controls;
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
        private readonly ILogger<CustomerDetailsPageLogic> logger;

        public CustomerDetailsPageLogic(CustomerDetailsPageViewModel viewModel) : base(viewModel)
        {
            queryService = ViewModel.Arguments.CustomerQueryService;
            dispatcherQueue = ViewModel.Arguments.DispatcherQueue;
            logger = ViewModel.Arguments.LoggerFactory.CreateLogger<CustomerDetailsPageLogic>();
        }

        internal async Task RefreshCustomer()
        {
            Customer? customer = await queryService.GetEntity(CreateSearchableCustomer());

            if (customer is null)
            {
                return;
            }

            dispatcherQueue.TryEnqueue(() =>
            {
                ViewModel.Customer = customer;
                ApplyCustomerToEditor();
            });
        }

        private SearchableCustomer CreateSearchableCustomer()
        {
            return new SearchableCustomer {Id = ViewModel.Arguments.CustomerId,};
        }

        internal void RegisterCustomerEditorEvents()
        {
            ViewModel.CustomerEditor.ViewModel.NameChanged += CustomerEditorChanged;
            ViewModel.CustomerEditor.ViewModel.PhoneChanged += CustomerEditorChanged;
            ViewModel.CustomerEditor.ViewModel.EmailChanged += CustomerEditorChanged;
        }

        private void CustomerEditorChanged(object? sender, EventArgs e)
        {
            UpdateHasChanges();
        }

        internal void EditCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            bool isEditing = ViewModel.EditCheckBox.IsChecked == true;

            ViewModel.IsEditing = isEditing;
            ViewModel.CustomerEditor.ViewModel.IsReadOnly = !isEditing;
        }

        internal async Task SaveClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ViewModel.HasChanges)
                {
                    ViewModel.Arguments.NavigationService.NavigateBack();
                    return;
                }

                ApplyEditorValuesToCustomer();

                await queryService.UpdateEntity(ViewModel.Customer);

                UpdateHasChanges();
                DisableEditing();
            }
            catch (Exception exe)
            {
                logger.LogError(exe, $"Failed to save changes to the customer.");
            }
        }

        internal void CancelClicked(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.HasChanges)
            {
                ViewModel.Arguments.NavigationService.NavigateBack();
                return;
            }

            ApplyCustomerToEditor();
            DisableEditing();
        }

        private void ApplyEditorValuesToCustomer()
        {
            logger.LogDebug("Applying customer changes. Id={CustomerId}", ViewModel.Customer.Id);

            logger.LogDebug("Name: '{OldValue}' -> '{NewValue}'", ViewModel.Customer.Name,
                ViewModel.CustomerEditor.ViewModel.Name);

            logger.LogDebug("Email: '{OldValue}' -> '{NewValue}'", ViewModel.Customer.Email,
                ViewModel.CustomerEditor.ViewModel.Email);

            logger.LogDebug("Phone: '{OldValue}' -> '{NewValue}'", ViewModel.Customer.Phone,
                ViewModel.CustomerEditor.ViewModel.Phone);

            ViewModel.Customer.Name = ViewModel.CustomerEditor.ViewModel.Name;
            ViewModel.Customer.Email = ViewModel.CustomerEditor.ViewModel.Email;
            ViewModel.Customer.Phone = ViewModel.CustomerEditor.ViewModel.Phone;
        }

        private void DisableEditing()
        {
            ViewModel.IsEditing = false;
            ViewModel.EditCheckBox.IsChecked = false;
            ViewModel.CustomerEditor.ViewModel.IsReadOnly = true;
        }

        private void ApplyCustomerToEditor()
        {
            ViewModel.CustomerEditor.ViewModel.Name = ViewModel.Customer.Name;
            ViewModel.CustomerEditor.ViewModel.Email = ViewModel.Customer.Email;
            ViewModel.CustomerEditor.ViewModel.Phone = ViewModel.Customer.Phone;
        }

        private void UpdateHasChanges()
        {
            ViewModel.HasChanges = ViewModel.CustomerEditor.ViewModel.Name != ViewModel.Customer.Name ||
                                   ViewModel.CustomerEditor.ViewModel.Email != ViewModel.Customer.Email ||
                                   ViewModel.CustomerEditor.ViewModel.Phone != ViewModel.Customer.Phone;

            ViewModel.SaveButtonText = ViewModel.HasChanges ? "Save" : "Okay";
            ViewModel.CancelButtonText = ViewModel.HasChanges ? "Cancel" : "Back";
        }

        public void OrderClicked(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not DataGrid dataGrid)
            {
                return;
            }

            if (dataGrid.SelectedItem is not Order order)
            {
                return;
            }

            // TODO: Navigate to OrderDetailsPage for the selected order.
            // order.Id can be used here.
        }
    }
}
