using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Frontend.Presentation.Core;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderDetailsPage
{
    private sealed class OrderDetailsPageLogic : BaseLogic<OrderDetailsPageViewModel>
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
            ViewModel.OrderEditor.ViewModel.HasBorrowedPhoneChanged += OrderEditorChanged;
            ViewModel.OrderEditor.ViewModel.CustomersGrid.ViewModel.CustomerSelectionChanged += OrderEditorChanged;
        }

        private void OrderEditorChanged(object? sender, EventArgs e)
        {
            UpdateHasChanges();
        }

        internal void EditCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            bool isEditing = ViewModel.EditCheckBox.IsChecked == true;

            ViewModel.IsEditing = isEditing;
            ViewModel.OrderEditor.ViewModel.IsReadOnly = !isEditing;
            ViewModel.CanDelete = isEditing;
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

        internal async Task SaveClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ViewModel.HasChanges)
                {
                    ViewModel.Arguments.NavigationService.NavigateBack();
                    return;
                }

                ApplyEditorValuesToOrder();

                await queryService.UpdateEntity(ViewModel.Order!);

                logger.LogDebug(
                    "Order changes saved to Db. Customer navigation after save: Id={CustomerId}, Name='{CustomerName}'",
                    ViewModel.Order.Customer?.Id, ViewModel.Order.Customer?.Name);

                UpdateHasChanges();
                DisableEditing();
            }
            catch (Exception exe)
            {
                logger.LogError(exe, "Failed to save changes to the order.");
            }
        }

        internal void CancelClicked(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.HasChanges)
            {
                ViewModel.Arguments.NavigationService.NavigateBack();
                return;
            }

            ApplyOrderToEditor();
            DisableEditing();
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

            logger.LogDebug("HasBorrowedPhone: '{OldValue}' -> '{NewValue}'", ViewModel.Order.HasBorrowedPhone,
                ViewModel.OrderEditor.ViewModel.HasBorrowedPhone);

            logger.LogDebug("CustomerId: '{OldValue}' -> '{NewValue}'", ViewModel.Order.CustomerId,
                ViewModel.OrderEditor.ViewModel.CustomerId);

            ViewModel.Order.HandInWhen = ViewModel.OrderEditor.ViewModel.HandInWhen;
            ViewModel.Order.ReturnedWhen = ViewModel.OrderEditor.ViewModel.ReturnedWhen;
            ViewModel.Order.HandInWhat = ViewModel.OrderEditor.ViewModel.HandInWhat;
            ViewModel.Order.RepairWhat = ViewModel.OrderEditor.ViewModel.RepairWhat;
            ViewModel.Order.IsOrderComplete = ViewModel.OrderEditor.ViewModel.IsOrderComplete;
            ViewModel.Order.HasBorrowedPhone = ViewModel.OrderEditor.ViewModel.HasBorrowedPhone;
            ViewModel.Order.CustomerId = ViewModel.OrderEditor.ViewModel.CustomerId;
        }

        private void DisableEditing()
        {
            ViewModel.IsEditing = false;
            ViewModel.EditCheckBox.IsChecked = false;
            ViewModel.OrderEditor.ViewModel.IsReadOnly = true;
            ViewModel.CanDelete = false;
        }

        private void ApplyOrderToEditor()
        {
            ViewModel.OrderEditor.ViewModel.HandInWhenPicker.ViewModel.SetSelectedDateTime(ViewModel.Order.HandInWhen);
            ViewModel.OrderEditor.ViewModel.ReturnedWhenPicker.ViewModel.SetSelectedDateTime(ViewModel.Order
                .ReturnedWhen);

            ViewModel.OrderEditor.ViewModel.HandInWhat = ViewModel.Order.HandInWhat;
            ViewModel.OrderEditor.ViewModel.RepairWhat = ViewModel.Order.RepairWhat;
            ViewModel.OrderEditor.ViewModel.IsOrderComplete = ViewModel.Order.IsOrderComplete;
            ViewModel.OrderEditor.ViewModel.HasBorrowedPhone = ViewModel.Order.HasBorrowedPhone;
            ViewModel.OrderEditor.ViewModel.CustomersGrid.ViewModel.SelectedCustomerId = ViewModel.Order.CustomerId;
        }

        private void UpdateHasChanges()
        {
            ViewModel.HasChanges = ViewModel.OrderEditor.ViewModel.HandInWhen != ViewModel.Order.HandInWhen ||
                                   ViewModel.OrderEditor.ViewModel.ReturnedWhen != ViewModel.Order.ReturnedWhen ||
                                   ViewModel.OrderEditor.ViewModel.HandInWhat != ViewModel.Order.HandInWhat ||
                                   ViewModel.OrderEditor.ViewModel.RepairWhat != ViewModel.Order.RepairWhat ||
                                   ViewModel.OrderEditor.ViewModel.IsOrderComplete != ViewModel.Order.IsOrderComplete ||
                                   ViewModel.OrderEditor.ViewModel.HasBorrowedPhone !=
                                   ViewModel.Order.HasBorrowedPhone || ViewModel.OrderEditor.ViewModel.CustomerId !=
                                   ViewModel.Order.CustomerId;

            ViewModel.SaveButtonText = ViewModel.HasChanges ? "Save" : "Okay";
            ViewModel.CancelButtonText = ViewModel.HasChanges ? "Cancel" : "Back";
        }

        internal async Task DeleteClicked(object sender, RoutedEventArgs e)
        {
            ContentDialogResult result = await ShowDeleteConfirmation(
                "Delete order?", "This will permanently delete the current order.");

            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            await queryService.DeleteEntityById(ViewModel.Order.Id);

            logger.LogInformation("Deleted order {OrderId}", ViewModel.Order.Id);

            ViewModel.Arguments.NavigationService.NavigateBack();
        }

        private async Task<ContentDialogResult> ShowDeleteConfirmation(string title, string content)
        {
            ContentDialog dialog = new()
            {
                Title = title,
                Content = content,
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = ViewModel.DeleteButton.XamlRoot,
            };

            return await dialog.ShowAsync();
        }
    }
}
