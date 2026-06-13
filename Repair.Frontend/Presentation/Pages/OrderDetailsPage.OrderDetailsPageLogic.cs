using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Core.Details;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderDetailsPage
{
    private sealed class OrderDetailsPageLogic : BaseDetailsPageLogic<OrderDetailsPageViewModel>
    {
        private readonly IEntityQueryService<Order, SearchableOrder> queryService;
        private readonly DispatcherQueue dispatcherQueue;
        private readonly ILogger<OrderDetailsPageLogic> logger;

        public OrderDetailsPageLogic(OrderDetailsPageViewModel viewModel) : base(viewModel)
        {
            queryService = ViewModel.Arguments.OrderQueryService;
            dispatcherQueue = ViewModel.Arguments.DispatcherQueue;
            logger = ViewModel.Arguments.LoggerFactory.CreateLogger<OrderDetailsPageLogic>();
        }

        internal async Task RefreshOrder()
        {
            Order? order = await queryService.GetEntity(CreateSearchableOrder());

            if (order is null)
            {
                return;
            }

            dispatcherQueue.TryEnqueue(() =>
            {
                ViewModel.Order = order;
                ApplyOrderToEditor();
                UpdateHasChanges();
            });
        }

        private SearchableOrder CreateSearchableOrder()
        {
            return new SearchableOrder {Id = ViewModel.Arguments.OrderId,};
        }

        internal void RegisterOrderEditorEvents()
        {
            ViewModel.OrderEditor.ViewModel.HandInWhenPicker.ViewModel.SelectedDateTimeChanged += OrderEditorChanged;
            ViewModel.OrderEditor.ViewModel.ReturnedWhenPicker.ViewModel.SelectedDateTimeChanged += OrderEditorChanged;
            ViewModel.OrderEditor.ViewModel.HandInWhatChanged += OrderEditorChanged;
            ViewModel.OrderEditor.ViewModel.RepairWhatChanged += OrderEditorChanged;
            ViewModel.OrderEditor.ViewModel.IsOrderCompleteChanged += OrderEditorChanged;
            ViewModel.OrderEditor.ViewModel.BorrowedPhoneChanged += OrderEditorChanged;
            ViewModel.OrderEditor.ViewModel.CustomersGrid.ViewModel.CustomerSelectionChanged += OrderEditorChanged;
        }

        private void OrderEditorChanged(object? sender, EventArgs e)
        {
            UpdateHasChanges();
        }

        internal async void PrintClicked(object sender, RoutedEventArgs e)
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

                await ViewModel.Arguments.ReportService.CreateReport(ViewModel.Order);
            }
            catch (Exception exe)
            {
                logger.LogError(exe, "Failed to generate report.");
            }
            finally
            {
                await Task.Delay(2000);

                ViewModel.IsPrinting = false;
                ViewModel.PrintButtonText = "Print";
            }
        }

        private void ApplyEditorValuesToOrder()
        {
            logger.LogDebug("Applying order changes. OrderId={OrderId}", ViewModel.Order.Id);

            logger.LogDebug("HandInWhen: '{OldValue}' -> '{NewValue}'", ViewModel.Order.HandInWhen,
                ViewModel.OrderEditor.ViewModel.HandInWhen);

            logger.LogDebug("ReturnedWhen: '{OldValue}' -> '{NewValue}'", ViewModel.Order.ReturnedWhen,
                ViewModel.OrderEditor.ViewModel.ReturnedWhen);

            logger.LogDebug("HandInWhat: '{OldValue}' -> '{NewValue}'", ViewModel.Order.HandInWhat,
                ViewModel.OrderEditor.ViewModel.HandInWhat);

            logger.LogDebug("RepairWhat: '{OldValue}' -> '{NewValue}'", ViewModel.Order.RepairWhat,
                ViewModel.OrderEditor.ViewModel.RepairWhat);

            logger.LogDebug("IsOrderComplete: '{OldValue}' -> '{NewValue}'", ViewModel.Order.IsOrderComplete,
                ViewModel.OrderEditor.ViewModel.IsOrderComplete);

            logger.LogDebug("BorrowedPhone: '{OldValue}' -> '{NewValue}'", ViewModel.Order.BorrowedPhone,
                ViewModel.OrderEditor.ViewModel.BorrowedPhone);

            logger.LogDebug("CustomerId: '{OldValue}' -> '{NewValue}'", ViewModel.Order.CustomerId,
                ViewModel.OrderEditor.ViewModel.CustomerId);

            ViewModel.Order.HandInWhen = ViewModel.OrderEditor.ViewModel.HandInWhen;
            ViewModel.Order.ReturnedWhen = ViewModel.OrderEditor.ViewModel.ReturnedWhen;
            ViewModel.Order.HandInWhat = ViewModel.OrderEditor.ViewModel.HandInWhat;
            ViewModel.Order.RepairWhat = ViewModel.OrderEditor.ViewModel.RepairWhat;
            ViewModel.Order.IsOrderComplete = ViewModel.OrderEditor.ViewModel.IsOrderComplete;
            ViewModel.Order.BorrowedPhone = ViewModel.OrderEditor.ViewModel.BorrowedPhone;
            ViewModel.Order.CustomerId = ViewModel.OrderEditor.ViewModel.CustomerId;
        }

        private void ApplyOrderToEditor()
        {
            ViewModel.OrderEditor.ViewModel.HandInWhenPicker.ViewModel.SetSelectedDateTime(ViewModel.Order.HandInWhen);
            ViewModel.OrderEditor.ViewModel.ReturnedWhenPicker.ViewModel.SetSelectedDateTime(ViewModel.Order
                .ReturnedWhen);

            ViewModel.OrderEditor.ViewModel.HandInWhat = ViewModel.Order.HandInWhat;
            ViewModel.OrderEditor.ViewModel.RepairWhat = ViewModel.Order.RepairWhat;
            ViewModel.OrderEditor.ViewModel.IsOrderComplete = ViewModel.Order.IsOrderComplete;
            ViewModel.OrderEditor.ViewModel.BorrowedPhone = ViewModel.Order.BorrowedPhone ?? string.Empty;
            ViewModel.OrderEditor.ViewModel.CustomersGrid.ViewModel.SelectedCustomerId = ViewModel.Order.CustomerId;
        }

        internal override async Task DeleteClicked(object sender, RoutedEventArgs e)
        {
            ContentDialogResult result = await ShowDeleteConfirmation(
                "Delete order?", "This will permanently delete the current order.");

            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            await queryService.DeleteEntityById(ViewModel.Order.Id);

            logger.LogInformation("Deleted order {OrderId}", ViewModel.Order.Id);

            NavigateBack();
        }

        protected override void SetEditorReadOnly(bool isReadOnly)
        {
            ViewModel.OrderEditor.ViewModel.IsReadOnly = isReadOnly;
        }

        protected override async Task SaveChanges()
        {
            ApplyEditorValuesToOrder();

            await queryService.UpdateEntity(ViewModel.Order!);

            logger.LogDebug(
                "Order changes saved to Db. Customer navigation after save: Id={CustomerId}, Name='{CustomerName}'",
                ViewModel.Order.Customer?.Id, ViewModel.Order.Customer?.Name);
        }

        protected override void ApplyEntityToEditor()
        {
            ApplyOrderToEditor();
        }

        protected override void UpdateHasChanges()
        {
            ViewModel.HasChanges = ViewModel.OrderEditor.ViewModel.HandInWhen != ViewModel.Order.HandInWhen ||
                                   ViewModel.OrderEditor.ViewModel.ReturnedWhen != ViewModel.Order.ReturnedWhen ||
                                   ViewModel.OrderEditor.ViewModel.HandInWhat != ViewModel.Order.HandInWhat ||
                                   ViewModel.OrderEditor.ViewModel.RepairWhat != ViewModel.Order.RepairWhat ||
                                   ViewModel.OrderEditor.ViewModel.IsOrderComplete != ViewModel.Order.IsOrderComplete ||
                                   ViewModel.OrderEditor.ViewModel.BorrowedPhone !=
                                   (ViewModel.Order.BorrowedPhone ?? string.Empty) ||
                                   ViewModel.OrderEditor.ViewModel.CustomerId != ViewModel.Order.CustomerId;

            UpdateSaveAndCancelText();
        }

        protected override void NavigateBack()
        {
            ViewModel.Arguments.NavigationService.NavigateBack();
        }

        protected override void LogSaveError(Exception exception)
        {
            logger.LogError(exception, "Failed to save changes to the order.");
        }
    }
}
