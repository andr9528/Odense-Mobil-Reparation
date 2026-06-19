using System.ComponentModel;
using Repair.Frontend.Presentation.Core;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrderEditor
{
    internal sealed class OrderEditorLogic : BaseLogic<OrderEditorViewModel>
    {
        public OrderEditorLogic(OrderEditorViewModel viewModel) : base(viewModel)
        {
            ViewModel.IsReadOnlyChanged += OnIsReadOnlyChanged;
        }

        private void OnIsReadOnlyChanged(object? sender, EventArgs e)
        {
            UpdateReadOnlyState();
        }

        internal void UpdateReadOnlyState()
        {
            bool isEditable = !ViewModel.IsReadOnly;

            ViewModel.HandInWhatTextBox.IsReadOnly = ViewModel.IsReadOnly;
            ViewModel.RepairWhatTextBox.IsReadOnly = ViewModel.IsReadOnly;
            ViewModel.BorrowedPhoneTextBox.IsReadOnly = ViewModel.IsReadOnly;

            ViewModel.IsOrderCompleteCheckBox.IsEnabled = isEditable;

            ViewModel.HandInWhenPicker.ViewModel.DateButton.IsHitTestVisible = isEditable;
            ViewModel.HandInWhenPicker.ViewModel.TimeButton.IsHitTestVisible = isEditable;

            ViewModel.ReturnedWhenPicker.ViewModel.DateButton.IsHitTestVisible = isEditable;
            ViewModel.ReturnedWhenPicker.ViewModel.TimeButton.IsHitTestVisible = isEditable;

            ViewModel.CustomersGrid.ViewModel.DataGrid.IsHitTestVisible = isEditable;
        }

        internal void RegisterCustomerSelectionIndicator()
        {
            ViewModel.CustomersGrid.ViewModel.PropertyChanged += CustomersGridViewModelPropertyChanged;
            ViewModel.CustomersGrid.ViewModel.Customers.CollectionChanged += (_, _) => UpdateSelectedCustomerText();

            UpdateSelectedCustomerText();
        }

        private void CustomersGridViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ViewModel.CustomersGrid.ViewModel.SelectedCustomerId))
                return;

            UpdateSelectedCustomerText();
        }

        private void UpdateSelectedCustomerText()
        {
            Customer? customer =
                ViewModel.CustomersGrid.ViewModel.Customers.FirstOrDefault(x =>
                    x.Id == ViewModel.CustomersGrid.ViewModel.SelectedCustomerId);

            ViewModel.SelectedCustomerText = customer is null
                ? "Current Selected Customer: Missing Selection"
                : $"Current Selected Customer: {customer.Name}";
        }
    }
}
