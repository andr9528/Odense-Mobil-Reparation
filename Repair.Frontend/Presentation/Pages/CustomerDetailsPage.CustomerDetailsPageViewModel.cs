using Repair.Frontend.Presentation.Pieces;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerDetailsPage
{
    private sealed partial class CustomerDetailsPageViewModel(CustomerDetailsPageArguments arguments) : ObservableObject
    {
        internal event EventHandler? CustomerChanged;

        public CustomerDetailsPageArguments Arguments { get; } = arguments;

        [ObservableProperty] private Customer customer;
        [ObservableProperty] private bool isEditing;
        [ObservableProperty] private bool hasChanges;

        partial void OnCustomerChanged(Customer value)
        {
            CustomerChanged?.Invoke(this, EventArgs.Empty);
        }

        public CustomerEditor CustomerEditor { get; set; } = null!;
        public OrderGrid OrderGrid { get; set; } = null!;
        public CheckBox EditCheckBox { get; set; } = null!;
        public Button SaveButton { get; set; } = null!;
        public Button CancelButton { get; set; } = null!;
    }
}
