namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class NullableBooleanOptionBar
{
    internal sealed partial class NullableBooleanOptionBarViewModel : ObservableObject
    {
        internal event EventHandler? SelectionChanged;

        [ObservableProperty] private bool? selectedValue = null;

        [ObservableProperty] private string header = string.Empty;

        public RadioButton YesButton { get; set; } = null!;

        public RadioButton NoButton { get; set; } = null!;

        public RadioButton AnyButton { get; set; } = null!;

        public NullableBooleanOptionBarViewModel(string header)
        {
            Header = header;
        }

        partial void OnSelectedValueChanged(bool? value)
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
