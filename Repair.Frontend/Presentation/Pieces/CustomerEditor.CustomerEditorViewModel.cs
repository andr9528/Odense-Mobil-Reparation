namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class CustomerEditor
{
    internal sealed partial class CustomerEditorViewModel : ObservableObject
    {
        public event EventHandler? NameChanged;
        public event EventHandler? PhoneChanged;
        public event EventHandler? EmailChanged;
        public event EventHandler? IsReadOnlyChanged;

        [ObservableProperty] private string name = string.Empty;

        [ObservableProperty] private string phone = string.Empty;

        [ObservableProperty] private string email = string.Empty;

        [ObservableProperty] private bool isReadOnly = true;

        public TextBox NameTextBox { get; set; } = null!;

        public TextBox PhoneTextBox { get; set; } = null!;

        public TextBox EmailTextBox { get; set; } = null!;

        partial void OnNameChanged(string value)
        {
            NameChanged?.Invoke(this, EventArgs.Empty);
        }

        partial void OnPhoneChanged(string value)
        {
            PhoneChanged?.Invoke(this, EventArgs.Empty);
        }

        partial void OnEmailChanged(string value)
        {
            EmailChanged?.Invoke(this, EventArgs.Empty);
        }

        partial void OnIsReadOnlyChanged(bool value)
        {
            IsReadOnlyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
