using Repair.Frontend.Presentation.Core;

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
    }
}
