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
        }

        internal void PrintClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Call print service for ViewModel.Order.
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
    }
}
