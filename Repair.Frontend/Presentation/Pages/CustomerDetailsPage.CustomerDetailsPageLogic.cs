using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Core.Details;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerDetailsPage
{
    private sealed class CustomerDetailsPageLogic : BaseDetailsPageLogic<CustomerDetailsPageViewModel>
    {
        private readonly IEntityQueryService<Customer, SearchableCustomer> queryService;
        private readonly IUiDispatcher uiDispatcher;
        private readonly ILogger<CustomerDetailsPageLogic> logger;

        public CustomerDetailsPageLogic(CustomerDetailsPageViewModel viewModel) : base(viewModel)
        {
            queryService = ViewModel.Arguments.CustomerQueryService;
            uiDispatcher = ViewModel.Arguments.UiDispatcher;
            logger = ViewModel.Arguments.LoggerFactory.CreateLogger<CustomerDetailsPageLogic>();
        }

        internal async Task RefreshCustomer()
        {
            Customer? customer = await queryService.GetEntity(CreateSearchableCustomer());

            if (customer is null)
            {
                return;
            }

            uiDispatcher.TryEnqueue(() =>
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

        private void ApplyCustomerToEditor()
        {
            ViewModel.CustomerEditor.ViewModel.Name = ViewModel.Customer.Name;
            ViewModel.CustomerEditor.ViewModel.Email = ViewModel.Customer.Email;
            ViewModel.CustomerEditor.ViewModel.Phone = ViewModel.Customer.Phone;
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

            OrderDetailsPage.OrderDetailsPageArguments arguments =
                GetArgumentsFactory().CreateOrderDetailsPageArguments(order.Id);
            var page = new OrderDetailsPage(arguments);

            ViewModel.Arguments.NavigationService.NavigateTo(page, "Order Details Page");
        }

        internal void CreateOrderClicked(object sender, RoutedEventArgs e)
        {
            OrderCreationPage.OrderCreationPageArguments arguments = GetArgumentsFactory()
                .CreateOrderCreationPageArguments(ViewModel.Customer.Id);

            var page = new OrderCreationPage(arguments);

            ViewModel.Arguments.NavigationService.NavigateTo(page, "Create Order");
        }

        internal override async Task DeleteClicked(object sender, RoutedEventArgs e)
        {
            ContentDialogResult result = await ShowDeleteConfirmation("Delete customer?",
                "This will permanently delete the current customer and all orders belonging to that customer.");

            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            await queryService.DeleteEntityById(ViewModel.Customer.Id);

            logger.LogInformation("Deleted customer {CustomerId}", ViewModel.Customer.Id);

            ViewModel.Arguments.NavigationService.NavigateBack();
        }

        protected override void SetEditorReadOnly(bool isReadOnly)
        {
            ViewModel.CustomerEditor.ViewModel.IsReadOnly = isReadOnly;
        }

        protected override async Task SaveChanges()
        {
            ApplyEditorValuesToCustomer();

            await queryService.UpdateEntity(ViewModel.Customer);
            logger.LogInformation("Saved changes to customer {CustomerId}.", ViewModel.Customer.Id);
        }

        protected override void ApplyEntityToEditor()
        {
            ApplyCustomerToEditor();
        }

        protected override void UpdateHasChanges()
        {
            ViewModel.HasChanges = ViewModel.CustomerEditor.ViewModel.Name != ViewModel.Customer.Name ||
                                   ViewModel.CustomerEditor.ViewModel.Email != ViewModel.Customer.Email ||
                                   ViewModel.CustomerEditor.ViewModel.Phone != ViewModel.Customer.Phone;

            UpdateSaveAndCancelText();
        }

        protected override void NavigateBack()
        {
            ViewModel.Arguments.NavigationService.NavigateBack();
        }

        protected override void LogSaveError(Exception exception)
        {
            logger.LogError(exception, "Failed to save changes to the customer.");
        }

        /// <inheritdoc />
        protected internal override async void PrintClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ViewModel.IsPrinting)
                {
                    logger.LogDebug("Ignoring print request while another print is already running.");
                    return;
                }

                ViewModel.IsPrinting = true;
                ViewModel.PrintButtonText = "Printing...";

                await ViewModel.Arguments.ReportService.CreateReport(ViewModel.Customer);
                logger.LogInformation("Generated customer report for customer {CustomerId}.", ViewModel.Customer.Id);
            }
            catch (Exception exe)
            {
                logger.LogError(exe, "Failed to generate customer report.");
            }
            finally
            {
                await Task.Delay(2000);

                ViewModel.IsPrinting = false;
                ViewModel.PrintButtonText = "Print";
            }
        }
    }
}
