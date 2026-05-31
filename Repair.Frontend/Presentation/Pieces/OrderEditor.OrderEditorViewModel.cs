namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrderEditor
{
    internal sealed partial class OrderEditorViewModel(OrderEditorArguments arguments) : ObservableObject
    {
        internal OrderEditorArguments Arguments { get; } = arguments;

        public event EventHandler? IsReadOnlyChanged;

        internal DateTimePicker HandInWhenPicker { get; set; } = null!;
        internal DateTimePicker ReturnedWhenPicker { get; set; } = null!;
        internal CheckBox IsOrderCompleteCheckBox { get; set; } = null!;
        internal CheckBox HasBorrowedPhoneCheckBox { get; set; } = null!;
        internal TextBox HandInWhatTextBox { get; set; } = null!;
        internal TextBox RepairWhatTextBox { get; set; } = null!;
        internal CustomersGrid CustomersGrid { get; set; } = null!;

        [ObservableProperty] private string handInWhat = arguments.Order?.HandInWhat ?? string.Empty;
        [ObservableProperty] private string repairWhat = arguments.Order?.RepairWhat ?? string.Empty;
        [ObservableProperty] private bool isOrderComplete = arguments.Order?.IsOrderComplete ?? false;
        [ObservableProperty] private bool hasBorrowedPhone = arguments.Order?.HasBorrowedPhone ?? false;
        [ObservableProperty] private bool isReadOnly = true;

        public DateTime HandInWhen => Arguments.Order?.HandInWhen ?? DateTime.Now;
        public DateTime? ReturnedWhen => Arguments.Order?.ReturnedWhen;
        public int CustomerId => Arguments.Order?.CustomerId ?? 0;

        partial void OnIsReadOnlyChanged(bool value)
        {
            IsReadOnlyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
